namespace lab{
    
public static class SymbolTable{

    public static Dictionary<string,VarInfo> table = new();
    public static Stack<List<VarInfo>> shadowed = new();
    static Stack< HashSet<String> > locals = new();
    public static int numLocals = 0;
    public static int nesting = 0;


    //static int numParameters=0;

    public static void enterFunctionScope(){ 
        //numParameters = 0;
        numLocals = 0;
        nesting++;
        shadowed.Push( new() );
        locals.Push( new() );
    }
    public static void leaveFunctionScope(){
        numLocals = 0;
        nesting--;
        removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nesting);
        restoreShadowedVariables();
    }

    public static void enterLocalScope(){
        nesting++;
        shadowed.Push( new() );
    }
    public static void leaveLocalScope(){
        nesting--;
        removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nesting);
        restoreShadowedVariables();
    }

    static void removeVariablesFromTableWithNestingLevelGreaterThanThreshold(int v){
        //delete anything from table where 
        //table thing's nestinglevel > v
        List<string> badList = new();
        foreach(var t in table.Keys){
            if(table[t].nesting > v)
                badList.Add(t);
        }
        foreach(var e in badList){
            table.Remove(e);
        }
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

    static void restoreShadowedVariables(){
        foreach(VarInfo vi in shadowed.Peek()){
            string varname = vi.token.lexeme;
            table[varname] = vi;
        }
        shadowed.Pop();
    }

    public static VarInfo lookup(string name){
        if( !table.ContainsKey(name) )
            return table[name];
        else {
            Console.WriteLine($"Not found {name}");
            Environment.Exit(1);
        }
        return null;
    }

    public static void declareGlobal(Token token, NodeType type){ 
        if( table.ContainsKey(token.lexeme) )
            Utils.error(token,$"Redeclaration of global variable {token.lexeme}");
        table[token.lexeme] = new VarInfo(token,0,type,new GlobalLocation(new Label(token.lexeme)));
    }
    public static void declareLocal(Token token, NodeType type){
        string name = token.lexeme;
        if( table.ContainsKey(name)){
            var info = table[name];
            if( info.nesting == locals.Count )
                Utils.error(token,$"Redeclaration of local variable {name}");
            shadowed.Peek().Add(table[name]);
        }
        table[name] = new VarInfo(token,locals.Count,type,new LocalLocation(numLocals,name));
        locals.Peek().Add(name);
        numLocals++;
    }

    public static void declareParameter(Token token, NodeType type){ 
        string name = token.lexeme;
        if( table.ContainsKey(name)){
            var info = table[name];
            if( info.nesting == locals.Count )
                Utils.error(token,$"Redeclaration of parameter {name}");
            else if(info.nesting > locals.Count) {
                throw new Exception("ICE");
            } else {
                shadowed.Peek().Add(table[name]);
            }
        }
        //locals.Count is the nesting level
        table[name] = new VarInfo(
            token,
            nesting,
            type,
            new ParameterLocation(numLocals,name)

        );
    }

    public static bool currentlyInGlobalScope(){ 
        if(nesting == 0)
            return true;
        else
            return false;
    }
}

} //namespace lab