using System.Collections.Generic;

namespace lab
{
    public class ProductionsExpr
    {
        private static void TypeCheck(Token opToken, NodeType leftType, NodeType rightType, string opName,
                                    HashSet<NodeType> permittedTypes = null,
                                    bool checkBool = false)
        {
            if (leftType != rightType)
            {
                Utils.error(opToken, $"Type mismatch for {opName} (left: {leftType}, right: {rightType})");
            }

            if (permittedTypes != null && !permittedTypes.Contains(leftType))
            {
                Utils.error(opToken, $"Bad type for {opName} ({leftType})");
            }
            
            if (checkBool && leftType != NodeType.Bool)
            {
                Utils.error(opToken, $"Operand of {opName} must be Bool, got {leftType}");
            }
        }

        private static void TypeCheckUnary(Token opToken, NodeType type, string opName,
                                    HashSet<NodeType> permittedTypes)
        {
             if (!permittedTypes.Contains(type)) {
                 Utils.error(opToken, $"Bad type for {opName} ({type})");
             }
        }

        public static void makeThem()
        {
            Grammar.defineProductions(new PSpec[] {

                new("expr :: orexp",
                    setNodeTypes: (n) => {
                        n["orexp"].setNodeTypes();
                        n.nodeType = n["orexp"].nodeType;
                    }
                ),

                new("orexp :: orexp OROP andexp",
                    setNodeTypes: (n) => {
                        n["orexp"].setNodeTypes();
                        n["andexp"].setNodeTypes();
                        TypeCheck(n["OROP"].token, n["orexp"].nodeType, n["andexp"].nodeType, "OR",
                                  new HashSet<NodeType> { NodeType.Bool }, true);
                        n.nodeType = NodeType.Bool;
                    }
                ),
                new("orexp :: andexp",
                    setNodeTypes: (n) => {
                        n["andexp"].setNodeTypes();
                        n.nodeType = n["andexp"].nodeType;
                    }
                ),

                new("andexp :: andexp ANDOP relexp",
                    setNodeTypes: (n) => {
                        n["andexp"].setNodeTypes();
                        n["relexp"].setNodeTypes();
                        TypeCheck(n["ANDOP"].token, n["andexp"].nodeType, n["relexp"].nodeType, "AND",
                                  new HashSet<NodeType> { NodeType.Bool }, true);
                        n.nodeType = NodeType.Bool;
                    }
                ),
                new("andexp :: relexp",
                    setNodeTypes: (n) => {
                        n["relexp"].setNodeTypes();
                        n.nodeType = n["relexp"].nodeType;
                    }
                ),

                new("relexp :: bitexp RELOP bitexp",
                    setNodeTypes: (n) => {
                        n.children[0].setNodeTypes();
                        n.children[2].setNodeTypes();
                        TypeCheck(n["RELOP"].token, n.children[0].nodeType, n.children[2].nodeType, "relational operator",
                                  new HashSet<NodeType> { NodeType.Int, NodeType.Float, NodeType.String });
                        n.nodeType = NodeType.Bool;
                    }
                ),
                new("relexp :: bitexp",
                     setNodeTypes: (n) => {
                        n["bitexp"].setNodeTypes();
                        n.nodeType = n["bitexp"].nodeType;
                    }
                ),

                new("bitexp :: bitexp BITOP shiftexp",
                    setNodeTypes: (n) => {
                        n["bitexp"].setNodeTypes();
                        n["shiftexp"].setNodeTypes();
                        TypeCheck(n["BITOP"].token, n["bitexp"].nodeType, n["shiftexp"].nodeType, "bitwise operator",
                                  new HashSet<NodeType> { NodeType.Int });
                        n.nodeType = NodeType.Int;
                    }
                ),
                new("bitexp :: shiftexp",
                     setNodeTypes: (n) => {
                        n["shiftexp"].setNodeTypes();
                        n.nodeType = n["shiftexp"].nodeType;
                    }
                ),

                new("shiftexp :: shiftexp SHIFTOP sumexp",
                    setNodeTypes: (n) => {
                        n["shiftexp"].setNodeTypes();
                        n["sumexp"].setNodeTypes();
                        TypeCheck(n["SHIFTOP"].token, n["shiftexp"].nodeType, n["sumexp"].nodeType, "shift operator",
                                new HashSet<NodeType> { NodeType.Int });
                        n.nodeType = NodeType.Int;
                    }
                ),
                new("shiftexp :: sumexp",
                     setNodeTypes: (n) => {
                        n["sumexp"].setNodeTypes();
                        n.nodeType = n["sumexp"].nodeType;
                    }
                ),

                new("sumexp :: sumexp ADDOP prodexp",
                    setNodeTypes: (n) => {
                        n["sumexp"].setNodeTypes();
                        n["prodexp"].setNodeTypes();
                        var t1 = n["sumexp"].nodeType;
                        var t2 = n["prodexp"].nodeType;
                        var addop = n["ADDOP"].token;

                        if (t1 != t2)
                            Utils.error(addop, $"Type mismatch for add/subtract ({t1} and {t2})");

                        if (t1 != NodeType.Int && t1 != NodeType.Float && t1 != NodeType.String)
                        {
                            Utils.error(addop, $"Bad type for add/subtract ({t1})");
                        }

                        if (t1 == NodeType.String && n["ADDOP"].token.lexeme != "+")
                            Utils.error(addop, "Cannot subtract strings");

                        n.nodeType = t1;
                    }
                ),
                new("sumexp :: prodexp",
                    setNodeTypes: (n) => {
                        n["prodexp"].setNodeTypes();
                        n.nodeType = n["prodexp"].nodeType;
                    }
                ),

                new("prodexp :: prodexp MULOP powexp",
                    setNodeTypes: (n) => {
                        n["prodexp"].setNodeTypes();
                        n["powexp"].setNodeTypes();
                        TypeCheck(n["MULOP"].token, n["prodexp"].nodeType, n["powexp"].nodeType, "multiplication/division/modulo",
                                  new HashSet<NodeType> { NodeType.Int, NodeType.Float });
                        n.nodeType = n["prodexp"].nodeType;
                    }),
                new("prodexp :: powexp",
                    setNodeTypes: (n) => {
                        n["powexp"].setNodeTypes();
                        n.nodeType = n["powexp"].nodeType;
                    }
                ),

                new("powexp :: unaryexp POWOP powexp",
                    setNodeTypes: (n) => {
                        n["unaryexp"].setNodeTypes();
                        n["powexp"].setNodeTypes();
                        TypeCheck(n["POWOP"].token, n["unaryexp"].nodeType, n["powexp"].nodeType, "exponentiation",
                                  new HashSet<NodeType> { NodeType.Int, NodeType.Float });
                        n.nodeType = n["unaryexp"].nodeType;
                    }
                ),
                new("powexp :: unaryexp",
                    setNodeTypes: (n) => {
                        n["unaryexp"].setNodeTypes();
                        n.nodeType = n["unaryexp"].nodeType;
                    }
                ),

                new("unaryexp :: BITNOTOP unaryexp",
                    setNodeTypes: (n) => {
                        n["unaryexp"].setNodeTypes();
                        TypeCheckUnary(n["BITNOTOP"].token, n["unaryexp"].nodeType, "bitwise NOT",
                                       new HashSet<NodeType> { NodeType.Int });
                        n.nodeType = NodeType.Int;
                    }
                ),
                new("unaryexp :: ADDOP unaryexp",
                    setNodeTypes: (n) => {
                        n["unaryexp"].setNodeTypes();
                        var t = n["unaryexp"].nodeType;
                        var addop = n["ADDOP"].token;
                        if (addop.lexeme == "+") {
                             TypeCheckUnary(addop, t, "unary plus",
                                            new HashSet<NodeType> { NodeType.Int, NodeType.Float, NodeType.String });
                        } else {
                             TypeCheckUnary(addop, t, "unary minus",
                                            new HashSet<NodeType> { NodeType.Int, NodeType.Float });
                        }
                        n.nodeType = t;
                    }
                ),
                new("unaryexp :: NOTOP unaryexp",
                    setNodeTypes: (n) => {
                        n["unaryexp"].setNodeTypes();
                        TypeCheckUnary(n["NOTOP"].token, n["unaryexp"].nodeType, "logical NOT",
                                       new HashSet<NodeType> { NodeType.Bool });
                        n.nodeType = NodeType.Bool;
                    }
                ),
                new("unaryexp :: preincexp",
                     setNodeTypes: (n) => {
                        n["preincexp"].setNodeTypes();
                        n.nodeType = n["preincexp"].nodeType;
                    }
                ),

                new("preincexp :: PLUSPLUS preincexp",
                    setNodeTypes: (n) => {
                        n["preincexp"].setNodeTypes();
                        n.nodeType = n["preincexp"].nodeType;
                    }
                ),
                new("preincexp :: postincexp",
                     setNodeTypes: (n) => {
                        n["postincexp"].setNodeTypes();
                        n.nodeType = n["postincexp"].nodeType;
                    }
                ),

                new("postincexp :: postincexp PLUSPLUS",
                    setNodeTypes: (n) => {
                        n["postincexp"].setNodeTypes();
                        n.nodeType = n["postincexp"].nodeType;
                    }
                ),
                new("postincexp :: amfexp",
                     setNodeTypes: (n) => {
                        n["amfexp"].setNodeTypes();
                        n.nodeType = n["amfexp"].nodeType;
                    }
                ),

                new("amfexp :: amfexp DOT factor"
                ),
                new("amfexp :: amfexp LBRACKET expr RBRACKET"
                
                ),
                new("amfexp :: amfexp LPAREN calllist RPAREN"
                ),
                new("amfexp :: factor",
                     setNodeTypes: (n) => {
                        n["factor"].setNodeTypes();
                        n.nodeType = n["factor"].nodeType;
                    }
                ),

                new("factor :: NUM",
                    setNodeTypes: (n) => {
                        n.nodeType = NodeType.Int;
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
                        VarInfo vi = SymbolTable.lookup(tok);
                        n["ID"].varInfo = vi;
                        n["ID"].nodeType = n.nodeType = vi.type;
                    }
                ),
                new("factor :: FNUM",
                    setNodeTypes: (n) => {
                        n.nodeType = NodeType.Float;
                    }
                ),
                new("factor :: STRINGCONST",
                    setNodeTypes: (n) => {
                        n.nodeType = NodeType.String;
                    }),
                new("factor :: BOOLCONST",
                    setNodeTypes: (n) => {
                        n.nodeType = NodeType.Bool;
                    }
                ),

                new("calllist :: lambda",
                    setNodeTypes: (n) => { }
                 ),
                new("calllist :: calllist2 COMMA expr"

                ),
                new("calllist2 :: expr"

                ),
                new("calllist2 :: calllist2 COMMA expr"
                 )
            });
        }
    }
}
