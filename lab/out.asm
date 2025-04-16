.section .text
.global _start
_start:
    andq $~0xf, %rsp  /*align the stack*/
    call lbl0  /* main */
    mov %rax, %rcx
    sub $32,%rsp
    call ExitProcess
lbl0:      /* main */
    push %rbp  /* value */
    movq %rsp, %rbp    /*  */
    /* num loc * 16 = 64 */
    sub $64, %rsp
    lea $-16(%rbp), %rax  /* x */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    lea $-32(%rbp), %rax  /* y */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $2, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    test %rax, %rax
    jz lbl1  /* end of if starting at line 7 */
    lea $-64(%rbp), %rax  /* x */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $4, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    lea $-48(%rbp), %rax  /* z */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea $-64(%rbp), %rax  /* x */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
lbl1:      /* end of if starting at line 7 */
    lea $-48(%rbp), %rax  /* z */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea $-16(%rbp), %rax  /* x */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    lea $-32(%rbp), %rax  /* y */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    add $8, %rsp   /* discard storage class */
    pop %rbx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    add %rbx, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea $-48(%rbp), %rax  /* z */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    add $8, %rsp   /* discard storage class */
    pop %rbx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    add %rbx, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    /* Return at line 13 */
    lea $-48(%rbp), %rax  /* z */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    /* Epilogue at line 13 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
    /* Epilogue at line 14 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
.section .data
