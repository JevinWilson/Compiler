
namespace lab{

public class Productions{
    public static void makeThem(){
        Grammar.defineProductions( new PSpec[] {
            new("S :: decls"),
            new("decls :: funcdecl decls | classdecl decls | vardecl decls | SEMI decls | lambda"),
            new("funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE SEMI",
                setNodeTypes: (n) => {
                    
                    SymbolTable.enterFunctionScope();

                    foreach(TreeNode c in n.children){
                        c.setNodeTypes();
                    }

                    SymbolTable.leaveFunctionScope();
                    
                }
            ),
            new("braceblock :: LBRACE stmts RBRACE",
                setNodeTypes: (n) => {
       
                    SymbolTable.enterLocalScope();

                    foreach(TreeNode c in n.children){
                        c.setNodeTypes();
                    }

                    SymbolTable.leaveLocalScope();
             

                }
            ),
            new("optionalReturn :: lambda | COLON TYPE"),
            new("optionalSemi :: lambda | SEMI"),
            new("optionalPdecls :: lambda | pdecls"),
            new("pdecls :: pdecl | pdecl COMMA pdecls"),
            new("pdecl :: ID COLON TYPE",
                setNodeTypes: (n) => {
                    var type = NodeType.tokenToNodeType(n["TYPE"].token);
                    if( SymbolTable.currentlyInGlobalScope()){
                        SymbolTable.declareGlobal( n["ID"].token, type);
                    }
                    else{
                        SymbolTable.declareParameter( n["ID"].token, type);
                    }
                }
            ),
            new("classdecl :: CLASS ID LBRACE memberdecls RBRACE SEMI",
                collectClassNames: (TreeNode n) => {
                    string className = n.children[1].token.lexeme;
                    Console.WriteLine($"CLASS: {className}");
                    //assuming no nested classes; no need to walk
                    //children of n
                }
            ),
            new("memberdecls :: lambda | SEMI memberdecls | membervardecl memberdecls | memberfuncdecl memberdecls"),
            new("membervardecl :: VAR ID COLON TYPE SEMI"),
            new("memberfuncdecl :: funcdecl"),

            new("stmts :: stmt SEMI stmts"),
            new("stmts :: SEMI"),
            new("stmts :: lambda"),
            new("stmt :: assign | cond | loop | vardecl | return"),
            new("assign :: expr EQ expr",
                setNodeTypes: (n) => {
                    var t1 = n.children[0];
                    var t2 = n.children[2];
                    var eq = n.children[1].token;
                    t1.setNodeTypes();
                    t2.setNodeTypes();
                    if(t1.nodeType != t2.nodeType){
                        Utils.error(eq, $"Node type mismatch! ({n.children[0].nodeType} and {n.children[2].nodeType})");
                    }
                }
            ),
            new("cond :: IF LPAREN expr RPAREN braceblock"),
            new("cond :: IF LPAREN expr RPAREN braceblock ELSE braceblock"),
            new("loop :: WHILE LPAREN expr RPAREN braceblock"),
            new("loop :: REPEAT braceblock UNTIL LPAREN expr RPAREN"),
            new("return :: RETURN expr"),
            new("return :: RETURN"),
            new("vardecl :: VAR ID COLON TYPE",
                setNodeTypes:(n) => {
                    var type = NodeType.tokenToNodeType(n["TYPE"].token) ;
                    if( SymbolTable.currentlyInGlobalScope()){
                        SymbolTable.declareGlobal( n["ID"].token, type);
                    } else {
                        SymbolTable.declareLocal( n.children[1].token, type );
                    }
                }
            ),
            new("vardecl :: VAR ID COLON TYPE EQ expr",
                setNodeTypes: (n) => {
                    n["expr"].setNodeTypes();
                    var expr = n["expr"].nodeType;
                    var type = NodeType.tokenToNodeType(n["TYPE"].token);
                    var eq = n["EQ"].token;

                    if(expr != type){
                        Utils.error(eq, $"Type mismatch! ({expr} and {type})");
                    }
                    if( SymbolTable.currentlyInGlobalScope() ){
                        SymbolTable.declareGlobal( n["ID"].token, type);
                    }
                    else{
                        SymbolTable.declareLocal( n.children[1].token, type);
                    }
                }
            ),
            new("vardecl :: VAR ID COLON ID"),  //for user-defined types
            new("vardecl :: VAR ID COLON ID EQ expr"),  //for user-defined types

        });

    }
}

}