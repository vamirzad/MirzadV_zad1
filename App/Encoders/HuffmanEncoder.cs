using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;

namespace SourceCoding.Encoders
{
    public class HuffmanEncoder
    {
        public Dictionary<char, string> Encode(List<SymbolInfo> symbols)
        {
            // Build priority queue with initial nodes
            var pq = new List<Node>();
            foreach (var s in symbols)
            {
                pq.Add(new Node { Freq = s.P, Symbol = s.Symbol });
            }

            // Build Huffman tree
            while (pq.Count > 1)
            {
                // Sort by frequency
                pq = pq.OrderBy(n => n.Freq).ToList();

                // Take two nodes with smallest frequencies
                var left = pq[0];
                var right = pq[1];
                pq.RemoveAt(0);
                pq.RemoveAt(0);

                // Create parent node
                var parent = new Node
                {
                    Freq = left.Freq + right.Freq,
                    Left = left,
                    Right = right
                };
                pq.Add(parent);
            }

            // Generate codes from tree
            var codes = new Dictionary<char, string>();
            var root = pq[0];
            AssignCodes(root, "", codes);
            return codes;
        }

        private void AssignCodes(Node node, string code, Dictionary<char, string> codes)
        {
            if (node.Symbol != '\0')
            {
                codes[node.Symbol] = code;
                return;
            }
            if (node.Left != null)
                AssignCodes(node.Left, code + "0", codes);
            if (node.Right != null)
                AssignCodes(node.Right, code + "1", codes);
        }
    }
}
