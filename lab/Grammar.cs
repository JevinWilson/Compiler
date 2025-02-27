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
                    productions.Add(new Production(pspec, lhs, Array.Empty<string>(), productions.Count));
                } else {
                    // Split the alternative into tokens
                    var rhs = alt.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    productions.Add(new Production(pspec, lhs, rhs, productions.Count));
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

        Console.WriteLine("NULLABLE: ");
        Console.WriteLine(string.Join(", ", nullable));

        foreach(var sym in first.Keys) {
            Console.WriteLine($"first[{sym}] = ");
            Console.WriteLine("{" + string.Join(", ", first[sym]) + "}");
        }
    }

    public static void computeNullableAndFirst(){
        // initialize nullable set
        nullable = new HashSet<string>();

        var flag = true;
        while(flag) {
            flag = false;

            //for each production
            foreach(var p in productions) {
                // if lhs is already nullable, skip
                if (nullable.Contains(p.lhs)) continue;

                // check if rhs is empty (lambda production)
                if (p.rhs.Length == 0) {
                    nullable.Add(p.lhs);
                    flag = true;
                    continue;
                }

                // check if all symbols in rhs are nullable
                bool allNullable = true;
                foreach(var sym in p.rhs) {
                    if (!nullable.Contains(sym)) {
                        allNullable = false;
                        break;
                    }
                }

                if (allNullable) {
                    nullable.Add(p.lhs);
                    flag = true;
                }
            }
        }

        // initialize first sets for terminals
        foreach(var sym in allTerminals) {
            first[sym] = new HashSet<string> { sym };
        }

        // initialize first sets for nonterminals
        foreach(var sym in allNonterminals) {
            first[sym] = new HashSet<string>();
        }

        flag = true;
        while(flag) {
            flag = false;

            // for each production
            foreach(var p in productions) {
                // for each symbol in rhs
                for(int i = 0; i < p.rhs.Length; i++) {
                    string sym = p.rhs[i];

                    // add first(sym) to first(lhs)
                    int oldCount = first[p.lhs].Count;
                    first[p.lhs].UnionWith(first[sym]);

                    if (first[p.lhs].Count > oldCount) {
                        flag = true;
                    }

                    // if sym is not nullable, break
                    if (!nullable.Contains(sym)) {
                        break;
                    }

                    // if we're at the end of rhs and all symbols are nullable
                    if (i == p.rhs.Length -1 && nullable.Contains(sym)) {
                        if (!nullable.Contains(p.lhs)) {
                            nullable.Add(p.lhs);
                            flag = true;
                        }
                    }
                }
            }
        }
    }

    public static void defineTerminals(Terminal[] terminals) {
        addTerminals(terminals);
    }

} //end class Grammar

} //end namespace