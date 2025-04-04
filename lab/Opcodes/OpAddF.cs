namespace lab{

    public class OpAddF: Opcode {
        FloatRegister op1;
        FloatRegister op2;
        public OpAddF( FloatRegister r1, FloatRegister r2){
            this.op1=r1;
            this.op2=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    addsd {this.op2}, {this.op1}");
        }
    }

}