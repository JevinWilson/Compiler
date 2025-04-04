namespace lab{

    public class OpAnd: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpAnd( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    and {this.rbx}, {this.rax}");
        }
    }

}