using System.Text.RegularExpressions;

namespace lab{

public class Token{
    public string sym; 
    public string lexeme;
    public int line;
    public Token( string sym, string lexeme, int line){
        this.sym = sym;
        this.lexeme = lexeme;
        this.line = line;
    }
    public override string ToString()
    {
        var lex = lexeme.Replace("\\","\\\\").Replace("\"","\\\"").Replace("\n","\\n");

        return $"{{ \"sym\": \"{this.sym}\" , \"line\" : {this.line}, \"lexeme\" : \"{lex}\"  }}";
    }

}

public class Tokenizer{

    bool verbose=false;

    string input; //stuff we are tokenizing
    int line; //current line number
    int index; //where we are at in the input
    string lastSym; //last symbol returned from next()

    Stack<Token> nesting = new();

    public Tokenizer(string inp){
        this.input = inp;
        this.line = 1;
        this.index = 0;
        this.lastSym = null;
    }

    //we can insert an implicit semicolon after these things
    List<string> implicitSemiAfter = new(){"NUM", "RPAREN", "ID", "RB"};

    private bool needImplicitSemi(){
        return lastSym != null && nesting.Count == 0 && implicitSemiAfter.Contains(lastSym);
    }

    private void checkNesting(string sym){
        if (this.nesting.Count == 0){
            Console.WriteLine($"Error at line {this.line}: When looking for match to {sym}: Did not find any");
            Environment.Exit(2);
        }

        var tok = this.nesting.Pop();

        if ((sym == "RPAREN" && tok.sym != "LPAREN") || 
            (sym == "RB" && tok.sym != "LB")){
            Console.WriteLine($"Error at line {this.line}: When looking for match to {sym} found {tok.sym} at line {tok.line}");
            Environment.Exit(2);
        }
    }

    public Token next(){

        //consume leading whitespace
        while( this.index < this.input.Length && Char.IsWhiteSpace( this.input[this.index] ) ){
            //Implemented implicit semicolon insertion
            if( this.input[this.index] == '\n' ){
                this.line++;
                if( needImplicitSemi() ){
                    var t = new Token("SEMI", "", this.line - 1);
                    lastSym = "SEMI";
                    return t;
                }
            }
            this.index++;
        }

        //If we've exhausted the input, return EOF
        if( this.index >= this.input.Length ){
            if(verbose){
                Console.WriteLine("next(): At EOF!");
            }
            
            //Check if we need an implicit semicolon at the end of the file
            if(needImplicitSemi()){
                var t = new Token("SEMI", "", this.line);
                lastSym = "SEMI";
                return t;
            }
            
            //Check for unmatched open parentheses or brackets
            if(nesting.Count > 0){
                var unmatchedToken = nesting.Pop();
                Console.WriteLine($"Error at line {this.line}: At EOF: Unpaired {unmatchedToken.sym} at line {unmatchedToken.line}");
                Environment.Exit(2);
            }
            
            return new Token("$", "", this.line);
        }

        String sym=null;
        String lexeme=null;
        foreach( var t in Grammar.terminals){
            Match M = t.rex.Match( this.input, this.index );
            if(verbose){
                Console.WriteLine("Trying terminal "+t.sym+ "   Matched? "+M.Success);
            }
            if( M.Success ){
                if( lexeme == null || M.Groups[0].Value.Length > lexeme.Length ){
                    sym = t.sym;
                    lexeme = M.Groups[0].Value;
                }
            }
        }

        if( sym == null ){
            //print error message
            Console.WriteLine($"Error at line {this.line}: Could not match anything");
            Environment.Exit(1);
        }

        var tok = new Token( sym , lexeme, line);
        if( verbose ){
            Console.WriteLine("GOT TOKEN: "+tok);
        }

        foreach(var c in lexeme){
            if( c == '\n' )
                this.line++;
        }
        
        this.index += lexeme.Length;

        //Maintenance on nesting stack
        if (sym == "LPAREN" || sym == "LB"){
            nesting.Push(tok);
        } else if (sym == "RPAREN" || sym == "RB"){
            checkNesting(sym);
        }
        
        //Update lastSym for implicit semicolon insertion tracking
        if (sym == "COMMENT"){
            return this.next();
        } else {
            lastSym = sym;
            return tok;
        }
    }//next()

} //class Tokenizer

} //namespace