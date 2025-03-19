
namespace lab{

public class Productions{
    public static void makeThem(){
       

        Grammar.defineProductions( new PSpec[] {
            new("S :: cond | assign"),
            new("assign :: ID EQ NUM"),
            new("cond :: IF ID S | IF ID S ELSE S"),
        });
        
    }
}

}