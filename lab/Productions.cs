namespace lab{
    public class Productions{
        public static void makeThem(){
            Grammar.defineProductions( new PSpec[] {
                //declaring a class
                new("S :: decls"),
                new("decls :: funcdecl decls | classdecl decls | vardecl decls | SEMI decls | lambda"),
                new("funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE SEMI",
                    collectFunctionNames: (n) => {
                        string funcName = n.children[1].token.lexeme;
                        Console.WriteLine($"FUNC: {funcName}");
                        SymbolTable.declareGlobal( n["ID"].token, new FunctionNodeType() );
                    },   
                    setNodeTypes: (n) => {
                        SymbolTable.enterFunctionScope();
                        foreach( TreeNode c in n.children){
                            c.setNodeTypes();
                        }
                        n.numLocals = SymbolTable.numLocals;
                        SymbolTable.leaveFunctionScope();
                    },
                    generateCode: (n) => {
                        VarInfo vi = SymbolTable.lookup(n["ID"].token); //lookup the function that we're in
                        var loc = vi.location as GlobalLocation;
                        Asm.add( new OpLabel(loc.lbl));
                        Asm.add( new OpPush( Register.rbp, StorageClass.NO_STORAGE_CLASS));
                        Asm.add( new OpMov( src: Register.rsp, dest: Register.rbp));
                        Console.WriteLine(n.numLocals);
                        if( n.numLocals > 0 ){
                            Asm.add( new OpComment( $"num loc * 16 = {n.numLocals*16}"));
                            Asm.add( new OpSub( Register.rsp, n.numLocals*16 ));
                        }
                        n["stmts"].generateCode();
                        Utils.epilogue(n.lastToken());
                    }
                ),
                new("braceblock :: LBRACE stmts RBRACE",
                    setNodeTypes: (n) => {
                        SymbolTable.enterLocalScope();
                        foreach( TreeNode c in n.children){
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
                    }
                ),
                new("memberdecls :: lambda | SEMI memberdecls | membervardecl memberdecls | memberfuncdecl memberdecls"),
                new("membervardecl :: VAR ID COLON TYPE SEMI"),
                new("memberfuncdecl :: funcdecl"),

                new("stmts :: stmt SEMI stmts"),
                new("stmts :: SEMI"),
                new("stmts :: lambda"),
                new("stmt :: assign | cond | loop | vardecl | return | break | continue"),
                new("break :: BREAK",
                    generateCode: (n) => {
                        TreeNode p = n;
                        while(p != null && p.sym != "loop" )
                            p = p.parent;
                        if( p == null )
                            Utils.error(n["BREAK"].token, "break outside of a loop");
                        Asm.add( new OpJmp( p.exit ) );
                    }
                ),
                new("continue :: CONTINUE",
                    generateCode: (n) => {
                        TreeNode p = n;
                        while(p != null && p.sym != "loop" )
                            p = p.parent;
                        if( p == null )
                            Utils.error(n["CONTINUE"].token, "break outside of a loop");
                        Asm.add( new OpJmp( p.test ) );
                    }
                ),
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
                    },
                    generateCode: (n) => {
                        n.children[0].pushAddressToStack();
                        n.children[2].generateCode();
                        //get the value (rhs) to rax
                        //storage class to rbx
                        Asm.add(new OpPop(Register.rax, Register.rbx));
                        //address of variable is in rcx;
                        //discard storage class (storage class of an
                        //address is 0)
                        Asm.add( new OpPop( Register.rcx, null));
                        //Write data + storage to memory
                        //Storage class first, then data
                        Asm.add( new OpMov( src: Register.rbx, Register.rcx, 0));
                        Asm.add( new OpMov( src: Register.rax, Register.rcx, 8));

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
                            Utils.error(n["LPAREN"].token, "EXPR is not a BOOL in COND->IF");
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

                        Asm.add( new OpPop(Register.rax,null));
                        Utils.epilogue(n["RETURN"].token);
                    }
                ),
                new("return :: RETURN",
                    generateCode: (n) => {
                        Utils.epilogue(n["RETURN"].token);
                    }
                ),

                new("vardecl :: VAR ID COLON TYPE",
                    setNodeTypes: (n) => {
                        var t = NodeType.tokenToNodeType(n["TYPE"].token);
                        if( SymbolTable.currentlyInGlobalScope()){
                            SymbolTable.declareGlobal( n["ID"].token, t);
                        }
                        else{
                            SymbolTable.declareLocal( n.children[1].token, t);
                        }
                    }
                ),
                new("vardecl :: VAR ID COLON TYPE EQ expr",
                    setNodeTypes: (n) => {
                        n["expr"].setNodeTypes();
                        var e = n["expr"].nodeType;
                        var t = NodeType.tokenToNodeType(n["TYPE"].token);
                        var eq = n["EQ"].token;

                        if(e != t){
                            Utils.error(eq, $"Type mismatched ({e} and {t})");
                        }
                        if( SymbolTable.currentlyInGlobalScope() ){
                            SymbolTable.declareGlobal( n["ID"].token, t);
                        }
                        else{
                            SymbolTable.declareLocal( n.children[1].token, t);
                        }
                    }
                ),
                new("vardecl :: VAR ID COLON ID"),  //for user-defined types
                new("vardecl :: VAR ID COLON ID EQ expr"),  //for user-defined type
                });
        }//end makeThem()
    } //end class Productions
} //namespace