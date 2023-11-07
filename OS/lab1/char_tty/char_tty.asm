go:
    mov AH, 0Eh ; BIOS video function to display a character with teletype
    mov AL, 57h
    int 10h

;;; nasm -f bin -o main.bin main.asm