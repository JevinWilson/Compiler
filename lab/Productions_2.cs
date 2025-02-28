
namespace lab{

public class ProductionsAndTerminals2 {
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
            new( "S :: stmts" ),
            new( "stmts :: stmt stmts | lambda" ),
            new( "stmt :: assign | func"),
            new(  "assign :: ID EQ NUM SEMI"),
            new( "func :: ID LPAREN RPAREN" )
        });
    }
}

} //namespace