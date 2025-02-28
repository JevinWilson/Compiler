namespace lab {
    public class LRAutomaton {
        // Build the LR(0) automaton for a grammar
        public static void BuildLR0Automaton(string startSymbol) {
            // Clear any existing DFA nodes
            DFANode.allNodes.Clear();
            
            // Add the augmented start production (S' -> S)
            string augmentedStart = startSymbol + "'";
            var augmentedProduction = new Production(
                new PSpec($"{augmentedStart} :: {startSymbol}"), 
                augmentedStart, 
                new string[] { startSymbol }, 
                Grammar.productions.Count
            );
            
            Grammar.productions.Add(augmentedProduction);
            Grammar.allNonterminals.Add(augmentedStart);
            
            // Create initial LR item for start production with dot at the beginning
            var initialItem = new LRItem(Grammar.productions.Count - 1, 0);
            
            // Compute the closure of the initial item
            var initialItemSet = Closure(new HashSet<LRItem> { initialItem });
            
            // Create the initial DFA node
            var initialNode = new DFANode(initialItemSet);
            
            // Process all created nodes until no more are created
            int processedCount = 0;
            while (processedCount < DFANode.allNodes.Count) {
                var currentNode = DFANode.allNodes[processedCount++];
                
                // Find all symbols that appear after the dot in this node's items
                var symbols = new HashSet<string>();
                foreach (var item in currentNode.items) {
                    if (item.dpos < item.production.rhs.Length) {
                        symbols.Add(item.production.rhs[item.dpos]);
                    }
                }
                
                // For each symbol, compute the GOTO and create a new node if needed
                foreach (var symbol in symbols) {
                    var gotoSet = ComputeGoto(currentNode.items, symbol);
                    
                    // If not empty, create a new node or reuse existing one
                    if (gotoSet.Count > 0) {
                        // Check if this item set already exists in a node
                        DFANode targetNode = null;
                        foreach (var node in DFANode.allNodes) {
                            if (node.items.SetEquals(gotoSet)) {
                                targetNode = node;
                                break;
                            }
                        }
                        
                        // If not found, create a new node
                        if (targetNode == null) {
                            targetNode = new DFANode(gotoSet);
                        }
                        
                        // Add the transition from current node to target node
                        currentNode.transitions[symbol] = targetNode;
                    }
                }
            }
        }
        
        // Compute the closure of a set of LR items
        private static HashSet<LRItem> Closure(HashSet<LRItem> items) {
            var result = new HashSet<LRItem>(items);
            bool changed = true;
            
            while (changed) {
                changed = false;
                var newItems = new HashSet<LRItem>();
                
                foreach (var item in result) {
                    // If dot is before a nonterminal, add all productions for that nonterminal
                    if (item.dpos < item.production.rhs.Length) {
                        string symbol = item.production.rhs[item.dpos];
                        
                        if (Grammar.isNonterminal(symbol)) {
                            // Find all productions with this nonterminal on the left
                            for (int i = 0; i < Grammar.productions.Count; i++) {
                                var production = Grammar.productions[i];
                                if (production.lhs == symbol) {
                                    var newItem = new LRItem(i, 0);
                                    if (!result.Contains(newItem)) {
                                        newItems.Add(newItem);
                                        changed = true;
                                    }
                                }
                            }
                        }
                    }
                }
                
                // Add all new items to the result
                result.UnionWith(newItems);
            }
            
            return result;
        }
        
        // Compute the GOTO for a set of LR items and a grammar symbol
        private static HashSet<LRItem> ComputeGoto(HashSet<LRItem> items, string symbol) {
            var result = new HashSet<LRItem>();
            
            // Find all items where the dot is before the given symbol
            foreach (var item in items) {
                if (item.dpos < item.production.rhs.Length && 
                    item.production.rhs[item.dpos] == symbol) {
                    // Create a new item with the dot moved one position to the right
                    result.Add(new LRItem(item.productionIndex, item.dpos + 1));
                }
            }
            
            // Take the closure of the resulting set
            return Closure(result);
        }
    }
}