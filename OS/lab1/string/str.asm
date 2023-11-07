BITS 16
ORG 0x7c00

msg db "Hello, World!", 0
msg_len equ $-msg

start:
    mov ax, 0x1300   ; BIOS function to write a string
    mov bl, 0x02     ; Text color (green)
    mov cx, msg_len
    mov dh, 0x01     ; Row 1
    mov dl, 0x00     ; Column 0
    mov bp, msg
    int 0x10
    jmp $

times 510-($-$$) db 0
db 0x55, 0xAA
