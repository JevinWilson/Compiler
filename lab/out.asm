.section .text
.global _start
_start:
    andq $~0xf, %rsp  /*align the stack*/
    call lbl0  /* main */
    mov %rax, %rcx
    sub $32,%rsp
    call ExitProcess
lbl0:      /* main */
    movq $10, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $5, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rbx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    cmp %rbx, %rax
    setg %al
    movzx %al, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $100, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    ret
.section .data
