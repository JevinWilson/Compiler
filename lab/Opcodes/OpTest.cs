namespace lab{

    public class OpTest: Opcode {
        IntRegister rax;
        IntRegister rbx;
        public OpTest( IntRegister r1, IntRegister r2){
            this.rax=r1;
            this.rbx=r2;
        }

        public override void output(StreamWriter w){
            w.WriteLine($"    test {this.rbx}, {this.rax}");
            w.WriteLine($"    setz %al");   
        }
    }

}