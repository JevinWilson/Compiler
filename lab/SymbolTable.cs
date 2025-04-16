
namespace lab{
    
    public static class SymbolTable{

        public static int numLocals=0;
        public static int nesting=0;
        public static Stack< List<VarInfo> > shadowed = new();

        public static Dictionary<string, VarInfo> table = new();

        public static void enterFunctionScope(){ 
            numLocals=0;
            nesting++;
            shadowed.Push(new());
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
        }
        public static void leaveLocalScope(){
            nesting--;
            removeVariablesFromTableWithNestingGreaterThanThreshold(nesting);
            restoreShadowedVariables();
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

        public static void declareGlobal(Token token, NodeType type){
            string varname = token.lexeme;
            if( table.ContainsKey(varname)){
                Utils.error(token, "Redeclaration of variable");
            }
            table[varname] = new VarInfo(token,
                nesting, //always zero
                type, new GlobalLocation( new Label(token.lexeme) ));
        }
        public static void declareLocal(Token token, NodeType type){
            string varname = token.lexeme;
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
                    new ParameterLocation(numLocals, token.lexeme)
            );
        }

        public static bool currentlyInGlobalScope(){
            if(nesting == 0){
                return true;
                }
            else {
                return false;
            }
        }
    }

}