namespace lab{
public static class ParseTable{
    public static List<Dictionary<string,ParseAction> > table = new() {
        // DFA STATE 0
        // S' :: • S ║ $
        // S :: • cond ║ $
        // S :: • assign ║ $
        // cond :: • IF ID S ║ $
        // cond :: • IF ID S ELSE S ║ $
        // assign :: • ID EQ NUM ║ $
        new Dictionary<string,ParseAction>(){
                {"S" , new ParseAction(PAction.SHIFT, 1, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 2, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 5, null, -1)},
        },
        // DFA STATE 1
        // S' :: S • ║ $
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 1, "S'", 5)},
        },
        // DFA STATE 2
        // S :: cond • ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 1, "S", 0)},
                {"ELSE" , new ParseAction(PAction.REDUCE, 1, "S", 0)},
        },
        // DFA STATE 3
        // S :: assign • ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 1, "S", 1)},
                {"ELSE" , new ParseAction(PAction.REDUCE, 1, "S", 1)},
        },
        // DFA STATE 4
        // cond :: IF • ID S ║ $ ELSE
        // cond :: IF • ID S ELSE S ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"ID" , new ParseAction(PAction.SHIFT, 8, null, -1)},
        },
        // DFA STATE 5
        // assign :: ID • EQ NUM ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"EQ" , new ParseAction(PAction.SHIFT, 6, null, -1)},
        },
        // DFA STATE 6
        // assign :: ID EQ • NUM ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"NUM" , new ParseAction(PAction.SHIFT, 7, null, -1)},
        },
        // DFA STATE 7
        // assign :: ID EQ NUM • ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 3, "assign", 2)},
                {"ELSE" , new ParseAction(PAction.REDUCE, 3, "assign", 2)},
        },
        // DFA STATE 8
        // cond :: IF ID • S ║ $ ELSE
        // cond :: IF ID • S ELSE S ║ $ ELSE
        // S :: • cond ║ $ ELSE
        // S :: • assign ║ $ ELSE
        // cond :: • IF ID S ║ $ ELSE
        // cond :: • IF ID S ELSE S ║ $ ELSE
        // assign :: • ID EQ NUM ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"S" , new ParseAction(PAction.SHIFT, 9, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 2, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 5, null, -1)},
        },
        // DFA STATE 9
        // cond :: IF ID S • ║ $ ELSE
        // cond :: IF ID S • ELSE S ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"ELSE" , new ParseAction(PAction.SHIFT, 10, null, -1)},
                {"$" , new ParseAction(PAction.REDUCE, 3, "cond", 3)},
        },
        // DFA STATE 10
        // cond :: IF ID S ELSE • S ║ $ ELSE
        // S :: • cond ║ $ ELSE
        // S :: • assign ║ $ ELSE
        // cond :: • IF ID S ║ $ ELSE
        // cond :: • IF ID S ELSE S ║ $ ELSE
        // assign :: • ID EQ NUM ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"S" , new ParseAction(PAction.SHIFT, 11, null, -1)},
                {"cond" , new ParseAction(PAction.SHIFT, 2, null, -1)},
                {"assign" , new ParseAction(PAction.SHIFT, 3, null, -1)},
                {"IF" , new ParseAction(PAction.SHIFT, 4, null, -1)},
                {"ID" , new ParseAction(PAction.SHIFT, 5, null, -1)},
        },
        // DFA STATE 11
        // cond :: IF ID S ELSE S • ║ $ ELSE
        new Dictionary<string,ParseAction>(){
                {"$" , new ParseAction(PAction.REDUCE, 5, "cond", 4)},
                {"ELSE" , new ParseAction(PAction.REDUCE, 5, "cond", 4)},
        },
    }; //close the table initializer
} //close the ParseTable class
} //close the namespace lab thing
