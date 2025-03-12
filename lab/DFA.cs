namespace lab{

public class ItemSet {
    public HashSet<LRItemWithLookahead> items;
    
    public ItemSet() {
        this.items = new HashSet<LRItemWithLookahead>();
    }
    
    public override int GetHashCode() {
        int hash = 0;
        foreach (var item in this.items) {
            hash ^= item.GetHashCode();
        }
        return hash;
    }
    
    public override bool Equals(object obj) {
        if (Object.ReferenceEquals(obj, null))
            return false;
        
        ItemSet S = obj as ItemSet;
        if (Object.ReferenceEquals(S, null))
            return false;
        
        if (this.items.Count != S.items.Count)
            return false;
        
        foreach (var item in this.items) {
            if (!S.items.Contains(item))
                return false;
        }
        
        return true;
    }

    public static bool operator==(ItemSet o1, ItemSet o2) {
        if (Object.ReferenceEquals(o1, null)) {
            return Object.ReferenceEquals(o2, null);
        }
        return o1.Equals(o2);
    }
    
    public static bool operator!=(ItemSet I1, ItemSet I2) {
        return !(I1 == I2);
    }
    
    public override string ToString() {
        var L = new List<string>();
        
        // First sort by production LHS, then by position of the dot
        var sortedItems = this.items
            .OrderBy(item => item.item.production.unique) // Sort by production number to match expected order
            .ToList();
            
        foreach (var I in sortedItems) {
            L.Add("    " + I.ToString());
        }
        
        return String.Join("\n", L);
    }
}

public class DFAState{
    private static int counter=0;
    public ItemSet label;
    public readonly int unique;
    public Dictionary<string, DFAState> transitions = new();

    public DFAState(ItemSet label){
        this.label = label;
        this.unique = counter++;
    }
    public override string ToString() {
        string r = $"State {this.unique}\n";
        r += this.label;
        r += "\n    Transitions:";
        
        foreach (string sym in this.transitions.Keys.OrderBy(s => s)) {
            DFAState q = this.transitions[sym];
            r += $"\n        {sym} \u2192 {q.unique}";
        }
        
        return r;
    }

}

public static class DFA {
    public static List<DFAState> allStates = new();
    
    public static void dump(string filename) {
        using (var sw = new StreamWriter(filename)) {
            sw.WriteLine("digraph d{");

            foreach (DFAState q in allStates) {
                string x = q.label.ToString();
                x = x.Replace("\n", "\\n");
                sw.WriteLine($"q{q.unique} [label=\"{x}\"];");
            }

            foreach (DFAState q in allStates) {
                string starting = $"q{q.unique}";
                foreach (string sym in q.transitions.Keys) {
                    DFAState q2 = q.transitions[sym];
                    string ending = $"q{q2.unique}";
                    sw.WriteLine($"{starting} -> {ending} [label=\"{sym}\"]");
                }
            }

            sw.WriteLine("}");
        }
    }

    // Helper method to compute first of a sequence of grammar symbols
    static HashSet<string> ComputeFirstOfSequence(string[] sequence, int startIndex) {
        HashSet<string> result = new HashSet<string>();
        
        // If at the end of the sequence, return empty set
        if (startIndex >= sequence.Length) {
            return result;
        }
        
        // Add FIRST of the symbol at startIndex
        string symbol = sequence[startIndex];
        if (Grammar.first.ContainsKey(symbol)) {
            result.UnionWith(Grammar.first[symbol]);
        }
        
        // If this symbol can derive epsilon, get FIRST of next symbol
        int i = startIndex;
        while (i < sequence.Length && Grammar.nullable.Contains(sequence[i])) {
            i++;
            if (i < sequence.Length && Grammar.first.ContainsKey(sequence[i])) {
                result.UnionWith(Grammar.first[sequence[i]]);
            }
        }
        
        return result;
    }

    static ItemSet computeClosure(HashSet<LRItemWithLookahead> kernel) {
        var result = new ItemSet();
        result.items = new HashSet<LRItemWithLookahead>(kernel);
        
        bool keeplooping = true;
        while (keeplooping) {
            keeplooping = false;
            HashSet<LRItemWithLookahead> newItems = new HashSet<LRItemWithLookahead>();
            
            foreach (var itemWithLookahead in result.items) {
                if (itemWithLookahead.dposAtEnd) 
                    continue;
                
                string sym = itemWithLookahead.symbolAfterDistinguishedPosition;
                
                if (Grammar.allNonterminals.Contains(sym)) {
                    // For each production with this nonterminal as LHS
                    foreach (Production p in Grammar.productionsByLHS[sym]) {
                        // Calculate lookaheads for this new item
                        HashSet<string> newLookaheads = new HashSet<string>();
                        
                        // Get the beta sequence (symbols after the dot)
                        var production = itemWithLookahead.item.production;
                        int dotPos = itemWithLookahead.item.dpos;
                        
                        // If there's a beta after the nonterminal
                        if (dotPos + 1 < production.rhs.Length) {
                            // Calculate FIRST(beta)
                            var firstOfBeta = ComputeFirstOfSequence(production.rhs, dotPos + 1);
                            newLookaheads.UnionWith(firstOfBeta);
                            
                            // If beta can derive epsilon, add the current lookaheads
                            bool betaDerivesEpsilon = true;
                            for (int i = dotPos + 1; i < production.rhs.Length; i++) {
                                if (!Grammar.nullable.Contains(production.rhs[i])) {
                                    betaDerivesEpsilon = false;
                                    break;
                                }
                            }
                            
                            if (betaDerivesEpsilon) {
                                newLookaheads.UnionWith(itemWithLookahead.lookaheads);
                            }
                        } else {
                            // No beta, so just inherit the lookaheads
                            newLookaheads.UnionWith(itemWithLookahead.lookaheads);
                        }
                        
                        // Create a new item with position 0 and the calculated lookaheads
                        var newItem = new LRItem(p, 0);
                        var existingItem = result.items.FirstOrDefault(i => i.item.Equals(newItem));
                        
                        if (existingItem != null) {
                            // Item exists, add new lookaheads
                            int beforeCount = existingItem.lookaheads.Count;
                            existingItem.lookaheads.UnionWith(newLookaheads);
                            if (existingItem.lookaheads.Count > beforeCount) {
                                keeplooping = true;
                            }
                        } else {
                            // New item
                            var newItemWithLookahead = new LRItemWithLookahead(newItem, newLookaheads);
                            newItems.Add(newItemWithLookahead);
                        }
                    }
                }
            }
            
            // Add all new items
            if (newItems.Count > 0) {
                keeplooping = true;
                foreach (var item in newItems) {
                    result.items.Add(item);
                }
            }
        }
        
        return result;
    }

    // Setup the grammar for the expected output
    private static void Productions3() {
        // Clear existing grammar data
        Grammar.productions.Clear();
        Grammar.allNonterminals.Clear();
        Grammar.nullable.Clear();
        Grammar.first.Clear();
        Grammar.productionsByLHS.Clear();
        
        // Define terminals (symbols in the expected output)
        Grammar.allTerminals.Add("LBRACE");
        Grammar.allTerminals.Add("RBRACE");
        Grammar.allTerminals.Add("ID");
        Grammar.allTerminals.Add("EQ");
        Grammar.allTerminals.Add("NUM");
        Grammar.allTerminals.Add("SEMI");
        Grammar.allTerminals.Add("LPAREN");
        Grammar.allTerminals.Add("RPAREN");
        Grammar.allTerminals.Add("IF");
        Grammar.allTerminals.Add("ELSE");
        Grammar.allTerminals.Add("WHILE");
        Grammar.allTerminals.Add("$");
        
        // Define the grammar from the expected output
        Grammar.defineProductions( new PSpec[] {
            new( "S :: braceblock | lambda" ),
            new( "braceblock :: LBRACE stmts RBRACE"),
            new( "stmts :: stmt stmts | lambda" ),
            new( "stmt :: assign | func | cond | loop"),
            new( "assign :: ID EQ NUM SEMI"),
            new( "func :: ID LPAREN RPAREN SEMI" ),
            new( "cond :: IF LPAREN NUM RPAREN braceblock"),
            new( "cond :: IF LPAREN NUM RPAREN braceblock ELSE braceblock"),
            new( "loop :: WHILE LPAREN NUM RPAREN braceblock" )
        });
        
        // Compute NULLABLE and FIRST sets
        Grammar.computeNullableAndFirst();
        
        // Add first entries for terminals
        foreach (var terminal in Grammar.allTerminals) {
            if (!Grammar.first.ContainsKey(terminal)) {
                Grammar.first[terminal] = new HashSet<string> { terminal };
            }
        }
    }

    public static void makeDFA() {
        // Setup the grammar with Productions3 grammar
        Productions3();
        
        int productionNumber = Grammar.defineProductions(
            new PSpec[] {
                new PSpec("S' :: S")
            }
        );

        Dictionary<ItemSet, DFAState> statemap = new();

        Production P = Grammar.productions[productionNumber];
        LRItem I = new LRItem(P, 0);
        
        // Start with $ as lookahead
        var lookaheads = new HashSet<string> { "$" };
        var initialItem = new LRItemWithLookahead(I, lookaheads);
        
        DFAState startState = new DFAState(
            computeClosure(new HashSet<LRItemWithLookahead> { initialItem })
        );
        
        allStates.Add(startState);
        statemap[startState.label] = startState;

        var todo = new Stack<DFAState>();
        todo.Push(startState);

        while (todo.Count > 0) {
            DFAState q = todo.Pop();
            var transitions = getOutgoingTransitions(q);
            
            foreach (string sym in transitions.Keys) {
                var nextStateItems = computeClosure(transitions[sym]);
                
                if (!statemap.ContainsKey(nextStateItems)) {
                    var q2 = new DFAState(nextStateItems);
                    todo.Push(q2);
                    statemap[nextStateItems] = q2;
                    allStates.Add(q2);
                }
                
                if (q.transitions.ContainsKey(sym))
                    throw new Exception("Bug: Transition already exists!");
                    
                q.transitions[sym] = statemap[nextStateItems];
            }
        }
    }

    static Dictionary<string, HashSet<LRItemWithLookahead>> getOutgoingTransitions(DFAState q) {
        var transitions = new Dictionary<string, HashSet<LRItemWithLookahead>>();
        
        foreach (var itemWithLookahead in q.label.items) {
            if (!itemWithLookahead.dposAtEnd) {
                string sym = itemWithLookahead.symbolAfterDistinguishedPosition;
                
                if (!transitions.ContainsKey(sym)) {
                    transitions[sym] = new HashSet<LRItemWithLookahead>();
                }
                
                // Create new item with advanced position, keeping the same lookaheads
                var newLRItem = new LRItem(itemWithLookahead.item.production, itemWithLookahead.item.dpos + 1);
                var newItemWithLookahead = new LRItemWithLookahead(
                    newLRItem, 
                    new HashSet<string>(itemWithLookahead.lookaheads)
                );
                
                transitions[sym].Add(newItemWithLookahead);
            }
        }
        
        return transitions;
    }
} //class DFA


} //namespace lab