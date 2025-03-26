using System.Collections.Immutable;
using System.Xml;

namespace lab{

public class CompilersAreGreat{
    private const int V = 3;

    public static void Main(string[] args){

        //initialize our grammar
        Terminals.makeThem();
        Grammar.addTerminals(new Terminal[]{new("WHITESPACE",@"\s+")});
        Productions.makeThem();
        //ProductionsExpr.makeThem();

        if( args.Length == 1 && args[0] == "-g" ){
            Grammar.check();
            Grammar.computeNullableAndFirst();
            DFA.makeDFA(); //time consuming
            TableWriter.create();
            DFA.dump("output.txt");
            return;
        }

        string inp = File.ReadAllText(args[0]);
        var tokens = new List<Token>();
        var T = new Tokenizer(inp);
        TreeNode root = Parser.parse(T);
        root.collectClassNames();
        root.collectFuncNames();

    }
} //class

} //namespace
