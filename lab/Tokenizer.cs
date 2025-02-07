using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace lab {
    // Represents a token with a symbol, lexeme, and line number
    public class Token {
        public string sym { get; set; } // Symbol of the token
        public string lexeme { get; set; } // The actual text of the token
        public int line { get; set; } // Line number where the token is found

        public Token(string sym, string lexeme, int line) {
            this.sym = sym;
            this.lexeme = lexeme;
            this.line = line;
        }

        // Converts the token to a formatted string
        public override string ToString() {
            // Properly escape characters for JSON
            var lex = lexeme.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
            return $"{{ \"sym\": \"{this.sym}\" ,\"line\": {this.line}, \"lexeme\": \"{lex}\" }}";
        }
    } // end of class Token

    public class Tokenizer {
        string input; // The input string to tokenize
        public int line { get; private set; } // Current line number
        int index; // Current index in the input string
        Stack<Token> nesting = new(); // Stack to track nesting structures
        Token lastToken = null; // The last non-whitespace token processed

        // Tokens that should allow implicit semicolons after them
        List<string> implicitSemiAfter = new() { 
            "NUM", "FNUM", "TYPE", "BOOLCONST", "STRINGCONST",
            "BREAK", "CONTINUE", "RETURN", "ID", "RPAREN", "RBRACE"
        };

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

        public Token next() {
            while (index < input.Length) {
                char currentChar = input[index];

                // Skip whitespace characters
                if (char.IsWhiteSpace(currentChar)) {
                    if (currentChar == '\n') {
                        // Handle implicit semicolons at line breaks
                        if (lastToken != null && implicitSemiAfter.Contains(lastToken.sym) && nesting.Count == 0) {
                            // Insert implicit semicolon
                            lastToken = new Token("SEMI", "", line);
                            index++; // Move past the newline character
                            return lastToken;
                        }
                        line++;
                    }
                    index++;
                    continue;
                }

                Token bestMatch = null;

                // Maximal munch: match the longest possible token
                foreach (var t in Grammar.terminals) {
                    Match M = t.rex.Match(this.input, this.index);
                    if (M.Success && M.Index == this.index) { // Ensure match starts at current index
                        if (bestMatch == null || M.Length > bestMatch.lexeme.Length) {
                            bestMatch = new Token(t.sym, M.Value, this.line);
                        }
                    }
                }

                if (bestMatch == null) {
                    Console.Error.WriteLine($"Error: Unexpected character '{currentChar}' at line {line}, index {index}");
                    Environment.Exit(1);
                }

                index += bestMatch.lexeme.Length;

                // Ignore comments
                if (bestMatch.sym == "COMMENT") {
                    int newLineCount = bestMatch.lexeme.Count(c => c == '\n');
                    line += newLineCount;
                    continue;
                }

                // Handle opening symbols: parentheses and brackets
                if (bestMatch.sym == "LPAREN" || bestMatch.sym == "LBRACKET" || bestMatch.sym == "LBRACE") {
                    // Push opening symbols onto the stack
                    nesting.Push(bestMatch);
                }
                // Handle closing symbols: check for matching opening symbols
                else if (bestMatch.sym == "RPAREN" || bestMatch.sym == "RBRACKET" || bestMatch.sym == "RBRACE") {
                    if (nesting.Count > 0) {
                        var last = nesting.Peek(); // Look at the top of the stack without popping
                        bool symbolsMatch = (bestMatch.sym == "RPAREN" && last.sym == "LPAREN") ||
                                            (bestMatch.sym == "RBRACKET" && last.sym == "LBRACKET") ||
                                            (bestMatch.sym == "RBRACE" && last.sym == "LBRACE");
                        if (symbolsMatch) {
                            // Symbols match, pop the opening symbol from the stack
                            nesting.Pop();
                        } else {
                            // Symbols do not match, report error with expected opening symbol
                            var expectedOpening = bestMatch.sym switch {
                                "RPAREN" => "LPAREN",
                                "RBRACKET" => "LBRACKET",
                                "RBRACE" => "LBRACE",
                                _ => "UNKNOWN"
                            };
                            var errorOutput = new {
                                returncode = 2,
                                tokens = $"Error at line {line}: Expected {expectedOpening} to match with {bestMatch.sym}\n",
                                error = line
                            };
                            string jsonOutput = JsonSerializer.Serialize(errorOutput, new JsonSerializerOptions {
                                WriteIndented = true,
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                            });
                            Console.WriteLine(jsonOutput);
                            Environment.Exit(2);
                        }
                    } else {
                        // No opening symbol in the stack when a closing symbol is encountered
                        var expectedOpening = bestMatch.sym switch {
                            "RPAREN" => "LPAREN",
                            "RBRACKET" => "LBRACKET",
                            "RBRACE" => "LBRACE",
                            _ => "UNKNOWN"
                        };
                        var errorOutput = new {
                            returncode = 2,
                            tokens = $"Error at line {line}: Expected {expectedOpening} to match with {bestMatch.sym}\n",
                            error = line
                        };
                        // used chat for all json stuff
                        string jsonOutput = JsonSerializer.Serialize(errorOutput, new JsonSerializerOptions {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });
                        Console.WriteLine(jsonOutput);
                        Environment.Exit(2);
                    }
                }

                lastToken = bestMatch;
                return bestMatch;
            }

            // Insert implicit semicolon at end if needed
            if (lastToken != null && implicitSemiAfter.Contains(lastToken.sym) && nesting.Count == 0) {
                lastToken = new Token("SEMI", "", line);
                return lastToken;
            }

            return null;
        } // end next()

    } // end of class Tokenizer

} // end of namespace