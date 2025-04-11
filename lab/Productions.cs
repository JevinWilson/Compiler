
namespace lab{

public class Productions{
    public static void makeThem(){
        Grammar.defineProductions( new PSpec[] {
            new("S :: decls"),
            new("decls :: funcdecl decls | classdecl decls | vardecl decls | SEMI decls | lambda"),
            new("funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE SEMI",
                collectClassNames: (n) => {
                    string funcName = n.children[1].token.lexeme;
                    Console.WriteLine($"FUNC: {funcName}");
                },           
                setNodeTypes: (n) => {
                    
                    SymbolTable.declareGlobal(n["ID"].token, new FunctionNodeType() );
                    SymbolTable.enterFunctionScope();
                    foreach( TreeNode c in n.children){
                        c.setNodeTypes();
                    }
                    SymbolTable.leaveFunctionScope();
                    
                },
                generateCode: (n) => {
                    VarInfo vi = SymbolTable.lookup( n["ID"].token );
                    var loc = (vi.location as GlobalLocation);
                    Asm.add( new OpLabel( loc.lbl ) );
                    n["stmts"].generateCode();
                    Asm.add(new OpRet());
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
                    var t = NodeType.tokenToNodeType(n["TYPE"].token);
                    if( SymbolTable.currentlyInGlobalScope()){
                        SymbolTable.declareGlobal( n["ID"].token, t);
                    }
                    else{
                        SymbolTable.declareParameter( n["ID"].token, t);
                    }
                }
            ),
            new("classdecl :: CLASS ID LBRACE memberdecls RBRACE SEMI",
                collectClassNames: (TreeNode n) => {
                    string className = n.children[1].token.lexeme;
                    Console.WriteLine($"CLASS: {className}");
                    //assuming no nested classes; no need to walk
                    //children of n
                    //This also means we won't pick up member
                    //functions of the class.
                }
            ),
            new("memberdecls :: lambda | SEMI memberdecls | membervardecl memberdecls | memberfuncdecl memberdecls"),
            new("membervardecl :: VAR ID COLON TYPE SEMI"),
            new("memberfuncdecl :: funcdecl"),

            new("stmts :: stmt SEMI stmts"),
            new("stmts :: SEMI"),
            new("stmts :: lambda"),
            new("stmt :: assign | cond | loop | vardecl | return | break | continue"),

            new( "break :: BREAK",
                generateCode: (n) => {
                    TreeNode x = n;
                    while( x != null && x.sym != "loop" ){
                        x=x.parent;
                    }
                    if( x == null ){
                        Utils.error(n["BREAK"].token, "Break not inside a loop");
                    }
                    Asm.add( new OpJmp( x.exit ));
                }
            ),

            new( "continue :: CONTINUE",
                generateCode: (n) => {
                    TreeNode x = n;
                    while(x != null && x.sym != "loop" )
                        x = x.parent;
                    if( x == null )
                        Utils.error(n["CONTINUE"].token, "break outside of a loop");
                    Asm.add( new OpJmp( x.test ) );
                }
            ),

            new("assign :: expr EQ expr",
                setNodeTypes: (n) => {
                    var type1 = n.children[0];
                    var type2 = n.children[2];
                    var eq = n.children[1].token;
                    type1.setNodeTypes();
                    type2.setNodeTypes();
                    if(type1.nodeType != type2.nodeType){
                        Utils.error(eq, $"Node type mismatch! ({n.children[0].nodeType} and {n.children[2].nodeType})");
                    }
                }
            ),
            new("cond :: IF LPAREN expr RPAREN braceblock",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    n["expr"].setNodeTypes();
                    var tmp = n["expr"].nodeType;
                    if(tmp != NodeType.Bool){
                        Utils.error(n["LPAREN"].token, "is not a BOOL in COND->IF/ELSE");
                    }
                    n.nodeType = tmp;
                },
                generateCode: (n) => {

                    var endifLabel = new Label($"end of if starting at line {n["IF"].token.line}");
                    
                    //make code for expr; leave result on stack
                    n["expr"].generateCode();

                    //get result into rax, discard storage class
                    Asm.add( new OpPop( Register.rax, null) );
                    Asm.add( new OpJmpIfZero( Register.rax, endifLabel) );

                    n["braceblock"].generateCode();
                    Asm.add( new OpLabel( endifLabel) );
                }
            ),
            new("cond :: IF LPAREN expr RPAREN braceblock ELSE braceblock",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    n["expr"].setNodeTypes();
                    var tmp = n["expr"].nodeType;
                    if(tmp != NodeType.Bool){
                        Utils.error(n["LPAREN"].token, "EXPR is not a BOOL in COND->IF/ELSE");
                    }
                    n.nodeType = tmp;
                },
                generateCode: (n) => {

                    var elseLabel = new Label($"else at line {n["ELSE"].token.line}");
                    var endifLabel = new Label($"end of if starting at line {n["IF"].token.line}");
                    
                    //make code for expr; leave result on stack
                    n["expr"].generateCode();

                    //get result into rax, discard storage class
                    Asm.add(new OpPop(Register.rax, null));
                    Asm.add( new OpJmpIfZero( Register.rax, elseLabel));
                    n.children[4].generateCode();
                    Asm.add( new OpJmp( endifLabel ));
                    Asm.add( new OpLabel( elseLabel ));
                    n.children[6].generateCode();
                    Asm.add( new OpLabel( endifLabel));
                }
            ),
            new("loop :: WHILE LPAREN expr RPAREN braceblock",
                setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    n["expr"].setNodeTypes();
                    var tmp = n["expr"].nodeType;
                    if(tmp != NodeType.Bool){
                        Utils.error(n["LPAREN"].token, "EXPR is not a BOOL in LOOP->WHILE");
                    }
                    n.nodeType = tmp;
                },
                generateCode: (n) => {
                    int line = n["WHILE"].token.line;
                    var topLoop = new Label($"top of while loop at line {line}");
                    var bottomLoop = new Label($"end of while loop at line {line}");

                    n.entry = topLoop; 
                    n.exit = bottomLoop;
                    n.test = topLoop;

                    Asm.add( new OpLabel(topLoop));
                    n["expr"].generateCode();
                    Asm.add( new OpPop( Register.rax, null));
                    Asm.add( new OpJmpIfZero( Register.rax, bottomLoop));
                    n["braceblock"].generateCode();
                    Asm.add( new OpJmp( topLoop));
                    Asm.add( new OpLabel( bottomLoop));
                }

            ),
            new("loop :: REPEAT braceblock UNTIL LPAREN expr RPAREN",
            setNodeTypes: (n) => {
                    foreach(var c in n.children){
                        c.setNodeTypes();
                    }
                    n["expr"].setNodeTypes();
                    var tmp = n["expr"].nodeType;
                    if(tmp != NodeType.Bool){
                        Utils.error(n["LPAREN"].token, "EXPR is not a BOOL in LOOP->REPEAT/UNTIL");
                    }
                    n.nodeType = tmp;
                },
                generateCode: (n) => {
                    var line = new Label($"end of test comparison at line {n["UNTIL"].token.line}");
                    var bottomLoop = new Label($"end of while loop at line {line}");
                    var topLoop = new Label($"top of while loop at line {n["REPEAT"].token.line}");
                    
                    n.entry = topLoop; 
                    n.exit = bottomLoop;
                    n.test = line;

                    Asm.add( new OpLabel(topLoop));
                    n["braceblock"].generateCode();
                    Asm.add( new OpLabel(line));
                    n["expr"].generateCode();
                    Asm.add( new OpPop( Register.rax, null));
                    Asm.add( new OpJmpIfZero( Register.rax, topLoop));
                    
                    Asm.add( new OpLabel( bottomLoop));

                }
            ),
            
            new("return :: RETURN expr",
                generateCode: (n) => {

                    Asm.add(new OpComment( 
                            $"Return at line {n.children[0].token.line}"));
                    n["expr"].generateCode();   //leaves value on top of stack

                    //ABI says return values come back in rax
                    Asm.add( new OpPop(Register.rax,null));
                    Asm.add( new OpRet());
                }
            ),
            new("return :: RETURN",
                generateCode: (n) => {
                    Asm.add( new OpRet() );
                }
            ),


            new("vardecl :: VAR ID COLON TYPE",
                setNodeTypes:(n) => {
                    var t = NodeType.tokenToNodeType(n["TYPE"].token) ;
                    if( SymbolTable.currentlyInGlobalScope()){
                        SymbolTable.declareGlobal( n["ID"].token, t);
                    } else {
                        SymbolTable.declareLocal( n.children[1].token, t );
                    }
                }
            ),
            new("vardecl :: VAR ID COLON TYPE EQ expr",
                setNodeTypes:(n)=>{
                    throw new Exception("FINISH ME");
                }
            ),
            new("vardecl :: VAR ID COLON ID"),  //for user-defined types
            new("vardecl :: VAR ID COLON ID EQ expr"),  //for user-defined types

        });

    }
}

}