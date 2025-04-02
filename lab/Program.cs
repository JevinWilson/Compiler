namespace lab{

    public class CompilersAreGreat{
        public static void Main(string[] args){

            //initialize our grammar
            Terminals.makeThem();
            
            Productions.makeThem();
            ProductionsExpr.makeThem();
            Grammar.addTerminals(new Terminal[] {new("WHITESPACE",       @"\s+" )});

    
            Grammar.check();

            if( args.Length == 1 && args[0] == "-g" ){
                Grammar.computeNullableAndFirst();
                //Grammar.dump();
                DFA.makeDFA(); //time consuming
                //DFA.dump();
                TableWriter.create();
                //ParseTable.dump();
                return;
            }
            TreeNode root = null;
            string inp = File.ReadAllText(args[0]);
            var tokens = new List<Token>();
            var T = new Tokenizer(inp);
            root = Parser.parse(T);
            root.collectClassNames();
            root.setNodeTypes();

            root.removeUnitProductions();     
            root.print();

            using(var w = new StreamWriter("tree.json")){
                root.toJson(w);
            }
            return;

            }
            
        } // end class
        
    } // end namespace