.section .text
.global _start
_start:
    andq $~0xf, %rsp  /*align the stack*/
    sub $32, %rsp
    call rtinit
    add $32, %rsp
    call lbl0  /* main */
    mov %rax, %r13
    sub $32, %rsp
    call rtcleanup
    add $32, %rsp
    mov %r13, %rax
    mov %rax, %rcx
    sub $32,%rsp
    call ExitProcess
lbl0:      /* main */
    push %rbp  /* value */
    movq %rsp, %rbp    /*  */
    /* total */
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    /* n */
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea -32(%rbp), %rax  /* n */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $1000000000, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    lea -16(%rbp), %rax  /* total */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
lbl1:      /* top of while loop at line 6 */
    lea -32(%rbp), %rax  /* n */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    movq $0, %rax    /*  */
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
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    test %rax, %rax
    jz lbl2  /* end of while loop at line 6 */
    lea -16(%rbp), %rax  /* total */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea -16(%rbp), %rax  /* total */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    lea -32(%rbp), %rax  /* n */
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
    lea -32(%rbp), %rax  /* n */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea -32(%rbp), %rax  /* n */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rbx  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    sub %rbx, %rax
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    jmp lbl1  /* top of while loop at line 6 */
lbl2:      /* end of while loop at line 6 */
    movq $10, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea -16(%rbp), %rax  /* total */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    movabs $putv, %rax    /*  builtin function putv */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    movq %rsp, %rcx    /*  */
    sub $32, %rsp
    call *%rax   /* function call at line 10 */
    add $64, %rsp
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $16, %rsp
    /* Return at line 11 */
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    /* Epilogue at line 11 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
    /* Epilogue at line 12 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
.section .rdata
emptyString:
    .quad 0  /* length */
.section .data
