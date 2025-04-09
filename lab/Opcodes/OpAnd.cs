namespace lab{

    public class OpAnd: Opcode {
        IntRegister op1;
        IntRegister op2;

        public OpAnd( IntRegister op1, IntRegister op2){
            this.op1=op1;
            this.op2 = op2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    and {this.op2}, {this.op1}");
        }
    }

}