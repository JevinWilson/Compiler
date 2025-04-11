.section .text
.global _start
_start:
    andq $~0xf, %rsp  /*align the stack*/
    call lbl0  /* main */
    mov %rax, %rcx
    sub $32,%rsp
    call ExitProcess
lbl0:      /* main */
    /* Binary operation MULOP at line 2 */
    movq $20, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $10, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
     cqo
    idiv %rcx
    push %rdx  /* value */
    push $0  /* storage class PRIMITIVE*/
    /* End of binary operation MULOP at line 2 */
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rbx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    cmp %rbx, %rax
    setl %al
    movzx %al, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    ret
.section .data
