
namespace lab{
    
    public static class SymbolTable{
        static int numParameters=0;
        static Stack< HashSet<String> > locals = new();

        public static int numLocals=0;
        public static int nesting=0;
        public static Stack< List<VarInfo> > shadowed = new();

        public static Dictionary<string, VarInfo> table = new();
        public static List<Tuple<string,NodeType>> localTypes = new();

        public static void enterFunctionScope(){ 
            numParameters=0;
            numLocals=0;
            nesting++;
            shadowed.Push(new());
            locals.Push(new());
        }
        public static void leaveFunctionScope(){
            nesting--;
            numLocals=0;        
            removeVariablesFromTableWithNestingGreaterThanThreshold(nesting);
            restoreShadowedVariables();
        }

        public static void enterLocalScope(){
            nesting++;    
            shadowed.Push(new());
            locals.Push(new());
        }
        public static void leaveLocalScope(){
            foreach( string name in locals.Peek() ) {
                table.Remove(name);
            }
            locals.Pop();
            foreach( var vi in shadowed.Pop() ) {
                table[vi.token.lexeme] = vi;
            }
                
        }
        static void removeVariablesFromTableWithNestingGreaterThanThreshold(int v){
            List<string> badList = new();
            foreach(var t in table.Keys){
                if(table[t].nesting > v)
                    badList.Add(t);
            }
            foreach(var e in badList){
                table.Remove(e);
            }
        }

        static void restoreShadowedVariables(){
            foreach(VarInfo vi in shadowed.Peek()){
                string varname = vi.token.lexeme;
                table[varname] = vi;
            }
            shadowed.Pop();
        }

        public static VarInfo lookup(Token id){
            if( table.ContainsKey(id.lexeme) )
                return table[id.lexeme];
            else{
                Console.WriteLine($"No such lexeme {id.lexeme}");
                Environment.Exit(23);
            }
            return null;
        }
        public static VarInfo lookup(string id){
            if( table.ContainsKey(id) )
                return table[id];
            else{
                Console.WriteLine($"No such Id {id}");
                Environment.Exit(1);
            }
            return null;
        }

        public static void declareGlobal(Token token, NodeType type, Label lbl=null){
            if(lbl == null) {
                lbl = new Label(token.lexeme);
            }
                
            string varname = token.lexeme;
            if( table.ContainsKey(varname)){
                Utils.error(token, "Redeclaration of variable");
            }
            table[varname] = new VarInfo(token,
                nesting, //always zero
                type, new GlobalLocation( lbl ));
        }
        public static void declareLocal(Token token, NodeType type){
            string varname = token.lexeme;
            localTypes.Add( new(varname,type) );    //new from strings 1
            if( table.ContainsKey(varname)){
                VarInfo vi = table[varname];
                if( vi.nesting == nesting ){
                    Utils.error(token, "Redeclaration of variable");
                } else if( vi.nesting > nesting ){
                    throw new Exception("ICE");
                } else {
                    shadowed.Peek().Add( table[varname] );
                }
            }
            table[varname] = new VarInfo(token, 
                    nesting, 
                    type, 
                    new LocalLocation(numLocals, token.lexeme)
            );
            numLocals++;
        }
        public static void declareParameter(Token token, NodeType type){ 
            string varname = token.lexeme;
            if( table.ContainsKey(varname)){
                VarInfo vi = table[varname];
                if( vi.nesting == locals.Count ){
                    Utils.error(token, "Redeclaration of variable");
                } else if( vi.nesting >= nesting ){
                    throw new Exception("ICE");
                } else {
                    //vi.nesting must be < nesting
                    shadowed.Peek().Add( table[varname] );
                }
            }
            table[varname] = new VarInfo(token, 
                    nesting, 
                    type, 
                    new ParameterLocation(numParameters, varname)
            );
            locals.Peek().Add(varname);
            numParameters++;
        }

        public static bool currentlyInGlobalScope(){
            if(nesting == 0){
                return true;
                }
            else {
                return false;
            }
        }

        public static void populateBuiltins(){
            SymbolTable.declareGlobal(
                new Token("ID", "putc", -1),
                new FunctionNodeType(NodeType.Int,
                    new List<NodeType>(){NodeType.Int},
                    true                ),
                new Label("putc", "builtin function putc")
            );

            SymbolTable.declareGlobal(
                new Token("ID", "getc", -1),
                new FunctionNodeType(NodeType.Int,
                    new List<NodeType>(){},
                    true
                ),
                new Label("getc", "builtin function getc")
            );

            SymbolTable.declareGlobal(
                new Token("ID", "putv", -1),
                new FunctionNodeType(NodeType.Bool,
                    new List<NodeType>(){NodeType.Int, NodeType.Int},
                    true
                ),
                new Label("putv", "builtin function putv")
            );

            SymbolTable.declareGlobal(
                new Token("ID", "newline", -1),
                new FunctionNodeType(NodeType.Bool,
                    new List<NodeType>(){},
                    true
                ),
                new Label("newline", "builtin function newline")
            );
            
            SymbolTable.declareGlobal(
                new Token("ID","length",-1),
                new FunctionNodeType(
                    returnType: NodeType.Int, 
                    paramTypes: new List<NodeType>(){NodeType.String},
                    builtin: true
                ),
                new Label("length","builtin function length")
            );     

            SymbolTable.declareGlobal(
                new Token("ID","print",-1),
                new FunctionNodeType(
                    returnType: NodeType.Void, 
                    paramTypes: new List<NodeType>(){NodeType.String},
                    builtin: true
                ),
                new Label("print","builtin function print")
            );    

        }
    }

}