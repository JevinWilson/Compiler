using System.Linq;

namespace lab{

public static class Grammar{
    public static List<Terminal> terminals = new();
    public static HashSet<string> allTerminals = [];
    public static List<Production> productions = new();
    public static HashSet<string> allNonterminals = new();
    public static HashSet<string> nullable = new();
    public static Dictionary<string,HashSet<string>> first = new();

    public static void addTerminals( Terminal[] terminals){
        foreach(var t in terminals){
            if( isTerminal( t.sym ) )
                throw new Exception("YOU ARE MISTAKE");
            Grammar.terminals.Add(t);
            allTerminals.Add(t.sym);
        }
    }

    public static bool isTerminal(string sym){
        return allTerminals.Contains(sym);
    }
    public static bool isNonterminal(string sym){
        return allNonterminals.Contains(sym);
    }

    public static void defineProductions(PSpec[] specs) {
        foreach(var pspec in specs) {
            // First clean up the spec by replacing newlines with spaces
            var cleanSpec = pspec.spec
                .Replace("\n", " ")
                .Trim();

            // Find the :: in the original string
            int sepPos = cleanSpec.IndexOf("::");
            if (sepPos == -1)
                throw new Exception("Invalid production format (missing ::): " + cleanSpec);

            string lhs = cleanSpec.Substring(0, sepPos).Trim();
            string rhsPart = cleanSpec.Substring(sepPos + 2).Trim();

            allNonterminals.Add(lhs);

            // Split RHS into alternatives
            var alternatives = rhsPart
                .Split('|')
                .Select(alt => alt.Trim())
                .Where(alt => !string.IsNullOrWhiteSpace(alt));

            foreach(var alt in alternatives) {
                // Handle lambda productions
                if (alt == "lambda" || alt == "λ" || alt == "?") {
                    productions.Add(new Production(pspec, lhs, Array.Empty<string>()));
                } else {
                    // Split the alternative into tokens
                    var rhs = alt.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    productions.Add(new Production(pspec, lhs, rhs));
                }
            }
        }
    }

    public static void check(){
        //check for problems. panic if so.
        foreach( Production p in productions){
            foreach( string sym in p.rhs){
                if(!isTerminal(sym) && !isNonterminal(sym)){
                    throw new Exception("Undefined symbol: "+sym);
                }
            }
        }
    }

    public static void dump(){
        //dump grammar stuff to the screen (debugging)
        foreach( var p in productions ){
            Console.WriteLine(p);
        }
        Console.Write("NULLABLE: FINISH ME");
        Console.WriteLine();
        foreach(var sym in first.Keys){
            Console.Write($"first[{sym}] = ");
            Console.WriteLine("FINISH ME");
        }
        
    }

    public static void computeNullableAndFirst(){
        var flag = true;
        while(flag){
            flag=false;
            //TODO: Finish computing nullable
        }
        
        foreach( var sym in Grammar.allTerminals){
            first[sym] = new();
            first[sym].Add(sym);
        }
        foreach(var sym in Grammar.allNonterminals){
            first[sym] = new();
        }

        flag=true;
        while(flag){
            flag=false;
            //TODO: Finish computing first
        }
    }
} //end class Grammar

} //end namespace