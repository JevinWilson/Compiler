
namespace lab{

public class Productions{
    public static void makeThem(){
       

        Grammar.defineProductions( new PSpec[] {
            new("S :: decls"),
            new("decls :: decl decls"),
            new("decl :: vardecl | funcdecl"),
            new("vardecl :: nonVoidType ID SEMI"),
            new("funcdecl :: anyType ID LP RP SEMI"),
            new("nonVoidType :: TYPE"),
            new("anyType :: VOID | TYPE")
        });
        
    }
}

}