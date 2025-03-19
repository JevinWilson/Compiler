
namespace lab{

public class Terminals{
    public static void makeThem(){
        Grammar.addTerminals( new Terminal[] {
            new("COMMENT",          @"//[^\n]*"),
            new("EQ",               @"="),
            new("LB",               @"\["),
            new("LP",               @"\("),
            new("MUL",              @"\*"),
            new("NUM",              @"\d+" ),
            new("PLUS",             @"\+"),
            new("RB",               @"\]"),
            new("RP",               @"\)"),
            new("IF",               @"\bif\b"),
            new("ELSE",             @"\belse\b"),
            new("TYPE",             @"\bint\b"),
            new("SEMI",             @";"),
            new("VOID",             @"\bvoid\b"),
            new("ID",               @"(?!\d)\w+" )
        });
    }

}

}