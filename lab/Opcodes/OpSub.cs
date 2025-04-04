namespace lab{

    public class OpSub: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpSub( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    sub {this.rbx}, {this.rax}");
        }
    }

}