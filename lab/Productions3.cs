
namespace lab{

public class Productions {
    public static void makeThem(){
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
