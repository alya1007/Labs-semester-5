section .text
    global _start

_start:
    ; receive segment:offset pair from the bootloader
    mov [add1], ax
    mov [add2], bx

    mov si, [add1]
    mov ds, [add2]

    mov byte [string], 0

    mov word [line_number], 10

    mov word [character], 0

    mov byte [char_counter], 0
    mov byte [result], 0

    mov byte [c], 0
    
    jmp menu


menu:
    mov word [line_number], 10

    ; set text video mode
    mov ah, 00h 
    mov al, 2
    int 10h 

    call clear_screen

    ; print command disclaimer
    call find_current_cursor_position
    
    mov ax, [add2]
	mov es, ax
    mov bh, 0
	mov bl, 07h
    mov cx, disclaimer_length

    mov ax, disclaimer
    add ax, word [add1]
	mov bp, ax

	mov ax, 1301h
	int 10h 

    call newline

    ; print reboot option
    ; print command disclaimer
    call find_current_cursor_position
    
    mov ax, [add2]
	mov es, ax
    mov bh, 0
	mov bl, 07h
    mov cx, reboot_prompt_length

	mov ax, reboot_prompt
    add ax, word [add1]
	mov bp, ax

	mov ax, 1301h
	int 10h 

    ; read character
    mov ah, 00h
    int 16h

    cmp al, 'r'
    je reboot

    call newline

    ; input string
    call find_current_cursor_position
    
    mov ax, [add2]
	mov es, ax
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
    rep movsb

    call newline

    ; input character
    call find_current_cursor_position
    
    mov ax, [add2]
	mov es, ax
    mov bh, 0
	mov bl, 07h
    mov cx, character_prompt_length
	
    mov ax, character_prompt
    add ax, word [add1]
	mov bp, ax

	mov ax, 1301h
	int 10h 

    mov byte [result], 0
    call clear_buffer
    call read_buffer

    ; move value of buffer into character
    mov si, buffer
    mov di, character
    mov cx, 2
    rep movsb

    call newline
    
    ; display result prompt
    call find_current_cursor_position

    mov ax, [add2]
    mov es, ax
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



print_character:
    mov ax, 0

    mov ah, byte [result]
    call int_to_string

    ; print string from di

    mov ax, 1300H; print string
    mov bh, 0; page number
    mov bl, 03H; text attribute
    mov cx, 2
    mov dh, 1;
    mov dl, 0; column
    mov bp, di; pointer to occcurance number
    int 10h

    ret

end:

reboot:
    call clear_screen

    ; set text video mode
    mov ah, 00h 
    mov al, 2
    int 10h 

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
        mov byte [si], 0
        mov si, buffer
        ; call convert_input_int
        jmp end_read_buffer

    handle_backspace:
        call find_current_cursor_position

        cmp byte [char_counter], 0
        je read_char

        ; clear last buffer char 
        dec si
        dec byte [char_counter]

        ; move cursor to the left
        mov ah, 02h
        mov bh, 0
        dec dl
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

    ; get length of the string
    mov si, string
    mov cx, 0

    count_length:
        cmp byte [si], 0
        je end_count_length
        inc si
        inc cx
        jmp count_length
    
    end_count_length:

    ; find character occurrence
    mov si, string
    mov al, byte [character]
    mov byte [occurrences], 0xFF ; Set to indicate occurrence not found

    find_occurrence_loop:
        cmp byte [si], al
        je found_occurrence
        inc byte [counter]
        inc si
        cmp byte [counter], cl ; Check if the entire string has been traversed
        jge occurrence_not_found
        jmp find_occurrence_loop

    found_occurrence:
        mov byte [occurrences], bl ; Store the index of the occurrence (in BL)
        jmp print_occurrence

    occurrence_not_found:
        mov byte [occurrences], 0xFF ; Indicate that character was not found
        jmp print_occurrence

    print_occurrence:
        ; Display the occurrences
        call find_current_cursor_position

        cmp byte [occurrences], 0xFF
        je display_not_found

        ; Convert index (BL register) to string
        pusha
        mov ax, 0
        mov ah, byte [occurrences]

        mov di, another_buffer
        call int_to_string

        ; Print the string (index of occurrence)
        mov ax, 1300h       ; Print string
        mov bh, 0           ; Page number
        mov bl, 07h         ; Text attribute
        mov cx, 2           ; Length of the string in CX
        mov dh, 4           ; Row
        mov dl, 9           ; Column
        mov bp, di          ; Pointer to the string
        int 10h

        popa

        jmp end_find_character_occurrence


    display_not_found:
        ; Display "not found" message
        call find_current_cursor_position
        
        mov ax, [add2]
        mov es, ax
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
        mov ax, 0

        mov ah, 00h
        int 16h

        jmp menu

        ret


int_to_string:
    pusha
    mov bx, 10
    mov cx, 0
    .int_to_string_loop:
        xor dx, dx
        div bx
        push dx
        inc cx
        cmp ax, 0
        jne .int_to_string_loop
    .int_to_string_loop2:
        pop dx
        add dl, '0'
        mov [di], dl
        inc di
        loop .int_to_string_loop2
    mov byte [di], 0
    popa
    ret


clear_buffer:
    mov byte [char_counter], 0
    mov byte [si], 0
    mov si, buffer

    ret


find_current_cursor_position:
    mov ah, 03h
    mov bh, byte 0
    int 10h

    ret


newline:
    call find_current_cursor_position

    mov ah, 02h
    mov bh, 0
    inc dh
    mov dl, 0
    int 10h

    ret


section .data
    disclaimer db "Find the index of all ocurrences of a character in a string."
    disclaimer_length equ 72

    reboot_prompt db "Press r to reboot or any other key to continue: "
    reboot_prompt_length equ 47

    string_prompt db "String: "
    string_prompt_length equ 8

    character_prompt db "Character: "
    character_prompt_length equ 11

    not_found_message db "Character not found in the string."
    not_found_message_length equ 34

    result_prompt db "Result: "
    result_prompt_length equ 8


section .bss
    string resb 20

    line_number resb 2

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

