namespace lab{

    public class OpOr: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpOr( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    or {this.rbx}, {this.rax}");
        }
    }

}