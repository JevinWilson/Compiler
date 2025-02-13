namespace lab{

public class CompilersAreGreat{
    public static void Main(string[] args){

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //initialize our grammar
        Terminals.makeAllOfTheTerminals();
        Productions.makeThem();

        Grammar.computeNullableAndFirst();
        Grammar.dump();
        return;
    }

} //class

} //namespace

// to run: dotnet run -- "C:\Users\jaw06\Desktop\Compiler\lab\tests\testcases\(fileneme).txt"