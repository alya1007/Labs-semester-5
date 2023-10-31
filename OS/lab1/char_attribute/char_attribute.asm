BITS 16
ORG 0x7c00

start:
    mov ah, 0x09
    mov al, 'e'
    mov bl, 4; red color
    int 0x10

halt:
    jmp halt

times 510-($-$$) db 0
dw 0xAA55
