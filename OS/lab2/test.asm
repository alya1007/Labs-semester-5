org 0x7C00

section .text
start:
    ; Set up the stack and clear registers
    mov ax, 0
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov ss, ax
    mov sp, 0x7C00

    ; Set the initial cursor position
    mov ah, 0x02   ; Set cursor position
    mov bh, 0      ; Page number
    mov dh, 0      ; Row
    mov dl, 0      ; Column (initially 0)
    int 0x10

read_char:
    ; Read a character from the keyboard
    mov ah, 0
    int 0x16

    ; Check if the Enter key was pressed
    cmp al, 0x0D
    je handle_enter

    ; Check if the Backspace key was pressed
    cmp al, 0x08
    je handle_backspace

    ; Display the character on the screen
    mov ah, 0x0E
    int 0x10

    ; Move the cursor one position to the right
    mov ah, 0x02   ; Set cursor position
    mov bh, 0      ; Page number
    mov dh, 0      ; Row
    add dl, 1      ; Move cursor to the right
    int 0x10

    ; Continue reading characters
    jmp read_char

    

handle_backspace:
    ; Check if the cursor is already at the leftmost position
    cmp dl, 0
    je read_char  ; If at the leftmost position, just continue reading

    ; Move the cursor one position to the left
    mov ah, 0x02   ; Set cursor position
    mov bh, 0      ; Page number
    mov dh, 0      ; Row
    sub dl, 1      ; Move cursor to the left
    int 0x10

    ; Clear the character on the screen by overwriting it with a space
    mov ah, 0x0E
    mov al, ' '    ; Space character
    int 0x10

    ; Move the cursor back to the original position
    mov ah, 0x02   ; Set cursor position
    mov bh, 0      ; Page number
    mov dh, 0      ; Row
    int 0x10

    ; Continue reading characters
    jmp read_char

handle_enter:
    ; Infinite loop to halt the program
    jmp $

times 510-($-$$) db 0
dw 0xAA55  ; Boot signature