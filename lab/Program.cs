namespace lab{

public class CompilersAreGreat{
    public static void Main(string[] args){

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //initialize our grammar
        Terminals.makeAllOfTheTerminals();
        Productions.makeThem();

        Grammar.dump();
        return;

        // Grammar.computeNullableAndFirst();
        
        // Grammar.dump();

        // return;
        
        /*string inp = File.ReadAllText(args[0]);
        var tokens = new List<Token>();
        var T = new Tokenizer(inp);
        while(true){
            Token tok = T.next();
            if( tok == null )
                break;
            tokens.Add(tok);
        }
        Console.WriteLine("[");
        for(int i=0;i<tokens.Count;++i){
            Console.Write(tokens[i]);
            if( i != tokens.Count-1 )
                Console.Write(",");
            Console.WriteLine();
        }
        Console.WriteLine("]");*/

    }

} //class

} //namespace

// to run: dotnet run -- "C:\Users\jaw06\Desktop\Compiler\lab\tests\testcases\(fileneme).txt"