namespace lab{

    public class OpDivF: Opcode {
        FloatRegister numerator;
        FloatRegister denominator;
        public OpDivF( FloatRegister n, FloatRegister d){
            this.numerator=n;
            this.denominator=d;
            if( this.numerator != Register.xmm0 ){
                throw new Exception();  //x86 weirdness
            }
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    divsd {this.denominator}, {this.numerator}");
        }
    }

}