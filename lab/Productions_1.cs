
namespace lab{

public class ProductionsAndTerminals1 {
    public static void makeThem(){
        Grammar.defineTerminals( new Terminal[] {
            new("COMMENT",          @"//[^\n]*"),
            new("EQ",               @"="),
            new("LPAREN",           @"\("),
            new("NUM",              @"\d+" ),
            new("RPAREN",           @"\)"),
            new("SEMI",             @";"),
            new("ID",               @"(?!\d)\w+" )
        });

        Grammar.defineProductions( new PSpec[] {
            new( "S :: stmt S | lambda" ),
            new( "stmt :: ID EQ NUM SEMI"),
            new( "stmt :: ID LPAREN RPAREN")
        });

    }
}

} //namespace