namespace lab{
public static class ParseTable{
    public static List<Dictionary<string,ParseAction> > table = new() {
        // DFA STATE 0
        // S' :: • S ║ $
        // S :: • decls ║ $
        // decls :: • funcdecl decls ║ $
        // decls :: • classdecl decls ║ $
        // decls :: • vardecl decls ║ $
        // decls :: • SEMI decls ║ $
        // decls :: • ║ $
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $
        // classdecl :: • CLASS ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"S" , new ParseAction(PAction.SHIFT, 1, null, -1)},
                {"decls" , new ParseAction(PAction.SHIFT, 2, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"classdecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"CLASS" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"$",new ParseAction(PAction.REDUCE, 0, "decls", 5)},
        },
        // DFA STATE 1
        // S' :: S • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 1, "S'", 46)},
        },
        // DFA STATE 2
        // S :: decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 1, "S", 0)},
        },
        // DFA STATE 3
        // decls :: funcdecl • decls ║ $
        // decls :: • funcdecl decls ║ $
        // decls :: • classdecl decls ║ $
        // decls :: • vardecl decls ║ $
        // decls :: • SEMI decls ║ $
        // decls :: • ║ $
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $
        // classdecl :: • CLASS ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"decls" , new ParseAction(PAction.SHIFT, 94, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"classdecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"CLASS" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"$",new ParseAction(PAction.REDUCE, 0, "decls", 5)},
        },
        // DFA STATE 4
        // decls :: classdecl • decls ║ $
        // decls :: • funcdecl decls ║ $
        // decls :: • classdecl decls ║ $
        // decls :: • vardecl decls ║ $
        // decls :: • SEMI decls ║ $
        // decls :: • ║ $
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $
        // classdecl :: • CLASS ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"decls" , new ParseAction(PAction.SHIFT, 93, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"classdecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"CLASS" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"$",new ParseAction(PAction.REDUCE, 0, "decls", 5)},
        },
        // DFA STATE 5
        // decls :: vardecl • decls ║ $
        // decls :: • funcdecl decls ║ $
        // decls :: • classdecl decls ║ $
        // decls :: • vardecl decls ║ $
        // decls :: • SEMI decls ║ $
        // decls :: • ║ $
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $
        // classdecl :: • CLASS ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"decls" , new ParseAction(PAction.SHIFT, 92, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"classdecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"CLASS" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"$",new ParseAction(PAction.REDUCE, 0, "decls", 5)},
        },
        // DFA STATE 6
        // decls :: SEMI • decls ║ $
        // decls :: • funcdecl decls ║ $
        // decls :: • classdecl decls ║ $
        // decls :: • vardecl decls ║ $
        // decls :: • SEMI decls ║ $
        // decls :: • ║ $
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $
        // classdecl :: • CLASS ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"decls" , new ParseAction(PAction.SHIFT, 91, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"classdecl" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 5, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 6, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"CLASS" , new ParseAction(PAction.SHIFT, 8, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"$",new ParseAction(PAction.REDUCE, 0, "decls", 5)},
        },
        // DFA STATE 7
        // funcdecl :: FUNC • ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 39, null, -1)},
        },
        // DFA STATE 8
        // classdecl :: CLASS • ID LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 22, null, -1)},
        },
        // DFA STATE 9
        // vardecl :: VAR • ID COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR • ID COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR • ID COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR • ID COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 10, null, -1)},
        },
        // DFA STATE 10
        // vardecl :: VAR ID • COLON TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID • COLON TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID • COLON ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID • COLON ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"COLON" , new ParseAction(PAction.SHIFT, 11, null, -1)},
        },
        // DFA STATE 11
        // vardecl :: VAR ID COLON • TYPE ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID COLON • TYPE EQ expr ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID COLON • ID ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID COLON • ID EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.SHIFT, 12, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 13, null, -1)},
        },
        // DFA STATE 12
        // vardecl :: VAR ID COLON TYPE • ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID COLON TYPE • EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"EQ" , new ParseAction(PAction.SHIFT, 20, null, -1)},
                {"SEMI",new ParseAction(PAction.REDUCE, 4, "vardecl", 39)},
                {"FUNC",new ParseAction(PAction.REDUCE, 4, "vardecl", 39)},
                {"CLASS",new ParseAction(PAction.REDUCE, 4, "vardecl", 39)},
                {"VAR",new ParseAction(PAction.REDUCE, 4, "vardecl", 39)},
                {"$",new ParseAction(PAction.REDUCE, 4, "vardecl", 39)},
        },
        // DFA STATE 13
        // vardecl :: VAR ID COLON ID • ║ SEMI FUNC CLASS VAR $
        // vardecl :: VAR ID COLON ID • EQ expr ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"EQ" , new ParseAction(PAction.SHIFT, 14, null, -1)},
                {"SEMI",new ParseAction(PAction.REDUCE, 4, "vardecl", 41)},
                {"FUNC",new ParseAction(PAction.REDUCE, 4, "vardecl", 41)},
                {"CLASS",new ParseAction(PAction.REDUCE, 4, "vardecl", 41)},
                {"VAR",new ParseAction(PAction.REDUCE, 4, "vardecl", 41)},
                {"$",new ParseAction(PAction.REDUCE, 4, "vardecl", 41)},
        },
        // DFA STATE 14
        // vardecl :: VAR ID COLON ID EQ • expr ║ SEMI FUNC CLASS VAR $
        // expr :: • NUM ║ SEMI FUNC CLASS VAR $
        // expr :: • ID ║ SEMI FUNC CLASS VAR $
        // expr :: • NUM ADDOP NUM ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 15, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 15
        // vardecl :: VAR ID COLON ID EQ expr • ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 6, "vardecl", 42)},
                {"FUNC",new ParseAction(PAction.REDUCE, 6, "vardecl", 42)},
                {"CLASS",new ParseAction(PAction.REDUCE, 6, "vardecl", 42)},
                {"VAR",new ParseAction(PAction.REDUCE, 6, "vardecl", 42)},
                {"$",new ParseAction(PAction.REDUCE, 6, "vardecl", 42)},
        },
        // DFA STATE 16
        // expr :: NUM • ║ SEMI FUNC CLASS VAR $ EQ RPAREN
        // expr :: NUM • ADDOP NUM ║ SEMI FUNC CLASS VAR $ EQ RPAREN
        new Dictionary<string,ParseAction>(){
                {"ADDOP" , new ParseAction(PAction.SHIFT, 18, null, -1)},
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"FUNC",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"CLASS",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"VAR",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"$",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"EQ",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
                {"RPAREN",new ParseAction(PAction.REDUCE, 1, "expr", 43)},
        },
        // DFA STATE 17
        // expr :: ID • ║ SEMI FUNC CLASS VAR $ EQ RPAREN
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"FUNC",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"CLASS",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"VAR",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"$",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"EQ",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
                {"RPAREN",new ParseAction(PAction.REDUCE, 1, "expr", 44)},
        },
        // DFA STATE 18
        // expr :: NUM ADDOP • NUM ║ SEMI FUNC CLASS VAR $ EQ RPAREN
        new Dictionary<string,ParseAction>(){
                {"NUM" , new ParseAction(PAction.SHIFT, 19, null, -1)},
        },
        // DFA STATE 19
        // expr :: NUM ADDOP NUM • ║ SEMI FUNC CLASS VAR $ EQ RPAREN
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"FUNC",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"CLASS",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"VAR",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"$",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"EQ",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
                {"RPAREN",new ParseAction(PAction.REDUCE, 3, "expr", 45)},
        },
        // DFA STATE 20
        // vardecl :: VAR ID COLON TYPE EQ • expr ║ SEMI FUNC CLASS VAR $
        // expr :: • NUM ║ SEMI FUNC CLASS VAR $
        // expr :: • ID ║ SEMI FUNC CLASS VAR $
        // expr :: • NUM ADDOP NUM ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 21, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 21
        // vardecl :: VAR ID COLON TYPE EQ expr • ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 6, "vardecl", 40)},
                {"FUNC",new ParseAction(PAction.REDUCE, 6, "vardecl", 40)},
                {"CLASS",new ParseAction(PAction.REDUCE, 6, "vardecl", 40)},
                {"VAR",new ParseAction(PAction.REDUCE, 6, "vardecl", 40)},
                {"$",new ParseAction(PAction.REDUCE, 6, "vardecl", 40)},
        },
        // DFA STATE 22
        // classdecl :: CLASS ID • LBRACE memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"LBRACE" , new ParseAction(PAction.SHIFT, 23, null, -1)},
        },
        // DFA STATE 23
        // classdecl :: CLASS ID LBRACE • memberdecls RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        // memberdecls :: • ║ RBRACE
        // memberdecls :: • SEMI memberdecls ║ RBRACE
        // memberdecls :: • membervardecl memberdecls ║ RBRACE
        // memberdecls :: • memberfuncdecl memberdecls ║ RBRACE
        // membervardecl :: • VAR ID COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        // memberfuncdecl :: • funcdecl ║ SEMI VAR FUNC RBRACE
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"memberdecls" , new ParseAction(PAction.SHIFT, 24, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 25, null, -1)},
                {"membervardecl" , new ParseAction(PAction.SHIFT, 26, null, -1)},
                {"memberfuncdecl" , new ParseAction(PAction.SHIFT, 27, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 28, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 29, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "memberdecls", 18)},
        },
        // DFA STATE 24
        // classdecl :: CLASS ID LBRACE memberdecls • RBRACE SEMI ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"RBRACE" , new ParseAction(PAction.SHIFT, 37, null, -1)},
        },
        // DFA STATE 25
        // memberdecls :: SEMI • memberdecls ║ RBRACE
        // memberdecls :: • ║ RBRACE
        // memberdecls :: • SEMI memberdecls ║ RBRACE
        // memberdecls :: • membervardecl memberdecls ║ RBRACE
        // memberdecls :: • memberfuncdecl memberdecls ║ RBRACE
        // membervardecl :: • VAR ID COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        // memberfuncdecl :: • funcdecl ║ SEMI VAR FUNC RBRACE
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"memberdecls" , new ParseAction(PAction.SHIFT, 36, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 25, null, -1)},
                {"membervardecl" , new ParseAction(PAction.SHIFT, 26, null, -1)},
                {"memberfuncdecl" , new ParseAction(PAction.SHIFT, 27, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 28, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 29, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "memberdecls", 18)},
        },
        // DFA STATE 26
        // memberdecls :: membervardecl • memberdecls ║ RBRACE
        // memberdecls :: • ║ RBRACE
        // memberdecls :: • SEMI memberdecls ║ RBRACE
        // memberdecls :: • membervardecl memberdecls ║ RBRACE
        // memberdecls :: • memberfuncdecl memberdecls ║ RBRACE
        // membervardecl :: • VAR ID COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        // memberfuncdecl :: • funcdecl ║ SEMI VAR FUNC RBRACE
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"memberdecls" , new ParseAction(PAction.SHIFT, 35, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 25, null, -1)},
                {"membervardecl" , new ParseAction(PAction.SHIFT, 26, null, -1)},
                {"memberfuncdecl" , new ParseAction(PAction.SHIFT, 27, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 28, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 29, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "memberdecls", 18)},
        },
        // DFA STATE 27
        // memberdecls :: memberfuncdecl • memberdecls ║ RBRACE
        // memberdecls :: • ║ RBRACE
        // memberdecls :: • SEMI memberdecls ║ RBRACE
        // memberdecls :: • membervardecl memberdecls ║ RBRACE
        // memberdecls :: • memberfuncdecl memberdecls ║ RBRACE
        // membervardecl :: • VAR ID COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        // memberfuncdecl :: • funcdecl ║ SEMI VAR FUNC RBRACE
        // funcdecl :: • FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"memberdecls" , new ParseAction(PAction.SHIFT, 34, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 25, null, -1)},
                {"membervardecl" , new ParseAction(PAction.SHIFT, 26, null, -1)},
                {"memberfuncdecl" , new ParseAction(PAction.SHIFT, 27, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 28, null, -1)},
                {"funcdecl" , new ParseAction(PAction.SHIFT, 29, null, -1)},
                {"FUNC" , new ParseAction(PAction.SHIFT, 7, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "memberdecls", 18)},
        },
        // DFA STATE 28
        // membervardecl :: VAR • ID COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 30, null, -1)},
        },
        // DFA STATE 29
        // memberfuncdecl :: funcdecl • ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "memberfuncdecl", 23)},
                {"VAR",new ParseAction(PAction.REDUCE, 1, "memberfuncdecl", 23)},
                {"FUNC",new ParseAction(PAction.REDUCE, 1, "memberfuncdecl", 23)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 1, "memberfuncdecl", 23)},
        },
        // DFA STATE 30
        // membervardecl :: VAR ID • COLON TYPE SEMI ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"COLON" , new ParseAction(PAction.SHIFT, 31, null, -1)},
        },
        // DFA STATE 31
        // membervardecl :: VAR ID COLON • TYPE SEMI ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.SHIFT, 32, null, -1)},
        },
        // DFA STATE 32
        // membervardecl :: VAR ID COLON TYPE • SEMI ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"SEMI" , new ParseAction(PAction.SHIFT, 33, null, -1)},
        },
        // DFA STATE 33
        // membervardecl :: VAR ID COLON TYPE SEMI • ║ SEMI VAR FUNC RBRACE
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 5, "membervardecl", 22)},
                {"VAR",new ParseAction(PAction.REDUCE, 5, "membervardecl", 22)},
                {"FUNC",new ParseAction(PAction.REDUCE, 5, "membervardecl", 22)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 5, "membervardecl", 22)},
        },
        // DFA STATE 34
        // memberdecls :: memberfuncdecl memberdecls • ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE",new ParseAction(PAction.REDUCE, 2, "memberdecls", 21)},
        },
        // DFA STATE 35
        // memberdecls :: membervardecl memberdecls • ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE",new ParseAction(PAction.REDUCE, 2, "memberdecls", 20)},
        },
        // DFA STATE 36
        // memberdecls :: SEMI memberdecls • ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE",new ParseAction(PAction.REDUCE, 2, "memberdecls", 19)},
        },
        // DFA STATE 37
        // classdecl :: CLASS ID LBRACE memberdecls RBRACE • SEMI ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"SEMI" , new ParseAction(PAction.SHIFT, 38, null, -1)},
        },
        // DFA STATE 38
        // classdecl :: CLASS ID LBRACE memberdecls RBRACE SEMI • ║ SEMI FUNC CLASS VAR $
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 6, "classdecl", 17)},
                {"FUNC",new ParseAction(PAction.REDUCE, 6, "classdecl", 17)},
                {"CLASS",new ParseAction(PAction.REDUCE, 6, "classdecl", 17)},
                {"VAR",new ParseAction(PAction.REDUCE, 6, "classdecl", 17)},
                {"$",new ParseAction(PAction.REDUCE, 6, "classdecl", 17)},
        },
        // DFA STATE 39
        // funcdecl :: FUNC ID • LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"LPAREN" , new ParseAction(PAction.SHIFT, 40, null, -1)},
        },
        // DFA STATE 40
        // funcdecl :: FUNC ID LPAREN • optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        // optionalPdecls :: • ║ RPAREN
        // optionalPdecls :: • pdecls ║ RPAREN
        // pdecls :: • pdecl ║ RPAREN
        // pdecls :: • pdecl COMMA pdecls ║ RPAREN
        // pdecl :: • ID COLON TYPE ║ RPAREN COMMA
        new Dictionary<string,ParseAction>(){
                {"optionalPdecls" , new ParseAction(PAction.SHIFT, 41, null, -1)},
                {"pdecls" , new ParseAction(PAction.SHIFT, 42, null, -1)},
                {"pdecl" , new ParseAction(PAction.SHIFT, 43, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 44, null, -1)},
                {"RPAREN",new ParseAction(PAction.REDUCE, 0, "optionalPdecls", 12)},
        },
        // DFA STATE 41
        // funcdecl :: FUNC ID LPAREN optionalPdecls • RPAREN optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RPAREN" , new ParseAction(PAction.SHIFT, 49, null, -1)},
        },
        // DFA STATE 42
        // optionalPdecls :: pdecls • ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"RPAREN",new ParseAction(PAction.REDUCE, 1, "optionalPdecls", 13)},
        },
        // DFA STATE 43
        // pdecls :: pdecl • ║ RPAREN
        // pdecls :: pdecl • COMMA pdecls ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"COMMA" , new ParseAction(PAction.SHIFT, 47, null, -1)},
                {"RPAREN",new ParseAction(PAction.REDUCE, 1, "pdecls", 14)},
        },
        // DFA STATE 44
        // pdecl :: ID • COLON TYPE ║ RPAREN COMMA
        new Dictionary<string,ParseAction>(){
                {"COLON" , new ParseAction(PAction.SHIFT, 45, null, -1)},
        },
        // DFA STATE 45
        // pdecl :: ID COLON • TYPE ║ RPAREN COMMA
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.SHIFT, 46, null, -1)},
        },
        // DFA STATE 46
        // pdecl :: ID COLON TYPE • ║ RPAREN COMMA
        new Dictionary<string,ParseAction>(){
                {"RPAREN",new ParseAction(PAction.REDUCE, 3, "pdecl", 16)},
                {"COMMA",new ParseAction(PAction.REDUCE, 3, "pdecl", 16)},
        },
        // DFA STATE 47
        // pdecls :: pdecl COMMA • pdecls ║ RPAREN
        // pdecls :: • pdecl ║ RPAREN
        // pdecls :: • pdecl COMMA pdecls ║ RPAREN
        // pdecl :: • ID COLON TYPE ║ RPAREN COMMA
        new Dictionary<string,ParseAction>(){
                {"pdecls" , new ParseAction(PAction.SHIFT, 48, null, -1)},
                {"pdecl" , new ParseAction(PAction.SHIFT, 43, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 44, null, -1)},
        },
        // DFA STATE 48
        // pdecls :: pdecl COMMA pdecls • ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"RPAREN",new ParseAction(PAction.REDUCE, 3, "pdecls", 15)},
        },
        // DFA STATE 49
        // funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN • optionalReturn LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        // optionalReturn :: • ║ LBRACE
        // optionalReturn :: • COLON TYPE ║ LBRACE
        new Dictionary<string,ParseAction>(){
                {"optionalReturn" , new ParseAction(PAction.SHIFT, 50, null, -1)},
                {"COLON" , new ParseAction(PAction.SHIFT, 51, null, -1)},
                {"LBRACE",new ParseAction(PAction.REDUCE, 0, "optionalReturn", 8)},
        },
        // DFA STATE 50
        // funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn • LBRACE stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"LBRACE" , new ParseAction(PAction.SHIFT, 53, null, -1)},
        },
        // DFA STATE 51
        // optionalReturn :: COLON • TYPE ║ LBRACE
        new Dictionary<string,ParseAction>(){
                {"TYPE" , new ParseAction(PAction.SHIFT, 52, null, -1)},
        },
        // DFA STATE 52
        // optionalReturn :: COLON TYPE • ║ LBRACE
        new Dictionary<string,ParseAction>(){
                {"LBRACE",new ParseAction(PAction.REDUCE, 2, "optionalReturn", 9)},
        },
        // DFA STATE 53
        // funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE • stmts RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        // stmts :: • stmt SEMI stmts ║ RBRACE
        // stmts :: • SEMI ║ RBRACE
        // stmts :: • ║ RBRACE
        // stmt :: • assign ║ SEMI
        // stmt :: • cond ║ SEMI
        // stmt :: • loop ║ SEMI
        // stmt :: • vardecl ║ SEMI
        // stmt :: • return ║ SEMI
        // assign :: • expr EQ expr ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ELSE braceblock ║ SEMI
        // loop :: • WHILE LPAREN expr RPAREN braceblock ║ SEMI
        // loop :: • REPEAT braceblock UNTIL LPAREN expr RPAREN ║ SEMI
        // vardecl :: • VAR ID COLON TYPE ║ SEMI
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI
        // vardecl :: • VAR ID COLON ID ║ SEMI
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI
        // return :: • RETURN expr ║ SEMI
        // return :: • RETURN ║ SEMI
        // expr :: • NUM ║ EQ
        // expr :: • ID ║ EQ
        // expr :: • NUM ADDOP NUM ║ EQ
        new Dictionary<string,ParseAction>(){
                {"stmts" , new ParseAction(PAction.SHIFT, 54, null, -1)},
                {"stmt" , new ParseAction(PAction.SHIFT, 55, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 56, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 57, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 58, null, -1)},
                {"loop" , new ParseAction(PAction.SHIFT, 59, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 60, null, -1)},
                {"return" , new ParseAction(PAction.SHIFT, 61, null, -1)},
                {"expr" , new ParseAction(PAction.SHIFT, 62, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 63, null, -1)},
                {"WHILE" , new ParseAction(PAction.SHIFT, 64, null, -1)},
                {"REPEAT" , new ParseAction(PAction.SHIFT, 65, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"RETURN" , new ParseAction(PAction.SHIFT, 66, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "stmts", 26)},
        },
        // DFA STATE 54
        // funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts • RBRACE ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE" , new ParseAction(PAction.SHIFT, 90, null, -1)},
        },
        // DFA STATE 55
        // stmts :: stmt • SEMI stmts ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"SEMI" , new ParseAction(PAction.SHIFT, 88, null, -1)},
        },
        // DFA STATE 56
        // stmts :: SEMI • ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE",new ParseAction(PAction.REDUCE, 1, "stmts", 25)},
        },
        // DFA STATE 57
        // stmt :: assign • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "stmt", 27)},
        },
        // DFA STATE 58
        // stmt :: cond • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "stmt", 28)},
        },
        // DFA STATE 59
        // stmt :: loop • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "stmt", 29)},
        },
        // DFA STATE 60
        // stmt :: vardecl • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "stmt", 30)},
        },
        // DFA STATE 61
        // stmt :: return • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "stmt", 31)},
        },
        // DFA STATE 62
        // assign :: expr • EQ expr ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"EQ" , new ParseAction(PAction.SHIFT, 86, null, -1)},
        },
        // DFA STATE 63
        // cond :: IF • LPAREN expr RPAREN braceblock ║ SEMI
        // cond :: IF • LPAREN expr RPAREN braceblock ELSE braceblock ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"LPAREN" , new ParseAction(PAction.SHIFT, 80, null, -1)},
        },
        // DFA STATE 64
        // loop :: WHILE • LPAREN expr RPAREN braceblock ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"LPAREN" , new ParseAction(PAction.SHIFT, 76, null, -1)},
        },
        // DFA STATE 65
        // loop :: REPEAT • braceblock UNTIL LPAREN expr RPAREN ║ SEMI
        // braceblock :: • LBRACE stmts RBRACE ║ UNTIL
        new Dictionary<string,ParseAction>(){
                {"braceblock" , new ParseAction(PAction.SHIFT, 68, null, -1)},
                {"LBRACE" , new ParseAction(PAction.SHIFT, 69, null, -1)},
        },
        // DFA STATE 66
        // return :: RETURN • expr ║ SEMI
        // return :: RETURN • ║ SEMI
        // expr :: • NUM ║ SEMI
        // expr :: • ID ║ SEMI
        // expr :: • NUM ADDOP NUM ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 67, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
                {"SEMI",new ParseAction(PAction.REDUCE, 1, "return", 38)},
        },
        // DFA STATE 67
        // return :: RETURN expr • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 2, "return", 37)},
        },
        // DFA STATE 68
        // loop :: REPEAT braceblock • UNTIL LPAREN expr RPAREN ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"UNTIL" , new ParseAction(PAction.SHIFT, 72, null, -1)},
        },
        // DFA STATE 69
        // braceblock :: LBRACE • stmts RBRACE ║ UNTIL SEMI ELSE
        // stmts :: • stmt SEMI stmts ║ RBRACE
        // stmts :: • SEMI ║ RBRACE
        // stmts :: • ║ RBRACE
        // stmt :: • assign ║ SEMI
        // stmt :: • cond ║ SEMI
        // stmt :: • loop ║ SEMI
        // stmt :: • vardecl ║ SEMI
        // stmt :: • return ║ SEMI
        // assign :: • expr EQ expr ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ELSE braceblock ║ SEMI
        // loop :: • WHILE LPAREN expr RPAREN braceblock ║ SEMI
        // loop :: • REPEAT braceblock UNTIL LPAREN expr RPAREN ║ SEMI
        // vardecl :: • VAR ID COLON TYPE ║ SEMI
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI
        // vardecl :: • VAR ID COLON ID ║ SEMI
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI
        // return :: • RETURN expr ║ SEMI
        // return :: • RETURN ║ SEMI
        // expr :: • NUM ║ EQ
        // expr :: • ID ║ EQ
        // expr :: • NUM ADDOP NUM ║ EQ
        new Dictionary<string,ParseAction>(){
                {"stmts" , new ParseAction(PAction.SHIFT, 70, null, -1)},
                {"stmt" , new ParseAction(PAction.SHIFT, 55, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 56, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 57, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 58, null, -1)},
                {"loop" , new ParseAction(PAction.SHIFT, 59, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 60, null, -1)},
                {"return" , new ParseAction(PAction.SHIFT, 61, null, -1)},
                {"expr" , new ParseAction(PAction.SHIFT, 62, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 63, null, -1)},
                {"WHILE" , new ParseAction(PAction.SHIFT, 64, null, -1)},
                {"REPEAT" , new ParseAction(PAction.SHIFT, 65, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"RETURN" , new ParseAction(PAction.SHIFT, 66, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "stmts", 26)},
        },
        // DFA STATE 70
        // braceblock :: LBRACE stmts • RBRACE ║ UNTIL SEMI ELSE
        new Dictionary<string,ParseAction>(){
                {"RBRACE" , new ParseAction(PAction.SHIFT, 71, null, -1)},
        },
        // DFA STATE 71
        // braceblock :: LBRACE stmts RBRACE • ║ UNTIL SEMI ELSE
        new Dictionary<string,ParseAction>(){
                {"UNTIL",new ParseAction(PAction.REDUCE, 3, "braceblock", 7)},
                {"SEMI",new ParseAction(PAction.REDUCE, 3, "braceblock", 7)},
                {"ELSE",new ParseAction(PAction.REDUCE, 3, "braceblock", 7)},
        },
        // DFA STATE 72
        // loop :: REPEAT braceblock UNTIL • LPAREN expr RPAREN ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"LPAREN" , new ParseAction(PAction.SHIFT, 73, null, -1)},
        },
        // DFA STATE 73
        // loop :: REPEAT braceblock UNTIL LPAREN • expr RPAREN ║ SEMI
        // expr :: • NUM ║ RPAREN
        // expr :: • ID ║ RPAREN
        // expr :: • NUM ADDOP NUM ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 74, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 74
        // loop :: REPEAT braceblock UNTIL LPAREN expr • RPAREN ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"RPAREN" , new ParseAction(PAction.SHIFT, 75, null, -1)},
        },
        // DFA STATE 75
        // loop :: REPEAT braceblock UNTIL LPAREN expr RPAREN • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 6, "loop", 36)},
        },
        // DFA STATE 76
        // loop :: WHILE LPAREN • expr RPAREN braceblock ║ SEMI
        // expr :: • NUM ║ RPAREN
        // expr :: • ID ║ RPAREN
        // expr :: • NUM ADDOP NUM ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 77, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 77
        // loop :: WHILE LPAREN expr • RPAREN braceblock ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"RPAREN" , new ParseAction(PAction.SHIFT, 78, null, -1)},
        },
        // DFA STATE 78
        // loop :: WHILE LPAREN expr RPAREN • braceblock ║ SEMI
        // braceblock :: • LBRACE stmts RBRACE ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"braceblock" , new ParseAction(PAction.SHIFT, 79, null, -1)},
                {"LBRACE" , new ParseAction(PAction.SHIFT, 69, null, -1)},
        },
        // DFA STATE 79
        // loop :: WHILE LPAREN expr RPAREN braceblock • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 5, "loop", 35)},
        },
        // DFA STATE 80
        // cond :: IF LPAREN • expr RPAREN braceblock ║ SEMI
        // cond :: IF LPAREN • expr RPAREN braceblock ELSE braceblock ║ SEMI
        // expr :: • NUM ║ RPAREN
        // expr :: • ID ║ RPAREN
        // expr :: • NUM ADDOP NUM ║ RPAREN
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 81, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 81
        // cond :: IF LPAREN expr • RPAREN braceblock ║ SEMI
        // cond :: IF LPAREN expr • RPAREN braceblock ELSE braceblock ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"RPAREN" , new ParseAction(PAction.SHIFT, 82, null, -1)},
        },
        // DFA STATE 82
        // cond :: IF LPAREN expr RPAREN • braceblock ║ SEMI
        // cond :: IF LPAREN expr RPAREN • braceblock ELSE braceblock ║ SEMI
        // braceblock :: • LBRACE stmts RBRACE ║ SEMI ELSE
        new Dictionary<string,ParseAction>(){
                {"braceblock" , new ParseAction(PAction.SHIFT, 83, null, -1)},
                {"LBRACE" , new ParseAction(PAction.SHIFT, 69, null, -1)},
        },
        // DFA STATE 83
        // cond :: IF LPAREN expr RPAREN braceblock • ║ SEMI
        // cond :: IF LPAREN expr RPAREN braceblock • ELSE braceblock ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"ELSE" , new ParseAction(PAction.SHIFT, 84, null, -1)},
                {"SEMI",new ParseAction(PAction.REDUCE, 5, "cond", 33)},
        },
        // DFA STATE 84
        // cond :: IF LPAREN expr RPAREN braceblock ELSE • braceblock ║ SEMI
        // braceblock :: • LBRACE stmts RBRACE ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"braceblock" , new ParseAction(PAction.SHIFT, 85, null, -1)},
                {"LBRACE" , new ParseAction(PAction.SHIFT, 69, null, -1)},
        },
        // DFA STATE 85
        // cond :: IF LPAREN expr RPAREN braceblock ELSE braceblock • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 7, "cond", 34)},
        },
        // DFA STATE 86
        // assign :: expr EQ • expr ║ SEMI
        // expr :: • NUM ║ SEMI
        // expr :: • ID ║ SEMI
        // expr :: • NUM ADDOP NUM ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"expr" , new ParseAction(PAction.SHIFT, 87, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
        },
        // DFA STATE 87
        // assign :: expr EQ expr • ║ SEMI
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 3, "assign", 32)},
        },
        // DFA STATE 88
        // stmts :: stmt SEMI • stmts ║ RBRACE
        // stmts :: • stmt SEMI stmts ║ RBRACE
        // stmts :: • SEMI ║ RBRACE
        // stmts :: • ║ RBRACE
        // stmt :: • assign ║ SEMI
        // stmt :: • cond ║ SEMI
        // stmt :: • loop ║ SEMI
        // stmt :: • vardecl ║ SEMI
        // stmt :: • return ║ SEMI
        // assign :: • expr EQ expr ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ║ SEMI
        // cond :: • IF LPAREN expr RPAREN braceblock ELSE braceblock ║ SEMI
        // loop :: • WHILE LPAREN expr RPAREN braceblock ║ SEMI
        // loop :: • REPEAT braceblock UNTIL LPAREN expr RPAREN ║ SEMI
        // vardecl :: • VAR ID COLON TYPE ║ SEMI
        // vardecl :: • VAR ID COLON TYPE EQ expr ║ SEMI
        // vardecl :: • VAR ID COLON ID ║ SEMI
        // vardecl :: • VAR ID COLON ID EQ expr ║ SEMI
        // return :: • RETURN expr ║ SEMI
        // return :: • RETURN ║ SEMI
        // expr :: • NUM ║ EQ
        // expr :: • ID ║ EQ
        // expr :: • NUM ADDOP NUM ║ EQ
        new Dictionary<string,ParseAction>(){
                {"stmts" , new ParseAction(PAction.SHIFT, 89, null, -1)},
                {"stmt" , new ParseAction(PAction.SHIFT, 55, null, -1)},
                {"SEMI" , new ParseAction(PAction.SHIFT, 56, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 57, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 58, null, -1)},
                {"loop" , new ParseAction(PAction.SHIFT, 59, null, -1)},
                {"vardecl" , new ParseAction(PAction.SHIFT, 60, null, -1)},
                {"return" , new ParseAction(PAction.SHIFT, 61, null, -1)},
                {"expr" , new ParseAction(PAction.SHIFT, 62, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 63, null, -1)},
                {"WHILE" , new ParseAction(PAction.SHIFT, 64, null, -1)},
                {"REPEAT" , new ParseAction(PAction.SHIFT, 65, null, -1)},
                {"VAR" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"RETURN" , new ParseAction(PAction.SHIFT, 66, null, -1)},
                {"NUM" , new ParseAction(PAction.SHIFT, 16, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 17, null, -1)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 0, "stmts", 26)},
        },
        // DFA STATE 89
        // stmts :: stmt SEMI stmts • ║ RBRACE
        new Dictionary<string,ParseAction>(){
                {"RBRACE",new ParseAction(PAction.REDUCE, 3, "stmts", 24)},
        },
        // DFA STATE 90
        // funcdecl :: FUNC ID LPAREN optionalPdecls RPAREN optionalReturn LBRACE stmts RBRACE • ║ SEMI FUNC CLASS VAR $ RBRACE
        new Dictionary<string,ParseAction>(){
                {"SEMI",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
                {"FUNC",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
                {"CLASS",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
                {"VAR",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
                {"$",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
                {"RBRACE",new ParseAction(PAction.REDUCE, 9, "funcdecl", 6)},
        },
        // DFA STATE 91
        // decls :: SEMI decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 2, "decls", 4)},
        },
        // DFA STATE 92
        // decls :: vardecl decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 2, "decls", 3)},
        },
        // DFA STATE 93
        // decls :: classdecl decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 2, "decls", 2)},
        },
        // DFA STATE 94
        // decls :: funcdecl decls • ║ $
        new Dictionary<string,ParseAction>(){
                {"$",new ParseAction(PAction.REDUCE, 2, "decls", 1)},
        },
    }; //close the table initializer
} //close the ParseTable class
} //close the namespace lab thing
