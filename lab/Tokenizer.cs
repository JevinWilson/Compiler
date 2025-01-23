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
            return $"{{ \"sym\": \"{this.sym}\" , \"line\" : {this.line}, \"lexeme\" : \"{this.lexeme}\"  }}";
        }
    } // end of class Token

    public class Tokenizer {
        string input; // stuff to tokenize
        int line; // current line number
        int index; // where we are in the input

        public Tokenizer(string inp) {
            this.input = inp;
            this.line = 1;
            this.index = 0;
        }

        public void setInput(string inp) {
            this.input = inp;
            this.line = 1;
            this.index = 0;
        }

        public Token next(){
            // Skip whitespace
            while (index < input.Length && char.IsWhiteSpace(input[index])) {
                if (input[index] == '\n') {
                    line++;
                }
                index++;
            }

            if (index >= input.Length) {
                return null;
            }

            String sym = null;
            String lexeme = null;
            foreach (var t in Grammar.terminals) {
                Match M = t.rex.Match(this.input, this.index);
                if (M.Success) {
                    sym = t.sym;
                    lexeme = M.Groups[0].Value;
                    index += lexeme.Length;
                    break;
                }
            }

            if (sym == null) {
                // Print error message
                Console.Error.WriteLine($"Error: Unexpected character at line {line}, index {index}");
                Environment.Exit(1);
            }

            return new Token(sym, lexeme, line);
        }
        
    } // end of class Tokenizer

} // end of namespace