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

            // Kreiraj modificiranu listu simbola za Huffman kodiranje
            // Top K simbola zadržavaju svoje vjerovatnoće
            // Preostali simboli se grupišu u jedan "escape" simbol
            var topK = symbols.Take(k).ToList();
            var remaining = symbols.Skip(k).ToList();

            // Izračunaj ukupnu vjerovatnoću preostalih simbola
            double escapeProb = remaining.Sum(s => s.P);

            // Kreiraj privremeni simbol za escape kod
            var escapeSymbol = new SymbolInfo
            {
                Symbol = '\xFF', // Poseban escape karakter
                P = escapeProb
            };

            // Dodaj escape simbol top K simbolima za Huffman kodiranje
            var symbolsForHuffman = topK.ToList();
            symbolsForHuffman.Add(escapeSymbol);

            // Primijeni Huffman na top K + escape
            var huffmanEncoder = new HuffmanEncoder();
            var huffmanCodes = huffmanEncoder.Encode(symbolsForHuffman);

            // Dodijeli kodove top K simbolima
            foreach (var sym in topK)
                codes[sym.Symbol] = huffmanCodes[sym.Symbol];

            // Dobavi escape kod
            string escapeCode = huffmanCodes['\xFF'];

            // Fiksna dužina kodiranja za preostale simbole
            int fixedLength = (int)Math.Ceiling(Math.Log(remaining.Count, 2));
            if (fixedLength == 0) fixedLength = 1; // Obradi specijalan slučaj

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
