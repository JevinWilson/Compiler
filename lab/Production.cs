namespace lab{

public class Production {
    // foo :: bar baz bam
    public string lhs;      //foo
    public string[] rhs;    //[ bar, baz, bam]
    public PSpec pspec;
    public readonly int index;

    public Production(PSpec pspec, string lhs, string[] rhs, int index){
        this.pspec=pspec;
        this.lhs=lhs;
        this.rhs=rhs;
        this.index=index;

    }
    public override string ToString(){
        string rhsStr;
        if( this.rhs.Length == 0 )
            rhsStr = "\u03bb";      //lambda
        else
            rhsStr = String.Join(' ',this.rhs);
        return $"{this.lhs} :: {rhsStr}";
    }
} //class Production

}//namespace