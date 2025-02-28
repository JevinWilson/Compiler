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

        ProductionsAndTerminals.makeThem();

        Grammar.computeNullableAndFirst();
        
        LRAutomaton.BuildLR0Automaton("S");

        if (DFANode.allNodes.Count > 0) {
            DFANode.allNodes[0].toDot("dfa.dot");
        }

        return;
    }

} //class

} //namespace

// to run: dotnet run -- "C:\Users\jaw06\Desktop\Compiler\lab\tests\testcases\(fileneme).txt"