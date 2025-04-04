namespace lab{

    public class OpNeg: Opcode {
        IntRegister rax;
        public OpNeg( IntRegister r1){
            this.rax=r1;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    neg {this.rax}");
        }
    }

}