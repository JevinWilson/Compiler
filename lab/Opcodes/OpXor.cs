namespace lab{

    public class OpXor: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpXor( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    xor {this.rbx}, {this.rax}");
        }
    }

}