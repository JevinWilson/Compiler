.section .text
.global _start
_start:
    andq $~0xf, %rsp  /*align the stack*/
    call lbl0  /* main */
    mov %rax, %rcx
    sub $32,%rsp
    call ExitProcess
lbl0:      /* main */
    /* Return at line 2 */
    movq $123, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    ret
