
namespace lab{

public class ProductionsAndTerminals0 {
    public static void makeThem(){
        Grammar.defineTerminals( new Terminal[] {
            new("ID",               @"(?!\d)\w+" )
        });

        Grammar.defineProductions( new PSpec[] {
            new("S :: ID")
        });
    }
}

} //namespace