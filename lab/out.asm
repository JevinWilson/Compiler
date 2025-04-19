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
    movq $2, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movq $123456789, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    movabs $putv, %rax    /* builtin function putv */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    movq %rsp, %rcx    /*  */
    call *%rax  /* function call at line 2 */ 
    add $32, %rsp
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $16, %rsp
    /* Return at line 3 */
    movq $126, %rax    /*  */
    push %rax  /* value */
    push $0  /* storage class PRIMITIVE*/
    add $8, %rsp   /* discard storage class */
    pop %rax  /* value */
    /* Epilogue at line 3 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
    /* Epilogue at line 4 */
    movq %rbp, %rsp    /*  */
    /* Popping register %rbp... */
    pop %rbp  /* value */
    ret
.section .data
