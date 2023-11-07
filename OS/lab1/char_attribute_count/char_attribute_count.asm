BITS 16
ORG 0x7c00

start:
    mov ah, 0x09 ; BIOS video function to display a string
    mov al, 'e'
    mov bl, 4; red color
    mov cx, 4; 4 times
    int 0x10

halt:
    jmp halt

times 510-($-$$) db 0
dw 0xAA55
