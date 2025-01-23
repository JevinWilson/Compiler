
namespace lab{

public static class Terminals{
    public static void makeAllOfTheTerminals(){
        Grammar.addTerminals( new Terminal[] {
            new("EQ",               @"="),
            new("NUM",              @"\d+" ),
            new("STRINGCONSTANT",   @"(?x)  "" (\\[nr""]  | [^\\] )*  ""  "),
            new("LPAREN",           @"\("),
            new("RPAREN",           @"\)"),
            new("LBRACE",           @"\{"),
            new("RBRACE",           @"\}"),
            new("SEMI",             @";"),
            new("IF",               @"\bif\b"),
            new("ELSE",             @"\belse\b"),
            new("RELOP",            @">=|<=|>|<|==|!="),
            new("AND",              @"\band\b"),
            new("ID",               @"(?!\d)\w+" )
        });
    }
}

}