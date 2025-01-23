namespace lab {

public static class Grammar {
    public static List<Terminal> terminals = new();
    public static HashSet<string> allTerminals = [];
    
    public static void addTerminals( Terminal[] terminals) {
        foreach(var t in terminals){
            if( isTerminal( t.sym ))
                throw new Exception("YOU ARE MISTAKEN");
            Grammar.terminals.Add(t);
            allTerminals.Add(t.sym);
        }
        
    }

    public static bool isTerminal(string sym) {
        return allTerminals.Contains(sym);
    }
    
} // end of Grammar

} // end of namespace