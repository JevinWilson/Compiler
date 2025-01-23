namespace lab {

    public class CompilersAreGreat {
        public static void Main(string[] args) {

            // initialize our grammar
            Terminals.makeAllOfTheTerminals();

            string inp = File.ReadAllText(args[0]);
            var tokens = new List<Token>();
            var T = new Tokenizer();
            T.setInput(inp);
            while (true){
                var token = T.next();
                if (token == null){
                    break;
                }
                tokens.Add(token);
            }
                foreach (var t in tokens){
                Console.WriteLine(t);
            }
        }
    } // end of class

} // end of namespace
