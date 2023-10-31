org 0x7c00

start:
    mov ah, 0x0e
    mov al, 'e'
    int 0x10

halt:
jmp halt

times 510-($-$$) db 0

dw 0xAA55