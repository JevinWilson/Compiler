namespace lab {
    public class LRAutomaton {
        // build the LR(0) automaton for a grammar
        public static void BuildLR0Automaton(string startSymbol) {
            // clear any existing DFA nodes
            DFANode.allNodes.Clear();
            
            // start production (S' -> S)
            string augmentedStart = startSymbol + "'";
            var augmentedProduction = new Production(
                new PSpec($"{augmentedStart} :: {startSymbol}"), 
                augmentedStart, 
                new string[] { startSymbol }, 
                Grammar.productions.Count
            );
            
            Grammar.productions.Add(augmentedProduction);
            Grammar.allNonterminals.Add(augmentedStart);
            
            var initialItem = new LRItem(Grammar.productions.Count - 1, 0);
            
            // compute the closure of the initial item
            var initialItemSet = Closure(new HashSet<LRItem> { initialItem });
            
            // create the initial DFA node
            var initialNode = new DFANode(initialItemSet);
            
            int processedCount = 0;
            while (processedCount < DFANode.allNodes.Count) {
                var currentNode = DFANode.allNodes[processedCount++];
                
                // find all symbols that appear after the dot in this node's items
                var symbols = new HashSet<string>();
                foreach (var item in currentNode.items) {
                    if (item.dpos < item.production.rhs.Length) {
                        symbols.Add(item.production.rhs[item.dpos]);
                    }
                }
                
                // for each symbol, compute the GOTO and create a new node if needed
                foreach (var symbol in symbols) {
                    var gotoSet = ComputeGoto(currentNode.items, symbol);
                    
                    if (gotoSet.Count > 0) {
                        // check if item set already exists in node
                        DFANode targetNode = null;
                        foreach (var node in DFANode.allNodes) {
                            if (node.items.SetEquals(gotoSet)) {
                                targetNode = node;
                                break;
                            }
                        }
                        
                        // ff not  create a new node
                        if (targetNode == null) {
                            targetNode = new DFANode(gotoSet);
                        }
                        
                        // add the transition from current node to target 
                        currentNode.transitions[symbol] = targetNode;
                    }
                }
            }
        }
        
        // compute the closure of set of LR items
        private static HashSet<LRItem> Closure(HashSet<LRItem> items) {
            var result = new HashSet<LRItem>(items);
            bool changed = true;
            
            while (changed) {
                changed = false;
                var newItems = new HashSet<LRItem>();
                
                foreach (var item in result) {
                    // if dot is before a nonterminal, add all productions for that nonterminal
                    if (item.dpos < item.production.rhs.Length) {
                        string symbol = item.production.rhs[item.dpos];
                        
                        if (Grammar.isNonterminal(symbol)) {
                            // find all productions with this nonterminal on the left
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

    } //class LRAutomaton

} //namespace