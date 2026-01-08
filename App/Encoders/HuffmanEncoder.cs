using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;

namespace SourceCoding.Encoders
{
    public class HuffmanEncoder
    {
        public Dictionary<char, string> Encode(List<SymbolInfo> symbols)
        {
            // Izgradi prioritetni red sa početnim čvorovima
            var pq = new List<Node>();
            foreach (var s in symbols)
            {
                pq.Add(new Node { Freq = s.P, Symbol = s.Symbol });
            }

            // Izgradi Huffman stablo
            while (pq.Count > 1)
            {
                // Sortiraj po učestalosti
                pq = pq.OrderBy(n => n.Freq).ToList();

                // Uzmi dva čvora sa najmanjim učestalostima
                var left = pq[0];
                var right = pq[1];
                pq.RemoveAt(0);
                pq.RemoveAt(0);

                // Kreiraj roditeljski čvor
                var parent = new Node
                {
                    Freq = left.Freq + right.Freq,
                    Left = left,
                    Right = right
                };
                pq.Add(parent);
            }

            // Generiši kodove iz stabla
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
