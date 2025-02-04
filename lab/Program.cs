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

            while (true){
                var token = T.next();
                if (token == null){
                    break;
                }
                tokens.Add(token);
            }
            
            var output = new {
                returncode = tokens.Count > 0 ? 0 : errorCode,
                tokens = tokens,
                error = errorCode
            };

            string jsonOutput = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(jsonOutput);
        }
    } // end of class

} // end of namespace
