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
    /* num loc * 16 = 16 */
    sub $16, %rsp
lbl3:      /* top of while loop at line 4 */
    lea -16(%rbp), %rax  /* i */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $5, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    add $8, %rsp   /* discard storage class */
    pop %rcx  /* value */
    movq %rbx, 0(%rcx)    /*  */
    movq %rax, 8(%rcx)    /*  */
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    test %rax, %rax
    jz lbl4  /* end of if starting at line 6 */
    /* Return at line 7 */
    movq $4, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    /* Epilogue at line 7 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
    movq $1, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    test %rax, %rax
    jz lbl5  /* end of if starting at line 8 */
    jmp lbl1  /* end of test comparison at line 13 */
lbl5:      /* end of if starting at line 8 */
lbl4:      /* end of if starting at line 6 */
    /* Return at line 12 */
    movq $2, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    /* Epilogue at line 12 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
lbl1:      /* end of test comparison at line 13 */
    movq $0, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    test %rax, %rax
    jz lbl3  /* top of while loop at line 4 */
lbl2:      /* end of while loop at line lbl1 */
    /* Epilogue at line 14 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
.section .data
