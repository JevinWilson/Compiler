namespace lab{
    
class Terminals {
    public static void makeThem(){
        Grammar.addTerminals( new Terminal[] {
            new( "COMMENT", @"//[^\n]*"   ),
            new( "EQ", @"="               ),
            new( "LBRACE", @"[{]"         ),
            new( "LPAREN", @"\("          ),
            new( "NUM", @"\d+"            ),
            new( "RBRACE", @"\}"          ),
            new( "RPAREN", @"\)"          ),
            new( "SEMI", @";"             ),
            new( "IF", @"\bif\b"          ),
            new( "ELSE", @"\belse\b"      ),
            new( "WHILE", @"\bwhile\b"    ),
            new( "ID", @"(?!\d)\w+"       )
        });
    } //makeThem
} //Terminals

} //namespace lab
