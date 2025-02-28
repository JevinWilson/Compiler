
namespace lab{

public class ProductionsAndTerminals3 {
    public static void makeThem(){
        Grammar.defineTerminals( new Terminal[] {
            new("COMMENT",          @"//[^\n]*"),
            new("EQ",               @"="),
            new("LBRACE",           @"[{]"),
            new("LPAREN",           @"\("),
            new("NUM",              @"\d+" ),
            new("RBRACE",           @"\}"),
            new("RPAREN",           @"\)"),
            new("SEMI",             @";"),
            new("IF",               @"\bif\b"),
            new("ELSE",             @"\belse\b"),
            new("WHILE",            @"\bwhile\b"),
            new("ID",               @"(?!\d)\w+" )
        });

        Grammar.defineProductions( new PSpec[] {
            new( "S :: braceblock | lambda" ),
            new( "braceblock :: LBRACE stmts RBRACE"),
            new( "stmts :: stmt stmts | lambda" ),
            new( "stmt :: assign | func | cond | loop"),
            new( "assign :: ID EQ NUM SEMI"),
            new( "func :: ID LPAREN RPAREN SEMI" ),
            new( "cond :: IF LPAREN NUM RPAREN braceblock"),
            new( "cond :: IF LPAREN NUM RPAREN braceblock ELSE braceblock"),
            new( "loop :: WHILE LPAREN NUM RPAREN braceblock" )
        });

    }
}

} //namespace