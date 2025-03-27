namespace lab {

public static class SymbolTable {
    static int numLocals = 0;
    static int nestingLevel = 0;
    static Stack<List<VarInfo>> shadowed = new();
    static Dictionary<string, VarInfo> table = new();

    public static void enterFunctionScope() { 
        numLocals = 0;
        nestingLevel++;
        shadowed.Push(new());
    }

    public static void leaveFunctionScope() {
        nestingLevel--;
        numLocals = 0; // Reset locals for the next function
        removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nestingLevel);
        restoreShadowedVariables();
    }

    public static void enterLocalScope() {
        nestingLevel++;    
        shadowed.Push(new());
    }

    public static void leaveLocalScope() {
        nestingLevel--;
        removeVariablesFromTableWithNestingLevelGreaterThanThreshold(nestingLevel);
        restoreShadowedVariables();
    }

    static void removeVariablesFromTableWithNestingLevelGreaterThanThreshold(int v) {
        // Remove all variables with nesting level greater than v
        var toRemove = table.Where(kvp => kvp.Value.nestingLevel > v).Select(kvp => kvp.Key).ToList();
        foreach (var key in toRemove) {
            table.Remove(key);
        }
    }

    static void restoreShadowedVariables() {
        foreach (VarInfo vi in shadowed.Peek()) {
            string varname = vi.token.lexeme;
            table[varname] = vi;
        }
        shadowed.Pop();
    }

    public static VarInfo lookup(Token id) {
        string varname = id.lexeme;
        if (table.TryGetValue(varname, out VarInfo vi)) {
            return vi;
        }
        Utils.error(id, $"Undefined variable: {varname}");
        return null; // Unreachable due to Utils.error exiting, but required for return type
    }

    public static void declareGlobal(Token token, NodeType type) {
        string varname = token.lexeme;
        if (table.ContainsKey(varname)) {
            Utils.error(token, "Redeclaration of variable");
        }
        table[varname] = new VarInfo(token, nestingLevel, type, new GlobalLocation());
    }

    public static void declareLocal(Token token, NodeType type) {
        string varname = token.lexeme;
        if (table.ContainsKey(varname)) {
            VarInfo vi = table[varname];
            if (vi.nestingLevel == nestingLevel) {
                Utils.error(token, "Redeclaration of variable");
            } else if (vi.nestingLevel > nestingLevel) {
                throw new Exception("ICE"); // Internal Compiler Error: shouldn’t happen
            } else {
                // Shadow the outer variable
                shadowed.Peek().Add(table[varname]);
            }
        }
        table[varname] = new VarInfo(token, nestingLevel, type, new LocalLocation(numLocals));
        numLocals++;
    }

    public static void declareParameter(Token token, NodeType type) {
        // Treat parameters as locals within the function scope
        string varname = token.lexeme;
        if (table.ContainsKey(varname) && table[varname].nestingLevel == nestingLevel) {
            Utils.error(token, "Redeclaration of parameter");
        }
        // Parameters don’t shadow outer variables in the same way locals do; they’re part of the function’s scope
        table[varname] = new VarInfo(token, nestingLevel, type, new LocalLocation(numLocals));
        numLocals++;
    }

    public static bool currentlyInGlobalScope() {
        return nestingLevel == 0;
    }

    // Optional: Add a reset method for testing multiple files
    public static void Clear() {
        numLocals = 0;
        nestingLevel = 0;
        shadowed.Clear();
        table.Clear();
    }
}
}