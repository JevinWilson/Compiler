namespace lab{
    
    public static class SymbolTable{

        static int numLocals=0;
        static int nestingLevel=0;
        static Stack< List<VarInfo> > shadowed = new();

        static Dictionary<string, VarInfo> table = new();

        public static void enterFunctionScope(){ 
            numLocals=0;
            nestingLevel++;
            shadowed.Push(new());
        }
        public static void leaveFunctionScope(){
            nestingLevel--;
            numLocals=0;        //bogus
            removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nestingLevel);
            restoreShadowedVariables();
        }

        public static void enterLocalScope(){
            nestingLevel++;    
            shadowed.Push(new());
        }
        public static void leaveLocalScope(){
            nestingLevel--;
            //...fixme...
            // fixed
            removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nestingLevel);
            restoreShadowedVariables();
        }

        static void removeVariablesFromTableWithNestingLevelGreaterThanThreshold(int v){
            //delete anything from table where 
            //table thing's nestinglevel > v
            // fixed
            List<string> toRemove = new();
            foreach(var t in table.Keys) {
                if(table[t].nestingLevel > v) {
                    toRemove.Add(t);
                }
                foreach(var e in toRemove) {
                    table.Remove(e);
                }
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
            //look in table
            //find thing
            //if not found, signal error
            //else return data  
            if(table.ContainsKey(id.lexeme)) {
                return table[id.lexeme];
            } else {
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
                nestingLevel, //always zero
                type, new GlobalLocation());
        }
        public static void declareLocal(Token token, NodeType type){
            string varname = token.lexeme;
            if( table.ContainsKey(varname)){
                VarInfo vi = table[varname];
                if( vi.nestingLevel == nestingLevel ){
                    Utils.error(token, "Redeclaration of variable");
                } else if( vi.nestingLevel > nestingLevel ){
                    throw new Exception("ICE");
                } else {
                    //vi.nestingLevel must be < nestingLevel
                    shadowed.Peek().Add( table[varname] );
                }
            }
            table[varname] = new VarInfo(token, 
                    nestingLevel, 
                    type, 
                    new LocalLocation(numLocals)
            );
            numLocals++;
        }
        public static void declareParameter(Token token, NodeType type){ 
            string variableName = token.lexeme;
            if( table.ContainsKey(variableName)){
                VarInfo vi = table[variableName];
                if( vi.nestingLevel == nestingLevel ){
                    Utils.error(token, "Redeclaration of variable");
                } else if( vi.nestingLevel > nestingLevel ){
                    throw new Exception("ICE");
                } else {
                    //vi.nestingLevel must be < nestingLevel
                    shadowed.Peek().Add( table[variableName] );
                }
            }
            table[variableName] = new VarInfo(token, 
                    nestingLevel, 
                    type, 
                    new ParameterLocation(numLocals)
            ); 
        }

        public static bool currentlyInGlobalScope(){
            if(nestingLevel == 0) {
                return true;
            } else {
                return false;
            }
        }
    }

}