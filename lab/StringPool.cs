namespace lab{

    public static class StringPool{
        public static Dictionary<string,Label> allStrings = new();

        public static void addString(string theString){
            if( !allStrings.ContainsKey(theString) ){
                allStrings[theString] = new Label( theString );
            }
        }

        public static Label getLabel(string s){
            if( allStrings.ContainsKey(s) ){
                return allStrings[s];
            }
            else
                Environment.Exit(55);
                return null;
        }
        
    }

}