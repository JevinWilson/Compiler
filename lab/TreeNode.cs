using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace lab {

public class TreeNode {
    public string sym;  // terminal name or nonterminal
    public Token token; // only meaningful for terminals
    public List<TreeNode> children = new();
    public int productionNumber;

    [JsonIgnore()]
    public TreeNode parent = null;

    // only meaningful for tree nodes that are ID's which are variables
    public VarInfo varInfo = null;

    [JsonConverter(typeof(NodeTypeJsonConverter))]
    public NodeType nodeType = null;

    public TreeNode this[string childSym] {
        get {
            foreach(var c in this.children) {
                if(c.sym == childSym) {
                    return c;
                }
            }
            throw new Exception("No such child");
        }
    }

    Production production {
        get {
            if(this.productionNumber >= 0)
                return Grammar.productions[this.productionNumber];
            return null;
        }
    }

    // this is here for json deserialization
    public TreeNode() { }

    public TreeNode(string sym, Token tok, int prodNum) {
        this.sym = sym;
        this.token = tok;
        this.productionNumber = prodNum;
    }

    // nonterminal node
    public TreeNode(string sym, int prodNum) : this(sym, null, prodNum) { }

    // terminal node
    public TreeNode(Token tok) : this(tok.sym, tok, -1) { }

    public void appendChild(TreeNode n) {
        n.parent = this;
        this.children.Add(n);
    }

    public void setParents() {
        foreach(var c in this.children) {
            c.parent = this;
            c.setParents();
        }
    }
    
    public void writeJsonToFile(string filename) {
        StringBuilder sb = new StringBuilder();
        toJson(this, sb, 0);
        File.WriteAllText(filename, sb.ToString());
    }

    private void toJson(TreeNode node, StringBuilder sb, int depth) {
        string indent = new string(' ', depth * 2);
        
        // Opening brace
        sb.AppendLine(indent + "{");
        
        // Add the sym property
        sb.AppendLine(indent + "  \"sym\": \"" + node.sym + "\",");
        
        // Add the children array
        sb.AppendLine(indent + "  \"children\": [");
        
        // Process each child node
        for (int i = 0; i < node.children.Count; i++) {
            // Add proper indentation for child nodes
            if (depth == 0) {
                // Special indentation for top-level children
                sb.Append(indent + "  ");
            } else {
                // Regular indentation for nested children
                sb.Append(indent + "  ");
            }
            
            // Format the child node
            toJson(node.children[i], sb, depth + 1);
            
            // Add comma between children, with proper formatting
            if (i < node.children.Count - 1) {
                sb.AppendLine();
                if (depth == 0) {
                    // Special formatting for top-level commas
                    sb.AppendLine(indent + "  ,");
                } else {
                    // Regular formatting for nested commas
                    sb.AppendLine(indent + "  ,");
                }
            } else {
                // No comma after last child
                sb.AppendLine();
            }
        }
        
        // Close the children array
        sb.Append(indent + "  ]");
        
        // Add token information if present
        if (node.token != null) {
            sb.AppendLine(",");
            sb.Append(indent + "  \"token\": { \"sym\": \"" + node.token.sym + 
                     "\", \"lexeme\": \"" + node.token.lexeme + 
                     "\", \"line\": " + node.token.line + " }");
        }
        
        // Closing brace (no newline if part of a list)
        sb.Append(indent + "}");
    }

    public void print(string prefix = "") {
        string HLINE = "─"; // (Unicode \u2500)
        string VLINE = "│";  //(\u2502)
        string TEE = "├";  //(\u251c)
        string ELL = "└"; // (\u2514) 

        bool lastChild = this.parent != null && this == this.parent.children[^1];
        //if this node is the last child
        if(this.parent == null) {
            //root
            Console.WriteLine(this.ToString());
        } else {
            if(lastChild) {
                Console.WriteLine(prefix + "  " + ELL + HLINE + this.ToString());
            } else {
                Console.WriteLine(prefix + "  " + TEE + HLINE + this.ToString());
            }
        }

        foreach(var c in this.children) {
            if(this.parent == null)
                c.print("");
            else {
                if(lastChild)
                    c.print(prefix + "   ");
                else
                    c.print(prefix + "  " + VLINE);
            }
        }
    }

    public override string ToString() {
        string tmp = this.sym;
        
        if(this.token != null)
            tmp += $" ({this.token.lexeme})";

        if(this.nodeType != null)
            tmp += " nodeType=" + this.nodeType.ToString();

        if(this.varInfo != null)
            tmp += " varInfo=" + this.varInfo;

        return tmp;
    }

    public void removeUnitProductions() {
        for(int i = 0; i < this.children.Count; ++i)
            this.children[i].removeUnitProductions();

        if(this.children.Count == 1 && this.parent != null) {
            this.parent.replaceChild(this, this.children[0]);
        }
    }

    public void replaceChild(TreeNode n, TreeNode c) {
        //replace child n with c
        for(int i = 0; i < this.children.Count; ++i) {
            if(this.children[i] == n) {
                this.children[i] = c;
                c.parent = this;
                n.parent = null;
                return;
            }
        }
        throw new Exception();
    }

    public void collectClassNames() {
        this.production?.pspec.collectClassNames(this);
    }

    public void setNodeTypes() {
        this.production?.pspec.setNodeTypes(this);
    }
}

}