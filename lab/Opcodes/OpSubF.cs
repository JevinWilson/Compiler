namespace lab{

    public class OpSubF: Opcode {
        FloatRegister rax;
        FloatRegister rbx;
        public OpSubF( FloatRegister r1, FloatRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    subsd {this.rbx}, {this.rax}");
        }
    }

}