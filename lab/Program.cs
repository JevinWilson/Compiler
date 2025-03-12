using System.Collections.Immutable;

namespace lab {

public class CompilersAreGreat {
    //private const int V = 3;

    public static void Main(string[] args) {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Initialize grammar
        //Terminals.makeThem();
        // Not using our default productions
        //Productions.makeThem();

        //Grammar.check();
        //Grammar.computeNullableAndFirst();

        Grammar.terminals.Clear();
        Grammar.allTerminals.Clear();
        lab.Terminals.makeThem();
        
        // Build DFA
        DFA.makeDFA();

        foreach (var state in DFA.allStates) {
            Console.WriteLine(state);
        }

    }
}

} // namespace