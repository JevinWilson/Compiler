namespace lab{

public class CompilersAreGreat{
    public static void Main(string[] args){

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //initialize our grammar
        /*Terminals.makeAllOfTheTerminals();
        Productions.makeThem();

        Grammar.computeNullableAndFirst();
        Grammar.dump();*/

        Grammar.terminals.Clear();
        Grammar.allTerminals.Clear();
        Grammar.productions.Clear();
        Grammar.allNonterminals.Clear();
        Grammar.nullable.Clear();
        Grammar.first.Clear();

        // used chat to get from here on to get it to create the .dot file and name it
        // Prompt: how do I create a .dot file name associated to the test case I ran it on?
        
        string testFile = "0";
        if(args.Length > 0){
            testFile = args[0];
        }

        Console.WriteLine($"using Productions_{testFile}");

        // Select the appropriate grammar based on the command-line argument
        switch (testFile) {
            case "0":
                ProductionsAndTerminals0.makeThem();
                break;
            case "1":
                ProductionsAndTerminals1.makeThem();
                break;
            case "2":
                ProductionsAndTerminals2.makeThem();
                break;
            case "3":
                ProductionsAndTerminals3.makeThem();
                break;
            default:
                Console.WriteLine($"Unknown test file: {testFile}");
                return;
        }
        
        Grammar.computeNullableAndFirst();
        
        LRAutomaton.BuildLR0Automaton("S");
        
        if (DFANode.allNodes.Count > 0) {
            string dotFile = $"dfa_{testFile}.dot";
            DFANode.allNodes[0].toDot(dotFile);
            Console.WriteLine($"Generated DOT file: {dotFile}");
            
            // Optionally dump the nodes for debugging
            DFANode.dump();
        }

        return;
    }

} //class

} //namespace

// to run: dotnet run -- "C:\Users\jaw06\Desktop\Compiler\lab\tests\testcases\(fileneme).txt"