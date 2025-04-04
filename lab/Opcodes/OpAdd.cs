namespace lab{

    public class OpAdd: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpAdd( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    add {this.rbx}, {this.rax}");
        }
    }

}