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
            var lex = lexeme.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
            // output
            return $"{{ \"sym\": \"{this.sym}\" , \"line\" : {this.line}, \"lexeme\" : \"{lex}\"  }}";
        }
    } // end of class Token

    public class Tokenizer {
        string input; // stuff to tokenize
        int line; // current line number
        int index; // where we are in the input

        Stack<Token> nesting = new();
        public Tokenizer(string inp) {
            this.input = inp;
            this.line = 1;
            this.index = 0;
        }

        // we can insert an implicit semicolon after these things
        List<string> implicitSemiAfter = new(){"NUM", "RPAREN"};

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
                if (verbose) {
                    Console.WriteLine("Trying terminal "+t.sym+ "   Matched? " +M.Success);
                }
                if (M.Success) {
                    // FIXME: need maximal munch
                    sym = t.sym;
                    lexeme = M.Groups[0].Value;
                    index += lexeme.Length;
                    break;
                }
            }

            if (sym =="WHITESPACE" && lexeme.Contains('\n') && nesting.Count == 0) {
                // implicit semicolon
                // the previous token
                // is in my list, return semi
                // don't forget to update last token returned
                // return new Token("SEMI", "", this.line);

            if (sym == null) {
                // Print error message
                Console.Error.WriteLine($"Error: Unexpected character at line {line}, index {index}");
                Environment.Exit(1);
            }
            this.index += lexeme.Length;

            //return new Token(sym, lexeme, line);

            var tok = new Token(sym, lexeme, line);
            if (verbose) {
                Console.WriteLine("RETURNING TOKEN "+tok);
            }

            // FIXME: adjust line number
            if (sym == "WHITESPACE" ){
                return this.next();
            }

            // do maintenance on the nesting stack
            // if LPAREN or LBRACE: pust to stack
            // if RPAREN or RBRACE: pop from stack (first do checks)

            // update my 'last token' data: either store the token
            // itself or just store its sym or just store a bool
            // that says if it's in the eligible for implicit semi
            return tok;

            
        } // end next()
        
    } // end of class Tokenizer

} // end of namespace