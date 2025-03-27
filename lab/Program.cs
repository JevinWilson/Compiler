namespace lab {
public class CompilersAreGreat {
    private const int V = 3;
    public static void Main(string[] args) {
        // Initialize grammar
        Terminals.makeThem();
        Productions.makeThem();
        ProductionsExpr.makeThem();

        if (args.Length == 1 && args[0] == "-g") {
            Grammar.check();
            Grammar.computeNullableAndFirst();
            DFA.makeDFA();
            TableWriter.create();
            return;
        }

        // TEST SETUP - Add this block right before parsing
        SymbolTable.Clear();
        SymbolTable.declareGlobal(new Token("ID", "x", 0), NodeType.Int);
        // You can add other test variables here if needed
        // SymbolTable.declareGlobal(new Token("ID", "y", 0), NodeType.Float);
        // SymbolTable.declareGlobal(new Token("ID", "z", 0), NodeType.String);

        string inp = File.ReadAllText(args[0]);
        var tokens = new List<Token>();
        var T = new Tokenizer(inp);
        TreeNode root = Parser.parse(T);

        root.collectClassNames();
        root.setNodeTypes();

        // Debug output
        var opts = new System.Text.Json.JsonSerializerOptions {
            IncludeFields = true,
            WriteIndented = true,
            MaxDepth = 1000000
        };
        string J = System.Text.Json.JsonSerializer.Serialize(root, opts);
        using (var w = new StreamWriter("tree.json")) {
            w.WriteLine(J);
        }
    }
}
}