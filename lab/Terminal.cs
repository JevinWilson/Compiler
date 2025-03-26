using System.Text.RegularExpressions;

namespace lab{

public class Terminal{
    public string sym;
    public Regex rex;

    public Terminal(string sym, string rex){
        this.sym=sym;
        this.rex= new Regex( "\\G(" + rex + ")" );
    }

} //end of class Terminal


} //end of namespace