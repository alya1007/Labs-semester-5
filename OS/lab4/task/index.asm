section .text
    global _start

_start:
    ; receive segment:offset pair from the bootloader
    mov [add1], ax
    mov [add2], bx

    mov byte [string], 0

    mov word [character], 0

    mov byte [char_counter], 0
    mov byte [result], 0

    mov byte [c], 0
    
    jmp menu


menu:
    ; set text video mode
    mov ah, 00h 
    mov al, 2
    int 10h 

    call clear_screen

    ; print command disclaimer
    call find_current_cursor_position
    
    mov bh, 0
	mov bl, 07h

    mov si, disclaimer
    add si, word [add1] ; add offset to si
	call print_string_inline ; prints the string pointed by si

    call newline

    ; print reboot option
    ; print command disclaimer
    call find_current_cursor_position
    
    mov bh, 0
	mov bl, 07h
    mov cx, reboot_prompt_length

	mov ax, reboot_prompt
    add ax, word [add1]
	mov bp, ax

	mov ax, 1301h
	int 10h 

    ; read character and store it in al
    mov ah, 00h
    int 16h

    cmp al, 'r'
    je reboot

    call newline

    ; input string
    call find_current_cursor_position
    
    mov bh, 0
	mov bl, 07h
    mov cx, string_prompt_length
	
    mov ax, string_prompt
    add ax, word [add1]
	mov bp, ax

	mov ax, 1301h
	int 10h 

    mov byte [result], 0
    call clear_buffer
    call read_buffer

    ; move value of buffer into string
    mov si, buffer
    mov di, string
    mov cx, 20
    rep movsb ; move string from si to di

    call newline

    ; input character
    call find_current_cursor_position
    
    mov bh, 0
	mov bl, 07h
    mov si, character_prompt
    add si, word [add1]
	call print_string_inline

    mov byte [result], 0
    
    ; read character and store it in al
    mov ah, 00h
    int 16h
    mov byte [character], al 
    ;; print the character back from al
    mov ah, 0eh
    mov bl, 07h
    int 10h

    call newline
    
    ; display result prompt
    call find_current_cursor_position

    mov bh, 0
    mov bl, 07h
    mov cx, result_prompt_length

    mov ax, result_prompt
    add ax, word [add1]
    mov bp, ax

    mov ax, 1301h
    int 10h

    call find_character_occurrence
    
    jmp end


clear_screen:
    mov ah, 0; set the video mode
    mov al, 3; 80x25 text mode
    int 10h
    ret


end:

reboot:
    call clear_screen

    ; set text video mode
    mov ah, 00h 
    mov al, 2
    int 10h 

    ; jump to mini_bootloader
    jmp 0000h:7c00h


read_buffer:
    read_char:
        ; read character
        mov ah, 00h
        int 16h

        ; check if the ENTER key was introduced
        cmp al, 0dh
        je handle_enter

        ; check if the BACKSPACE key was introduced
        cmp al, 08h
        je handle_backspace

        ; add character into the buffer and increment its pointer
        mov [si], al
        inc si
        inc byte [char_counter]

        ; display character as TTY
        mov ah, 0eh
        mov bl, 07h
        int 10h

        jmp read_char
    
    handle_enter:
        mov byte [si], 0 ; add null terminator to the end of the string
        mov si, buffer ; reset pointer to the beginning of the buffer
        jmp end_read_buffer

    handle_backspace:
        call find_current_cursor_position

        cmp byte [char_counter], 0 ; if the buffer is empty
        je read_char

        ; clear last buffer char 
        dec si ; move pointer back by one
        dec byte [char_counter]

        ; move cursor to the left
        mov ah, 02h
        mov bh, 0
        dec dl ; move left by one column
        int 10h

        ; print space instead of the cleared char
        mov ah, 0ah
        mov al, ' '
        mov bh, 0
        mov cx, 1
        int 10h

        jmp read_char

    end_read_buffer:

    ret


find_character_occurrence:
    ; initialize counter
    mov byte [counter], 0

    ; find character occurrence
    mov si, string ; set si to the beginning of the string
    mov al, byte [character] ; character to find
    mov cx, 0 ; occurences counter

    find_occurrence_loop:
        cmp byte [si], 0 ; if the string is empty
        je print_occurrence
        cmp byte [si], al ; if the character is found
        je found_occurrence
        jne loop_end

        found_occurrence:
            inc cx ; increment occurrences counter
        
        loop_end:
            inc si ; move to the next character
            jmp find_occurrence_loop ; continue looping for the next character

    print_occurrence:
        ; Display the occurrences

        cmp cx, 0
        je display_not_found

        ; Convert count (BL register) to string
        pusha
        call find_current_cursor_position
        mov ax, cx ; move count to ax

        mov di, another_buffer
        call int_to_string ; converts string pointed by di (another_buffer) to string

        mov bh, 0           ; Page number
        mov bl, 07h         ; Text attribute
        mov si, another_buffer
        call print_string_inline

        popa

        jmp end_find_character_occurrence


    display_not_found:
        ; Display "not found" message
        call find_current_cursor_position
        
        mov bh, 0
        mov bl, 07h
        mov cx, not_found_message_length

        mov ax, not_found_message
        add ax, word [add1]
        mov bp, ax

        mov ax, 1301h
        int 10h 

        call newline

    end_find_character_occurrence:
        ; listen for key press and then jump to menu
        mov ax, 0

        mov ah, 00h
        int 16h

        jmp menu

        ret


;; Converts uint to string
;; Parameters: ax - uint to convert
;;             di - buffer to store string
;; Returns:    Nothing
;; Mutates:    di
 int_to_string:
    pusha
    mov bx, 10 ; Move constant 10 to bx for division
    mov cx, 0  ; Initialize counter for the number of digits
    .int_to_string_loop:
        xor dx, dx  ; Clear dx for division
        div bx      ; Divide ax by 10, quotient in ax, remainder in dx
        push dx     ; Push remainder (digit) onto stack
        inc cx
        cmp ax, 0
        jne .int_to_string_loop ; If not zero, continue looping
    .int_to_string_loop2:
        pop dx      ; Pop digit from stack to dx
        add dl, '0' ; Convert digit to ASCII character
        mov [di], dl ; Store ASCII character in buffer
        inc di      ; Move to the next position in the buffer
        loop .int_to_string_loop2 ; Continue until all digits are processed
    mov byte [di], 0 ; Null-terminate the string in the buffer
    popa
    ret


clear_buffer:
    mov byte [char_counter], 0 ; reset char counter
    mov byte [si], 0 ; null terminate the buffer
    mov si, buffer ; set si to the beginning of the buffer

    ret

;; Finds the current cursor position
;; Parameters: None
;; Returns:    dh - row
;;             dl - column
find_current_cursor_position:
    push ax
    push bx
    push cx
    mov ah, 03h
    mov bh, byte 0
    int 10h
    pop cx
    pop bx
    pop ax
    ret


newline:
    call find_current_cursor_position ; dh = row, dl = column

    mov ah, 02h
    mov bh, 0
    inc dh ; on the next row compared to the current one
    mov dl, 0 ; at the beginning of the row
    int 10h

    ret

;; Gets string length
;; Parameters: si - pointer to string
;; Returns:    cx - string length
;; Notes       String must be zero terminated
str_len:
    mov cx, 0
    mov [pointer_store], si
    cmp byte [si], 0
    je .str_len_end

    ; increment cx and si until the null terminator is reached
    .str_len_loop:
        inc cx
        inc si
        cmp byte [si], 0
        jne .str_len_loop

    .str_len_end:
        mov si, [pointer_store] ; restore si
        ret

;; Print string at cursor position and move cursor at the end of it
;; Parameters: bh - page number
;;             bl - video attribute http://www.techhelpmanual.com/87-screen_attributes.html
;;             si - pointer to string
;; Returns:    None
print_string_inline:
    pusha
    mov bh, 0
    ;; Get cursor position
    mov ah, 03H
    int 10h
    ;; Get string length
    call str_len ; cx = string length
    mov ax, 1301h
    mov bp, si
    int 10h
    popa
    ret


section .data
    disclaimer db "Find the index of all ocurrences of a character in a string.", 0
    disclaimer_length equ 72

    reboot_prompt db "Press r to reboot or any other key to continue: ", 0
    reboot_prompt_length equ 47

    string_prompt db "String: ", 0
    string_prompt_length equ 8

    character_prompt db "Character: ", 0
    character_prompt_length equ 11

    not_found_message db "Character not found in the string.", 0
    not_found_message_length equ 34

    result_prompt db "Result: ", 0
    result_prompt_length equ 8

    pointer_store dw 0 ; used by str_len to avoid changing extra registers

section .bss
    string resb 20

    character resb 1

    char_counter resb 1
    occurrences resb 1

    result resb 1

    c resb 1

    counter resb 1

    add1 resb 2
    add2 resb 2
    buffer resb 10
    another_buffer resb 10

