using System.Net.Mail;

namespace lab{

public static class TableWriter{

    public static void create(){
        // Create a file called ParseTable.cs which has the parse table
        using(var w = new StreamWriter("ParseTable.cs")){
            w.WriteLine("namespace lab{");
            w.WriteLine("public static class ParseTable{");
            w.WriteLine("    public static List<Dictionary<string,ParseAction> > table = new() {");

            for(int i=0;i<DFA.allStates.Count;++i){
                w.WriteLine($"        // DFA STATE {i}");
                
                DFAState q = DFA.allStates[i];
                foreach(LRItem I in q.label.items){
                    w.WriteLine($"        // {I}");
                }
                
                w.WriteLine("        new Dictionary<string,ParseAction>(){");
                
                // Dictionary to help detect conflicts
                var actions = new Dictionary<string, List<ParseAction>>();
                
                // Gather shift actions
                foreach(string sym in q.transitions.Keys){
                    var action = new ParseAction(PAction.SHIFT, q.transitions[sym].unique, null, -1);
                    if(!actions.ContainsKey(sym)){
                        actions[sym] = new List<ParseAction>();
                    }
                    actions[sym].Add(action);
                }
                
                // Gather reduce actions
                foreach(LRItem I in q.label.items){
                    if(I.dposAtEnd()){
                        foreach(string lookahead in I.lookahead){
                            var action = new ParseAction(PAction.REDUCE, I.production.rhs.Length, I.production.lhs, I.production.unique);
                            if(!actions.ContainsKey(lookahead)){
                                actions[lookahead] = new List<ParseAction>();
                            }
                            actions[lookahead].Add(action);
                        }
                    }
                }
                
                // Resolve conflicts and write actions
                foreach(var kvp in actions){
                    string symbol = kvp.Key;
                    List<ParseAction> symbolActions = kvp.Value;
                    
                    if(symbolActions.Count > 1){
                        // We have a conflict
                        int shiftCount = symbolActions.Count(a => a.action == PAction.SHIFT);
                        int reduceCount = symbolActions.Count(a => a.action == PAction.REDUCE);
                        
                        if(shiftCount > 0 && reduceCount > 0){
                            // Shift-reduce conflict - prefer shift but print warning to console
                            Console.WriteLine($"Shift-Reduce conflict in state {i} on symbol {symbol}");
                            // Keep only the shift action for this symbol
                            symbolActions.RemoveAll(a => a.action == PAction.REDUCE);
                        }
                        else if(reduceCount > 1){
                            // Reduce-reduce conflict - print error and exit
                            Console.WriteLine($"Reduce-Reduce conflict in state {i} on symbol {symbol}");
                            Environment.Exit(1);
                        }
                    }
                    
                    // Write the resolved action(s) to the file
                    foreach(var action in symbolActions){
                        w.Write("                {");
                        w.Write($"\"{symbol}\" , ");
                        
                        if(action.action == PAction.SHIFT){
                            w.Write($"new ParseAction(PAction.SHIFT, {action.num}, null, -1)");
                        }
                        else{ // REDUCE
                            w.Write($"new ParseAction(PAction.REDUCE, {action.num}, \"{action.sym}\", {action.productionNumber})");
                        }
                        
                        w.WriteLine("},");
                    }
                }
                
                w.WriteLine("        },");
            }

            w.WriteLine("    }; //close the table initializer");
            w.WriteLine("} //close the ParseTable class");
            w.WriteLine("} //close the namespace lab thing");
        }
        
        // Dump table to console in the expected format
        dumpConsoleOutput();
    }

    private static void dumpConsoleOutput(){
        // Process each state in order
        for(int i=0;i<DFA.allStates.Count;++i){
            Console.WriteLine($"Row {i}:");
            
            DFAState q = DFA.allStates[i];
            
            // Dictionary to collect and detect conflicts
            var actions = new Dictionary<string, List<ParseAction>>();
            
            // Collect shift actions
            foreach(string sym in q.transitions.Keys){
                var action = new ParseAction(PAction.SHIFT, q.transitions[sym].unique, null, -1);
                if(!actions.ContainsKey(sym)){
                    actions[sym] = new List<ParseAction>();
                }
                actions[sym].Add(action);
            }
            
            // Collect reduce actions
            foreach(LRItem I in q.label.items){
                if(I.dposAtEnd()){
                    foreach(string lookahead in I.lookahead){
                        var action = new ParseAction(PAction.REDUCE, I.production.rhs.Length, I.production.lhs, I.production.unique);
                        if(!actions.ContainsKey(lookahead)){
                            actions[lookahead] = new List<ParseAction>();
                        }
                        actions[lookahead].Add(action);
                    }
                }
            }
            
            // Check for conflicts and print them before the actions
            bool hasConflicts = false;
            foreach(var kvp in actions){
                string symbol = kvp.Key;
                List<ParseAction> symbolActions = kvp.Value;
                
                if(symbolActions.Count > 1){
                    int shiftCount = symbolActions.Count(a => a.action == PAction.SHIFT);
                    int reduceCount = symbolActions.Count(a => a.action == PAction.REDUCE);
                    
                    if(shiftCount > 0 && reduceCount > 0){
                        Console.WriteLine($"Shift-Reduce conflict in state {i} on symbol {symbol}");
                        hasConflicts = true;
                        // For console output, we'll keep the shift action (per the requirements)
                        symbolActions.RemoveAll(a => a.action == PAction.REDUCE);
                    }
                    else if(reduceCount > 1){
                        Console.WriteLine($"Reduce-Reduce conflict in state {i} on symbol {symbol}");
                        Environment.Exit(1);
                    }
                }
            }
            
            // Order the symbols for consistent output
            var orderedSymbols = new List<string>();
            
            // First, add all shift actions (these usually come first in output.txt)
            foreach(var kvp in actions.Where(a => a.Value.Any(act => act.action == PAction.SHIFT))
                               .OrderBy(a => a.Key)){ // Sort alphabetically for consistent output
                orderedSymbols.Add(kvp.Key);
            }
            
            // Then, add all reduce actions
            foreach(var kvp in actions.Where(a => a.Value.All(act => act.action == PAction.REDUCE))
                               .OrderBy(a => a.Key)){ // Sort alphabetically for consistent output
                orderedSymbols.Add(kvp.Key);
            }
            
            // Print the actions in the desired order
            foreach(var symbol in orderedSymbols){
                List<ParseAction> symbolActions = actions[symbol];
                
                // Print the resolved action(s) to the console
                foreach(var action in symbolActions){
                    Console.Write("    " + symbol + " ");
                    
                    if(action.action == PAction.SHIFT){
                        Console.WriteLine($"S {action.num} ");
                    }
                    else{ // REDUCE
                        Console.WriteLine($"R {action.num} {action.sym}");
                    }
                }
            }
        }
    }
}

} //namespace lab