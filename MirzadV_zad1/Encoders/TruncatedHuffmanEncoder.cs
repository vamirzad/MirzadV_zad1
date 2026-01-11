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

            var topK = symbols.Take(k).ToList();
            var remaining = symbols.Skip(k).ToList();

            double escapeProb = remaining.Sum(s => s.P);

            var escapeSymbol = new SymbolInfo
            {
                Symbol = '\xFF',
                P = escapeProb
            };

            var symbolsForHuffman = topK.ToList();
            symbolsForHuffman.Add(escapeSymbol);

            var huffmanEncoder = new HuffmanEncoder();
            var huffmanCodes = huffmanEncoder.Encode(symbolsForHuffman);

            foreach (var sym in topK)
                codes[sym.Symbol] = huffmanCodes[sym.Symbol];

            string escapeCode = huffmanCodes['\xFF'];

            int fixedLength = (int)Math.Ceiling(Math.Log(remaining.Count, 2));
            if (fixedLength == 0) fixedLength = 1;

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
