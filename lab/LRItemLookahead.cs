namespace lab {

public class LRItemWithLookahead {
    public LRItem item;
    public HashSet<string> lookaheads;
    
    public LRItemWithLookahead(LRItem item, HashSet<string> lookaheads) {
        this.item = item;
        this.lookaheads = lookaheads ?? new HashSet<string>();
    }
    
    // Properties to make it easier to work with the underlying item
    public bool dposAtEnd => this.item.dposAtEnd();
    public string symbolAfterDistinguishedPosition => 
        this.dposAtEnd ? null : this.item.symbolAfterDistinguishedPosition;
    
    public override int GetHashCode() {
        int hash = item.GetHashCode();
        // We don't include lookaheads in hash code for better performance
        return hash;
    }
    
    public override bool Equals(object obj) {
        if (Object.ReferenceEquals(obj, null))
            return false;
        
        LRItemWithLookahead other = obj as LRItemWithLookahead;
        if (Object.ReferenceEquals(other, null))
            return false;
        
        // Items must be equal
        if (!this.item.Equals(other.item))
            return false;
        
        // Lookaheads must be the same
        if (this.lookaheads.Count != other.lookaheads.Count)
            return false;
        
        foreach (var lookahead in this.lookaheads) {
            if (!other.lookaheads.Contains(lookahead))
                return false;
        }
        
        return true;
    }
    
    public static bool operator==(LRItemWithLookahead o1, LRItemWithLookahead o2) {
        if (Object.ReferenceEquals(o1, null)) {
            return Object.ReferenceEquals(o2, null);
        }
        return o1.Equals(o2);
    }
    
    public static bool operator!=(LRItemWithLookahead o1, LRItemWithLookahead o2) {
        return !(o1 == o2);
    }
    
    public override string ToString() {
        string itemStr = this.item.ToString();
        string lookaheadStr = String.Join(" ", this.lookaheads.OrderBy(s => s));
        return $"{itemStr} \u2551 {lookaheadStr}"; // Using the double vertical line separator
    }
}

} // namespace lab