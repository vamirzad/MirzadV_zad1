namespace SourceCoding.Models
{
    public class SymbolInfo
    {
        public char Symbol { get; set; }
        public int Count { get; set; }
        public double P { get; set; }
        public string Code { get; set; } = "";
    }

    public class Node
    {
        public double Freq { get; set; }
        public char Symbol { get; set; } = '\0';
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }
}
