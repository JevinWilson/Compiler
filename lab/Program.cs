using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab {

    public class CompilersAreGreat {
        public static void Main(string[] args) {

            // initialize our grammar
            Terminals.makeAllOfTheTerminals();
            Grammar.addTerminals(new Terminal[] {new("WHITESPACE", @"\s+" )});

            // read input
            string inp = File.ReadAllText(args[0]);
            var tokens = new List<Token>();
            var T = new Tokenizer(inp);
            T.setInput(inp);

            // error code
            int errorCode = -1;
            string errorMsg = "";
            int errorLine = -1;

            try {
                while (true) {
                    var token = T.next();
                    if (token == null) {
                        break;
                    }
                    tokens.Add(token);
                }
            } catch (Exception ex) {
                errorMsg = ex.Message;
                errorCode = 2;
                errorLine = T.line;
            }
            
            var output = new {
                returncode = errorCode == -1 ? 0 : errorCode,
                tokens = tokens.Count > 0 ? tokens : new List<Token>(), 
                error = errorCode == -1 ? -1 : T.line 
            };

            // used chat for all the Json stuff
            // Prompt: How do I format this (output from last assignment) to match this (output from result-tx.txt)?
            var options = new JsonSerializerOptions {
                WriteIndented = true,
                // print '+', '-', etc
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new TokenConverter() }
            };

            string jsonOutput = JsonSerializer.Serialize(output, options);
            Console.WriteLine(jsonOutput);

        }
    } // end of class

} // end of namespace

// to run: dotnet run -- "C:\Users\jaw06\Desktop\Compiler\lab\tests\testcases\(fileneme).txt"