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

    mov byte [page], 0
    mov byte [c], 0
    
    jmp menu


menu:
    ; mov byte [page], 0
    mov word [line_number], 10

    ; set text video mode
    mov ah, 00h 
    mov al, 2
    int 10h  

    ; print command disclaimer
    call find_current_cursor_position
    
    mov ax, [add2]
	mov es, ax
    mov bh, [page]
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
    mov bh, [page]
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
    mov bh, [page]
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

    ; mov al, [result]
    ; mov byte [string], al

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
    mov bh, [page]
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

    ; mov al, [result]
    ; mov byte [character], al

    ; move value of buffer into character
    mov si, buffer
    mov di, character
    mov cx, 2
    rep movsb

    call newline
    call print_string
    
    ; read character
    mov ah, 00h
    int 16h

    call change_page_number
    jmp menu
    
    jmp end
    
    
print_string:
    ; set graphic video mode
    mov ah, 00h 
    mov al, 3
    int 10h

    mov ax, 1300H; print string
    mov bh, [page]; page number
    mov bl, 03H; text attribute
    mov cx, 4
    mov dh, 1;
    mov dl, 0; column
    mov bp, character; pointer to string
    int 10h

    ret

end:

reboot:
    call change_page_number

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


clear_buffer:
    mov byte [char_counter], 0
    mov byte [si], 0
    mov si, buffer

    ret

change_page_number:
    inc byte [page]
    mov ah, 05h
    mov al, [page]
    int 10h

    ret


find_current_cursor_position:
    mov ah, 03h
    mov bh, byte [page]
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

    second_page db "Second page"
    second_page_length equ 11

    reboot_prompt db "Press r to reboot or any other key to continue: "
    reboot_prompt_length equ 47

    string_prompt db "String: "
    string_prompt_length equ 8

    character_prompt db "Character: "
    character_prompt_length equ 11


section .bss
    string resb 20

    line_number resb 2

    character resb 1

    char_counter resb 1
    result resb 1

    page resb 1
    c resb 1

    add1 resb 2
    add2 resb 2
    buffer resb 100