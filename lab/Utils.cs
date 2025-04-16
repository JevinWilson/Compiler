namespace lab{
    public static class Utils{
        public static void error(Token t, string msg){
            Console.WriteLine($"Error at line {t.line}: {msg}");
            Environment.Exit(1);
        }

        public static void typeCheck( TreeNode n, 
                    NodeType output,    //null -> output type matches input
                    params NodeType[] allowedInputs)
        {
            foreach(var c in n.children){
                c.setNodeTypes();
            }
            if(n.children[0].nodeType != n.children[2].nodeType){
                Utils.error(n.children[1].token, $"Node type mismatch! ({n.children[0].nodeType} and {n.children[2].nodeType})");
            }
            if(!allowedInputs.Contains(n.children[0].nodeType)){
                Utils.error(n.children[1].token, $"Type {n.children[0].nodeType} is not in the allowed types! ({String.Join<NodeType>(',',allowedInputs)})");
            }
            if( output == null )
                n.nodeType = n.children[0].nodeType;
            else
                n.nodeType = output;
        }

        public static void epilogue(Token t){
            Asm.add(new OpComment( "Epilogue at line "+t.line));
            Asm.add( new OpMov( src: Register.rbp, dest: Register.rsp));
            Asm.add( new OpComment( $"Popping register {Register.rbp}..." ));
            Asm.add( new OpPop( Register.rbp, StorageClass.NO_STORAGE_CLASS));
            Asm.add( new OpRet());
        }

    }
}