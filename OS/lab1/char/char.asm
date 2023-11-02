org 0x7c00

start:
    mov ah, 0Ah
    mov al, 'e'
    mov cx, 6
    int 0x10

halt:
jmp halt

times 510-($-$$) db 0

dw 0xAA55