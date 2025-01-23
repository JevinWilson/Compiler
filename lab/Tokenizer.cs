using System.Text.RegularExpressions;

namespace lab {
public class Token {
    public string sym; //maybe use later
    public string lexeme; 
    public int line; 
    public Token(string sym, string lexeme, int line) {
        this.sym = sym;
        this.lexeme = lexeme;
        this.line = line;
    }
        public override string ToString(){
            // output
            return $"{{ \"lexeme\": \"{this.lexeme}\", \"line\": {this.line} }}";
        }
}
    public class Tokenizer {
        string input; // stuff to tokenize
        int line; // current line number
        int index; // where we are in the input

        public Tokenizer() {
            
        }
        public void setInput(string inp) {
            this.input = inp;
            this.line = 1;
        }
        public Token next(){
            String sym;
            String lexeme;
            foreach( var t in Grammar.terminals) {
                Match M = t.rex.Match(this.input, this.index);
                if (M.Success) {
                    sym = t.sym;
                    lexeme = M.Groups[0].Value;
                    break;
                }
            }


            while(this.index < this.input.Length && Char.IsWhiteSpace(this.input[this.index])){
                if (this.input[this.index] == '\n'){
                    this.line++;
                }
                this.index++;
            }

            string tmp="";
            while (this.index < this.input.Length && !Char.IsWhiteSpace(this.input[this.index])){
                tmp += this.input[this.index];
                this.index++;
            }
            if (tmp.Length == 0){
                return null; // at end of line
            }

            if (sym == null) {
                Console.WriteLine("Error at line: " + this.line);
                Environment.Exit(1);
            }
            return new Token(tmp, this.line);

        }
    }
}