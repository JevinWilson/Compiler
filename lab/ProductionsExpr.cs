
namespace lab{

public class ProductionsExpr{

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
            new("expr :: orexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    n.nodeType = n["orexp"].nodeType;
                }
            ),

            //boolean OR
            new("orexp :: orexp OROP andexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(n,NodeType.Bool,NodeType.Bool);
                }
            ),
            new("orexp :: andexp"),

            //boolean AND
            new("andexp :: andexp ANDOP relexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(n,NodeType.Bool,NodeType.Bool);
                }
            ),
            new("andexp :: relexp"),

            //relational: x>y
            new("relexp :: bitexp RELOP bitexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(
                        n,
                        NodeType.Bool,
                        NodeType.Int,
                        NodeType.Float,
                        NodeType.String
                    );
                }),
            new("relexp :: bitexp"),

            //bitwise: or, and, xor
            new("bitexp :: bitexp BITOP shiftexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(n,NodeType.Int,NodeType.Int);
                },
                generateCode: (n) => {
                    n["bitexp"].generateCode();
                    n["shiftexp"].generateCode();

                    Asm.add(new OpPop(Register.rcx, null));
                    Asm.add(new OpPop(Register.rax, null));

                    if(n["BITOP"].token.lexeme == "|") {
                        Asm.add(new OpOr(Register.rax, Register.rcx));
                    }
                    if(n["BITOP"].token.lexeme == "&") {
                        Asm.add(new OpAnd(Register.rax, Register.rcx));
                    }
                    if(n["BITOP"].token.lexeme == "^") {
                        Asm.add(new OpXor(Register.rax, Register.rcx));
                    }

                    Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                }
            ),
            new("bitexp :: shiftexp"),

            new("shiftexp :: shiftexp SHIFTOP sumexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(n,NodeType.Int,NodeType.Int);
                },
                generateCode: (n) => {

                    //ex: 4 << 2
 
                     //ex: 4
                     n["shiftexp"].generateCode();
 
                     //ex: 2
                     n["sumexp"].generateCode();
     
                     Asm.add(new OpPop(Register.rcx,null));      //ex: 2
                     Asm.add(new OpPop(Register.rax,null));      //ex: 4
                     if( n["SHIFTOP"].token.lexeme == "<<" ){
                         Asm.add(new OpShl(Register.rax, Register.rcx));
                     }
                     if( n["SHIFTOP"].token.lexeme == ">>" ){
                         Asm.add(new OpShr(Register.rax, Register.rcx));
                     }
                     Asm.add( new OpPush( Register.rax, StorageClass.PRIMITIVE));
                }
            ),
            new("shiftexp :: sumexp"),

            //addition and subtraction
            new("sumexp :: sumexp ADDOP prodexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    var type1 = n["sumexp"].nodeType;
                    var type2 = n["prodexp"].nodeType;
                    var addop = n["ADDOP"].token;
                    if( type1 != type2) {
                        Utils.error(addop,$"type mismatch: {type1} and {type2}");
                    }
                    if( type1 != NodeType.Int && type1 != NodeType.Float && type1 != NodeType.String) {
                        n.print();
                        Utils.error(addop,$"Bad type for operation: {type1}");
                    }
                    if( type1 == NodeType.String && n["ADDOP"].token.lexeme != "+") {
                        Utils.error(addop, $"Can't suptract strings");
                    }
                    n.nodeType = type1;
                },
                generateCode: (n) => {
                    if(n.nodeType == NodeType.Int) {
                        n["sumexp"].generateCode();
                        n["prodexp"].generateCode();

                        Asm.add(new OpPop(Register.rcx, null));
                        Asm.add(new OpPop(Register.rax, null));

                        if(n["ADDOP"].token.lexeme == "+") {
                            Asm.add(new OpAdd(Register.rax, Register.rbx));
                        }

                        if(n["ADDOP"].token.lexeme == "-") {
                            Asm.add(new OpSub(Register.rax, Register.rbx));
                        }
                        Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));

                        if(n["ADDOP"].token.lexeme != "+" && n["ADDOP"].token.lexeme != "-") {
                            throw new Exception("ICE");
                        }
                    } else {
                        n["sumexp"].generateCode();
                        n["prodexp"].generateCode();

                        Asm.add(new OpPopF(Register.xmm1, null));
                        Asm.add(new OpPopF(Register.xmm0, null));

                        if(n["ADDOP"].token.lexeme == "+") {
                            Asm.add(new OpAddF(Register.xmm0, Register.xmm1));
                        }
                        if(n["ADDOP"].token.lexeme == "-") {
                            Asm.add(new OpSubF(Register.xmm0, Register.xmm1));
                        }
                        Asm.add(new OpPushF(Register.xmm0, StorageClass.PRIMITIVE));
                    }
                }
            ),
            new("sumexp :: prodexp"),

            //multiplication, division, modulo
            new("prodexp :: prodexp MULOP powexp",
                setNodeTypes: (n) => {
                    Utils.typeCheck(
                        n,
                        null,
                        NodeType.Int,
                        NodeType.Float
                    );
                },
                generateCode: (n) => {
                    if(n.nodeType == NodeType.Int) {
                        n["prodexp"].generateCode();
                        n["powexp"].generateCode();

                        Asm.add(new OpPop(Register.rbx, null));
                        Asm.add(new OpPop(Register.rax, null));

                        if(n["MULOP"].token.lexeme == "*") {
                            Asm.add(new OpMul(Register.rax, Register.rbx));
                            Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                        }

                        if(n["MULOP"].token.lexeme == "/") {
                            Asm.add(new OpDiv(Register.rax, Register.rbx));
                            Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                        }

                        if(n["MULOP"].token.lexeme == "%") {
                            Asm.add(new OpDiv(Register.rax, Register.rbx));
                            Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                        }
                    } else if (n.nodeType == NodeType.Float) {
                        n["prodexp"].generateCode();
                        n["powexp"].generateCode();

                        Asm.add(new OpPopF(Register.xmm1, null));
                        Asm.add(new OpPopF(Register.xmm0, null));

                        if(n["MULOP"].token.lexeme == "*") {
                            Asm.add(new OpMulF(Register.xmm0, Register.xmm1));
                            Asm.add(new OpPushF(Register.xmm0, StorageClass.PRIMITIVE));
                        }
                        if(n["MULOP"].token.lexeme == "/") {
                            Asm.add(new OpDivF(Register.xmm0, Register.xmm1));
                            Asm.add(new OpPushF(Register.xmm0, StorageClass.PRIMITIVE));
                        }
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
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    
                    var type1 = n["unaryexp"].nodeType;
                    var bitnotop = n["BITNOTOP"].token;
                    if (type1 != NodeType.Int) {
                        Utils.error(bitnotop, $"Bad type for operation: {type1}");
                    }
                    n.nodeType = type1;
                },
                generateCode: (n) => {
                    n["unaryexp"].generateCode();
                    Asm.add(new OpPop(Register.rax, null));
                    Asm.add(new OpNot(Register.rax));
                    Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                }
            ),
            new("unaryexp :: ADDOP unaryexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    var type1 = n["unaryexp"].nodeType;
                    var addop = n["ADDOP"].token;
                    if (type1 != NodeType.Int && type1 != NodeType.Float) {
                        Utils.error(addop, $"Bad type for operation: {type1}");
                    }
                    n.nodeType = type1;
                },
                generateCode: (n) => {
                    if(n.nodeType == NodeType.Int) {
                        if(n["ADDOP"].token.lexeme == "+") {
                            n["unaryexp"].generateCode();
                        }
                        if(n["ADDOP"].token.lexeme == "-") {
                            n["unaryexp"].generateCode();
                            Asm.add(new OpPop(Register.rax, null));
                            Asm.add(new OpNeg(Register.rax));
                            Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                        }
                    } else {
                        if(n["ADDOP"].token.lexeme == "+") {
                            n["unaryexp"].generateCode();
                        }
                        if(n["ADDOP"].token.lexeme == "-") {
                            n["unaryexp"].generateCode();
                            //Asm.add(new OpMov((long)0x8000000000000000, Register.rbx));
                            Asm.add(new OpXor(Register.rbx, Register.rax));
                            Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                        }
                    }
                }
            ),
            new("unaryexp :: NOTOP unaryexp",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    var type1 = n["unaryexp"].nodeType;
                    var notop = n["NOTOP"].token;
                    if (type1 != NodeType.Bool) {
                        Utils.error(notop, $"Bad type for operation: {type1}");
                    }
                    n.nodeType = type1;
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
                    string s = n["NUM"].token.lexeme;
                    long v = Int64.Parse(s);
                    Asm.add(new OpMov(v, Register.rax) );
                    Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE) );
                }
            ),
            new("factor :: LPAREN expr RPAREN",
                setNodeTypes: (n) => {
                    n["expr"].setNodeTypes();
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
                    double v = Double.Parse(s);
                    long iv = BitConverter.DoubleToInt64Bits(v);
                    Asm.add(new OpMov(iv, Register.rax));
                    Asm.add(new OpPush(Register.rax, StorageClass.PRIMITIVE));
                }
            ),
            new("factor :: STRINGCONST",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.String;
                }
            ),
            new("factor :: BOOLCONST",
                setNodeTypes: (n) => {
                    n.nodeType = NodeType.Bool;
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