using System.Text.RegularExpressions;

namespace lab {
    public class Token {
        public string sym { get; set; } //maybe use later
        public string lexeme { get; set; } 
        public int line { get; set; } 
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
        Token lastToken = null; // Added to track the last non-whitespace token

        public Tokenizer(string inp) {
            this.input = inp;
            this.line = 1;
            this.index = 0;
        }

        // we can insert an implicit semicolon after these things
        List<string> implicitSemiAfter = new(){ "NUM", "RPAREN", "ID" };

        public void setInput(string inp) {
            this.input = inp;
            this.line = 1;
            this.index = 0;
        }

        public Token next(){
            // Skip whitespace
            while (index < input.Length) {
                char currentChar = input[index];

                // Skip whitespace characters
                if (char.IsWhiteSpace(currentChar)) {
                    if (currentChar == '\n') {
                        line++;
                        if (lastToken != null && implicitSemiAfter.Contains(lastToken.sym) && nesting.Count == 0) {
                            lastToken = new Token("SEMI", "", line -1);
                            return lastToken;
                        }
                    }
                    index++;
                    continue;
                }

                Token bestMatch = null;

                // maximal munch
                foreach (var t in Grammar.terminals) {
                    Match M = t.rex.Match(this.input, this.index);
                    if (M.Success && (bestMatch == null || M.Length > bestMatch.lexeme.Length)) {
                        bestMatch = new Token(t.sym, M.Groups[0].Value, this.line);
                    }
                }

                if (bestMatch == null) {
                    Console.Error.WriteLine($"Error: Unexpected character at line {line}, index {index}");
                    Environment.Exit(1);
                }

                index += bestMatch.lexeme.Length;

                if (bestMatch.sym == "COMMENT") {
                    continue;
                }

                // handle paranthesis and brackets
                if (bestMatch.sym == "LPAREN" || bestMatch.sym == "LBRACE" ) {
                    nesting.Push(bestMatch);
                }
                else if (bestMatch.sym == "RPAREN" || bestMatch.sym == "RBRACE" ) {
                    if (nesting.Count > 0) {
                        nesting.Pop();
                    }
                }
                
                lastToken = bestMatch;
                return bestMatch;
            }

            return null;

        } // end next()
        
    } // end of class Tokenizer

} // end of namespace