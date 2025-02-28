using System.IO;

namespace lab {
    public class DFANode {
        public static List<DFANode> allNodes = new List<DFANode>();
        public int id;
        public HashSet<LRItem> items;
        public Dictionary<string, DFANode> transitions;

        public DFANode(HashSet<LRItem> items) {
            this.id = allNodes.Count;
            this.items = items;
            this.transitions = new Dictionary<string, DFANode>();
            allNodes.Add(this);
        }

        public override string ToString() {
            var result = $"Node q{id}:\n";
            
            foreach (var item in items) {
                result += $"  {item}\n";
            }
            
            foreach (var transition in transitions) {
                result += $"  --{transition.Key}--> q{transition.Value.id}\n";
            }
            
            return result;
        }

        public static void dump() {
            foreach (var q in allNodes) {
                Console.WriteLine(q);
                Console.WriteLine("-----------------------");
            }
        }

        public void toDot(string filename) {
            using (var sw = new StreamWriter(filename)) {
                sw.WriteLine("digraph d {");
                sw.WriteLine("    node [shape=rectangle,fontname=Helvetica];");
                
                // write all nodes
                foreach (var node in allNodes) {
                    string label = $"q{node.id}\\n";
                    foreach (var item in node.items) {
                        // escape any double quotes in the item's string representation
                        label += item.ToString().Replace("\"", "\\\"") + "\\n";
                    }
                    sw.WriteLine($"    q{node.id} [label=\"{label}\"];");
                }
                
                // write all transitions
                foreach (var node in allNodes) {
                    foreach (var transition in node.transitions) {
                        sw.WriteLine($"    q{node.id} -> q{transition.Value.id} [label=\"{transition.Key}\"];");
                    }
                }
                
                sw.WriteLine("}");
            }
        }
    }
}