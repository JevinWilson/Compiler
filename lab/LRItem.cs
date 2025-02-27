namespace lab {
    public class LRItem {
        public readonly int productionIndex;
        public readonly int dpos;

        public LRItem(int pi, int dpos) {
            this.productionIndex = pi;
            this.dpos = dpos;
        }

        public Production production {
            get {
                return Grammar.productions[this.productionIndex];
            }
        }

        public override string ToString() {
            Production p = this.production;
            string result = $"{p.lhs} ::";
            
            for (int i = 0; i < p.rhs.Length; i++) {
                if (i == dpos) result += " •";
                result += " " + p.rhs[i];
            }
            
            // Handle dot at the end
            if (dpos >= p.rhs.Length) result += " •";
            
            return result;
        }

        public override int GetHashCode() {
            return productionIndex * 31 + dpos;
        }

        public override bool Equals(object obj) {
            if (Object.ReferenceEquals(obj, null))
                return false;
            if (obj.GetType() != typeof(LRItem))
                return false;
                
            LRItem other = (LRItem)obj;
            return productionIndex == other.productionIndex && dpos == other.dpos;
        }

        public static bool operator ==(LRItem o1, object o2) {
            if (Object.ReferenceEquals(o1, null))
                return Object.ReferenceEquals(o2, null);
            return o1.Equals(o2);
        }

        public static bool operator !=(LRItem o1, object o2) {
            return !(o1 == o2);
        }

    } //class LRItem
    
} //namespace