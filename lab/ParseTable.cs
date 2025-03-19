namespace lab{
public static class ParseTable{
    public static List<Dictionary<string,ParseAction> > table = new() {
        // DFA STATE 0
        // S' :: • S ║ $
        // S :: • decls ║ $
        // decls :: • decl decls ║ $
        // decl :: • vardecl ║ TYPE VOID
        // decl :: • funcdecl ║ TYPE VOID
        // vardecl :: • nonVoidType ID SEMI ║ TYPE VOID
        // funcdecl :: • anyType ID LP RP SEMI ║ TYPE VOID
        // nonVoidType :: • TYPE ║ ID
        // anyType :: • VOID ║ ID
        // anyType :: • TYPE ║ ID
        new Dictionary<string,ParseAction>(){
                {"S" , new ParseAction(PAction.SHIFT, 1, null, -1)},
                {"decls" , new ParseAction(PAction.SHIFT, 2, null, -1)},
                {"decl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"nonVoidType" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"anyType" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"TYPE" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VOID" , new ParseAction(PAction.SHIFT, 9, null, -1)},
        },
        // DFA STATE 1
        // S' :: S • ║ $
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 1, "S'", 9)},
        },
        // DFA STATE 2
        // S :: decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 1, "S", 0)},
        },
        // DFA STATE 3
        // decls :: decl • decls ║ $
        // decls :: • decl decls ║ $
        // decl :: • vardecl ║ TYPE VOID
        // decl :: • funcdecl ║ TYPE VOID
        // vardecl :: • nonVoidType ID SEMI ║ TYPE VOID
        // funcdecl :: • anyType ID LP RP SEMI ║ TYPE VOID
        // nonVoidType :: • TYPE ║ ID
        // anyType :: • VOID ║ ID
        // anyType :: • TYPE ║ ID
        new Dictionary<string,ParseAction>(){
                {"decls" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"decl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"nonVoidType" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"anyType" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"TYPE" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VOID" , new ParseAction(PAction.SHIFT, 9, null, -1)},
        },
        // DFA STATE 4
        // decl :: vardecl • ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.REDUCE, 1, "decl", 2)},
                {"VOID" , new ParseAction(PAction.REDUCE, 1, "decl", 2)},
        },
        // DFA STATE 5
        // decl :: funcdecl • ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.REDUCE, 1, "decl", 3)},
                {"VOID" , new ParseAction(PAction.REDUCE, 1, "decl", 3)},
        },
        // DFA STATE 6
        // vardecl :: nonVoidType • ID SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 14, null, -1)},
        },
        // DFA STATE 7
        // funcdecl :: anyType • ID LP RP SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 10, null, -1)},
        },
        // DFA STATE 8
        // nonVoidType :: TYPE • ║ ID
        // anyType :: TYPE • ║ ID
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.REDUCE, 1, "nonVoidType", 6)},
        },
        // DFA STATE 9
        // anyType :: VOID • ║ ID
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.REDUCE, 1, "anyType", 7)},
        },
        // DFA STATE 10
        // funcdecl :: anyType ID • LP RP SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"LP" , new ParseAction(PAction.SHIFT, 11, null, -1)},
        },
        // DFA STATE 11
        // funcdecl :: anyType ID LP • RP SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"RP" , new ParseAction(PAction.SHIFT, 12, null, -1)},
        },
        // DFA STATE 12
        // funcdecl :: anyType ID LP RP • SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"SEMI" , new ParseAction(PAction.SHIFT, 13, null, -1)},
        },
        // DFA STATE 13
        // funcdecl :: anyType ID LP RP SEMI • ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.REDUCE, 5, "funcdecl", 5)},
                {"VOID" , new ParseAction(PAction.REDUCE, 5, "funcdecl", 5)},
        },
        // DFA STATE 14
        // vardecl :: nonVoidType ID • SEMI ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"SEMI" , new ParseAction(PAction.SHIFT, 15, null, -1)},
        },
        // DFA STATE 15
        // vardecl :: nonVoidType ID SEMI • ║ TYPE VOID
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.REDUCE, 3, "vardecl", 4)},
                {"VOID" , new ParseAction(PAction.REDUCE, 3, "vardecl", 4)},
        },
        // DFA STATE 16
        // decls :: decl decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 2, "decls", 1)},
        },
    }; //close the table initializer
} //close the ParseTable class
} //close the namespace lab thing
