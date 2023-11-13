BITS 16
ORG 0x7c00

msg db "Hello, World+",0
msg_len equ $-msg

start:
    mov ax, 1300h  ; BIOS function to write a string
    mov al, 1; display the entire string and update cursor
    mov bl, 0x03; blue
    mov cx, msg_len
    mov dh, 0x01     ; Row 1
    mov dl, 0x00     ; Column 0
    mov bp, msg
    int 0x10
    jmp $

times 510-($-$$) db 0
db 0x55, 0xAA
