namespace lab{

    public class OpMul: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpMul( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    mul {this.rbx}");
        }
    }

}