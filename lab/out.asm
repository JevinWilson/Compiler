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
    /* x */
    movabs $emptyString, %rax    /* emptyString */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    lea -16(%rbp), %rax  /* x */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    movabs $print, %rax    /* builtin function print */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    movq %rsp, %rcx    /*  */
    sub $32, %rsp
    call *%rax  /* function call at line 3 */ 
    add $48, %rsp
    /* Return at line 4 */
    lea -16(%rbp), %rax  /* x */
    movq 0(%rax), %rbx    /*  */
    movq 8(%rax), %rax    /*  */
    push %rax  /* value */
    push %rbx  /* storage class */
    movabs $length, %rax    /* builtin function length */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    movq %rsp, %rcx    /*  */
    sub $32, %rsp
    call *%rax  /* function call at line 4 */ 
    add $48, %rsp
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    pop %rbx  /* storage class */
    pop %rax  /* value */
    /* Epilogue at line 4 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
    /* Epilogue at line 5 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
.section .rdata
emptyString:
    .quad 0  /* length */
.section .data
