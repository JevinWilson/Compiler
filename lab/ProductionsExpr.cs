
namespace lab{

public class ProductionsExpr{

    static void binaryOp( TreeNode n, Opcode op, IntRegister whatPush=null){
        if(whatPush == null )
            whatPush = Register.rax;
        Asm.add(new OpComment($"Binary operation {n.children[1].sym} at line {n.children[1].token.line}"));
        n.children[0].generateCode();
        n.children[2].generateCode();
        Asm.add(new OpPop(Register.rcx, null));
        Asm.add(new OpPop(Register.rax, null));
        Asm.add( op );
        Asm.add( new OpPush( whatPush, StorageClass.PRIMITIVE));
        Asm.add(new OpComment($"End of binary operation {n.children[1].sym} at line {n.children[1].token.line}"));
    }

    static void binaryOpF( TreeNode n, Opcode op){
        Asm.add(new OpComment($"Binary operation {n.children[1].sym} at line {n.children[1].token.line}"));
        n.children[0].generateCode();
        n.children[2].generateCode();
        Asm.add(new OpPopF(Register.xmm1, null));
        Asm.add(new OpPopF(Register.xmm0, null));
        Asm.add( op );
        Asm.add( new OpPushF( Register.xmm0, StorageClass.PRIMITIVE));
        Asm.add(new OpComment($"End of binary operation {n.children[1].sym} at line {n.children[1].token.line}"));
    }

    static void unaryOp( TreeNode n, Opcode op){
        Asm.add(new OpComment($"unary operation {n.children[0].sym} at line {n.children[0].token.line}"));
        n.children[1].generateCode();
        Asm.add(new OpPop(Register.rax, null));
        Asm.add( op );
        Asm.add( new OpPush( Register.rax, StorageClass.PRIMITIVE ));
        Asm.add(new OpComment($"End of unary operation {n.children[0].sym} at line {n.children[0].token.line}"));
    }

    static void unaryOpF( TreeNode n, Opcode op){
        Asm.add(new OpComment($"unary operation {n.children[0].sym} at line {n.children[0].token.line}"));
        n.children[1].generateCode();
        Asm.add(new OpPopF(Register.xmm0, null));
        Asm.add( op );
        Asm.add( new OpPushF( Register.xmm0, StorageClass.PRIMITIVE ));
        Asm.add(new OpComment($"End of unary operation {n.children[0].sym} at line {n.children[0].token.line}"));
    }


    static void unary(TreeNode n, NodeType[] operandTypes, NodeType resultType){
        foreach(var c in n.children)
            c.setNodeTypes();
        if( resultType == null )
            n.nodeType = n.children[1].nodeType;
        else
            n.nodeType = resultType;
        foreach(var t in operandTypes){
            if( n.children[1].nodeType == t )
                return;
        }
        Utils.error(n.children[0].token,$"Bad type for operation: {n.children[1].nodeType}");
    }

    static void unary(TreeNode n, NodeType operandType, NodeType resultType){
        unary(n,new NodeType[]{operandType}, resultType);
    }

    static void binary(TreeNode n, NodeType[] operandTypes, NodeType resultType){
        foreach(var c in n.children)
            c.setNodeTypes();
        if( n.children[0].nodeType != n.children[2].nodeType )
            Utils.error(n.children[1].token, $"Different types: {n.children[0].nodeType} and {n.children[2].nodeType}");
        if( resultType == null )
            n.nodeType = n.children[0].nodeType;
        else
            n.nodeType = resultType;
        foreach(var t in operandTypes){
            if( n.children[0].nodeType == t )
                return;
        }
        Utils.error(n.children[1].token,$"Bad type for operation: {n.children[0].nodeType}");
    }

    static void binary(TreeNode n, NodeType operandType, NodeType resultType){
        binary(n,new[]{operandType},resultType);
    }

    public static void makeThem(){

        Grammar.defineProductions( new PSpec[] {

            //convenience: Starts the whole expression hierarchy
            new("expr :: orexp"),

            //boolean OR
            new("orexp :: orexp OROP andexp",
                setNodeTypes: (n) => {
                    binary(n,NodeType.Bool,NodeType.Bool);
                }
            ),
            new("orexp :: andexp"),

            //boolean AND
            new("andexp :: andexp ANDOP relexp",
                setNodeTypes: (n) => {
                    binary(n,NodeType.Bool,NodeType.Bool);
                }
            ),
            new("andexp :: relexp"),

            //relational: x>y
            new("relexp :: bitexp RELOP bitexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children)
                        c.setNodeTypes();
                    var t1 = n.children[0].nodeType; 
                    var t2 = n.children[2].nodeType;
                    var relop = n["RELOP"].token;
                    if(t1 != t2)
                        Utils.error(relop, $"Type mismatch for {relop.sym} ({t1} and {t2})");
                    if(t1 != NodeType.Int && t1 != NodeType.Float && t1 != NodeType.String && t1 != NodeType.Bool)
                        Utils.error(relop,$"Bad type for relop ({t1})");
                    n.nodeType = NodeType.Bool;
                },
                generateCode: (n) => {
                    n.children[0].generateCode();
                    n.children[2].generateCode();

                    var ntype = n["bitexp"].nodeType;
                    if (ntype == NodeType.Int) {
                        Asm.add(new OpPop(Register.rbx, null));  // RHS
                        Asm.add(new OpPop(Register.rax, null));  // LHS

                        string cmp;
                        switch (n["RELOP"].token.lexeme) {
                            case ">": cmp = "g"; break;   
                            case "<": cmp = "l"; break;   
                            case ">=": cmp = "ge"; break; 
                            case "<=": cmp = "le"; break;
                            case "==": cmp = "e"; break; 
                            case "!=": cmp = "ne"; break; 
                            default: Environment.Exit(0);
                            return;
                        }

                        Asm.add(new OpCmp(Register.rax, Register.rbx)); 
                        Asm.add(new OpSetCC(cmp, Register.rax));      
                        Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE)); 
                    } else if( ntype == NodeType.String) {
                        //TBD later
                        throw new Exception();
                    }
                    else if( ntype == NodeType.Bool ) {
                        Asm.add( new OpPop( Register.rbx, null ));  // right operand
                        Asm.add( new OpPop( Register.rax, null ));  // left operand

                        string cmp;
                        switch(n["RELOP"].token.lexeme ){
                            case "==": cmp = "e"; break;
                            case "!=": cmp = "ne"; break;
                            default: Environment.Exit(1);
                            return;
                        }

                        Asm.add( new OpCmp( Register.rax, Register.rbx ));
                        Asm.add( new OpSetCC( cmp, Register.rax ));
                        Asm.add( new OpPush( Register.rax, StorageClass.PRIMITIVE ));
                    }
                }
            ),
            new("relexp :: bitexp"),

            //bitwise: or, and, xor
            new("bitexp :: bitexp BITOP shiftexp",
                setNodeTypes: (n) => {
                    binary(n,NodeType.Int,NodeType.Int);
                },
                generateCode: (n) => {
                    switch( n["BITOP"].token.lexeme){
                        case "&":  binaryOp( n,new OpAnd(Register.rax,Register.rcx)); break;
                        case "|":  binaryOp( n,new OpOr(Register.rax,Register.rcx)); break;
                        case "^":  binaryOp( n, new OpXor(Register.rax,Register.rcx)); break;
                    }
                }
            ),
            new("bitexp :: shiftexp"),

            new("shiftexp :: shiftexp SHIFTOP sumexp",
                setNodeTypes: (n) => {
                    binary(n,NodeType.Int,NodeType.Int);
                },
                generateCode: (n) => {
                    switch(n["SHIFTOP"].token.lexeme){
                        case "<<":
                            binaryOp( n, new OpShl(Register.rax, Register.rcx)); break;
                        case ">>":
                            binaryOp( n, new OpShr(Register.rax, Register.rcx)); break;
                    }
                }
            ),
            new("shiftexp :: sumexp"),

            //addition and subtraction
            new("sumexp :: sumexp ADDOP prodexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    binary( n, 
                        new NodeType[]{NodeType.Int, NodeType.Float, NodeType.String},
                        null
                    );
                    if( n.children[0].nodeType == NodeType.String && n["ADDOP"].token.lexeme != "+" )
                        Utils.error(n.children[0].token,"Cannot subtract strings");
                },
                generateCode: (n) => {
                    if( n.nodeType == NodeType.Int){
                        switch(n["ADDOP"].token.lexeme){
                            case "+": binaryOp( n, new OpAdd( Register.rax, Register.rcx)); break;
                            case "-": binaryOp( n, new OpSub( Register.rax, Register.rcx)); break;
                            default: throw new Exception();
                        }
                    } else if(n.nodeType == NodeType.Float){
                        switch(n["ADDOP"].token.lexeme){
                            case "+": binaryOpF( n, new OpAddF( Register.xmm0, Register.xmm1)); break;
                            case "-": binaryOpF( n, new OpSubF( Register.xmm0, Register.xmm1)); break;
                            default: throw new Exception();
                        }
                    } else {
                        throw new NotImplementedException();

                    }
                }
            ),
            new("sumexp :: prodexp"),

            //multiplication, division, modulo
            new("prodexp :: prodexp MULOP powexp",
                setNodeTypes: (n) => {
                    binary(n,
                        new NodeType[]{NodeType.Int, NodeType.Float},
                        null
                    );
                },
                generateCode: (n) => {
                    if( n.nodeType == NodeType.Int){
                        switch(n["MULOP"].token.lexeme){
                            case "*": binaryOp( n, new OpMul( Register.rax, Register.rcx)); break;
                            case "/": binaryOp( n, new OpDiv( Register.rax, Register.rcx)); break;
                            case "%": binaryOp( n, new OpDiv( Register.rax, Register.rcx), Register.rdx); break;
                            default: throw new Exception();
                        }
                    } else if( n.nodeType == NodeType.Float){
                        switch(n["MULOP"].token.lexeme){
                            case "*": binaryOpF( n, new OpMulF( Register.xmm0, Register.xmm1)); break;
                            case "/": binaryOpF( n, new OpDivF( Register.xmm0, Register.xmm1)); break;
                            case "%": Utils.error(n["MULOP"].token, "Modulo is not allowed on floats"); break;
                            default: throw new Exception();
                        }
                    } else {
                        throw new NotImplementedException();
                    }
                }
            ),
            new("prodexp :: powexp"),

            //exponentiation
            new("powexp :: unaryexp POWOP powexp"),
            new("powexp :: unaryexp"),

            //bitwise not, negation, unary plus
            new("unaryexp :: BITNOTOP unaryexp",
                setNodeTypes: (n) => {
                    unary(n,NodeType.Int,NodeType.Int);
                },
                generateCode: (n) => {
                    unaryOp(n, new OpNot(Register.rax));
                }
            ),
            new("unaryexp :: ADDOP unaryexp",
                setNodeTypes: (n) => {
                    unary(n,new NodeType[]{NodeType.Int,NodeType.Float},null);
                },
                generateCode: (n) => {
                    if( n.nodeType == NodeType.Int){
                        switch(n["ADDOP"].token.lexeme){
                            case "+": n.children[1].generateCode(); break;
                            case "-": unaryOp(n, new OpNeg(Register.rax)); break;
                            default: throw new Exception();
                        }
                    } else if( n.nodeType == NodeType.Float ){
                        switch(n["ADDOP"].token.lexeme){
                            case "+": n.children[1].generateCode(); break;
                            case "-": unaryOpF(n, new OpNegF(Register.xmm0,Register.xmm1)); break;
                            default: throw new Exception();
                        }
                    } else {
                        throw new NotImplementedException();
                    }
                }
            ),
            new("unaryexp :: NOTOP unaryexp",
                setNodeTypes: (n) => {
                    unary(n,NodeType.Bool,NodeType.Bool);
                }
            ),
            new("unaryexp :: preincexp"),

            //preincrement, predecrement
            new("preincexp :: PLUSPLUS preincexp"),
            new("preincexp :: postincexp"),

            //postincrement, postdecrement
            new("postincexp :: postincexp PLUSPLUS"),
            new("postincexp :: amfexp"),

            //array, member, function call
            new("amfexp :: amfexp DOT factor"),
            new("amfexp :: amfexp LBRACKET expr RBRACKET"),
            new("amfexp :: amfexp LPAREN calllist RPAREN"),
            new("amfexp :: factor"),

            //indivisible atom
            new("factor :: NUM",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.Int;
                },
                generateCode: (n) => {
                    long v = Int64.Parse(n["NUM"].token.lexeme);
                    Asm.add(new OpMov(v,Register.rax,""));
                    Asm.add(new OpPush(Register.rax,StorageClass.PRIMITIVE));
                }
            ),
            new("factor :: LPAREN expr RPAREN",
                setNodeTypes: (n) => {
                    foreach(var c in n.children )
                        c.setNodeTypes();
                    n.nodeType = n["expr"].nodeType;
                }
            ),
            new("factor :: ID",
                setNodeTypes: (n) => {
                    var tok = n.children[0].token;
                    var vi =  SymbolTable.lookup(tok);
                    n["ID"].varInfo = vi;
                    n["ID"].nodeType = n.nodeType = vi.type;
                }
            ),
            new("factor :: FNUM",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.Float;
                },
                generateCode: (n) => {
                    string s = n["FNUM"].token.lexeme;
                    double value = Double.Parse(s);
                    long ivalue = BitConverter.DoubleToInt64Bits(value);
                    Asm.add( new OpMov(ivalue, Register.rax, comment: $"{s}"));
                    Asm.add( new OpPush(Register.rax, StorageClass.PRIMITIVE));
                }
            ),

            new("factor :: STRINGCONST",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.String;
                },
                generateCode: (n) => {
                    throw new NotImplementedException();
                }

            ),
            new("factor :: BOOLCONST",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.Bool;
                },
                generateCode: (n) => {
                    var value =  n["BOOLCONST"].token.lexeme;
                    if(value == "true")
                    {
                        Asm.add(new OpMov(1, Register.rax));
                        Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                    }
                    else
                    {
                        Asm.add(new OpMov(0, Register.rax));
                        Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                    }
                }
            ),

            //function call
            //calllist = zero or more arguments
            //calllist2 = 1 or more arguments
            new("calllist :: lambda"),
            new("calllist :: calllist2 COMMA expr"),
            new("calllist2 :: expr"),
            new("calllist2 :: calllist2 COMMA expr")

        });

    }
}
}