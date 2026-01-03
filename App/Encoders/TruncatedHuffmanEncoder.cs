using System;
using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;

namespace SourceCoding.Encoders
{
    public class TruncatedHuffmanEncoder
    {
        private readonly int k;

        public TruncatedHuffmanEncoder(int k)
        {
            this.k = k;
        }

        public Dictionary<char, string> Encode(List<SymbolInfo> symbols)
        {
            var codes = new Dictionary<char, string>();

            // Create a modified symbol list for Huffman encoding
            // Top K symbols keep their probabilities
            // Remaining symbols are grouped into one "escape" symbol
            var topK = symbols.Take(k).ToList();
            var remaining = symbols.Skip(k).ToList();

            // Calculate total probability of remaining symbols
            double escapeProb = remaining.Sum(s => s.P);

            // Create a temporary symbol for the escape code
            var escapeSymbol = new SymbolInfo
            {
                Symbol = '\xFF', // Special escape character
                P = escapeProb
            };

            // Add escape symbol to top K for Huffman encoding
            var symbolsForHuffman = topK.ToList();
            symbolsForHuffman.Add(escapeSymbol);

            // Apply Huffman to top K + escape
            var huffmanEncoder = new HuffmanEncoder();
            var huffmanCodes = huffmanEncoder.Encode(symbolsForHuffman);

            // Assign codes to top K symbols
            foreach (var sym in topK)
                codes[sym.Symbol] = huffmanCodes[sym.Symbol];

            // Get the escape code
            string escapeCode = huffmanCodes['\xFF'];

            // Fixed-length encoding for remaining symbols
            int fixedLength = (int)Math.Ceiling(Math.Log(remaining.Count, 2));
            if (fixedLength == 0) fixedLength = 1; // Handle edge case

            int idx = 0;
            foreach (var sym in remaining)
            {
                string fixedCode = Convert.ToString(idx, 2).PadLeft(fixedLength, '0');
                codes[sym.Symbol] = escapeCode + fixedCode;
                idx++;
            }

            return codes;
        }
    }
}
