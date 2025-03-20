using System.Text.Json;

namespace lab{

public class CompilersAreGreat{
    public static void Main(string[] args){
        if (args.Length < 1) {
            Console.WriteLine("Error: Please provide a file to parse.");
            Environment.Exit(1);
        }

        //initialize our grammar
        Terminals.makeThem();
        Productions.makeThem();

        if (args.Length == 1 && args[0] == "-g"){
            Grammar.check();
            Grammar.computeNullableAndFirst();
            DFA.makeDFA(); //time consuming
            TableWriter.create();
            return;
        }

        try {
            string inp = File.ReadAllText(args[0]);
            var T = new Tokenizer(inp);
            TreeNode root = Parser.parse(T);

            // Output the parse tree
            Console.WriteLine("The parse tree:");
            root.print();

            // Save to JSON file
            root.writeJsonToFile("tree.json");

            // Return success exit code
            Environment.Exit(0);
        }
        catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
} //class

} //namespace