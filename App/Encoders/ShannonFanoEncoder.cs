using System.Collections.Generic;
using SourceCoding.Models;

namespace SourceCoding.Encoders
{
    public class ShannonFanoEncoder
    {
        public Dictionary<char, string> Encode(List<SymbolInfo> symbols)
        {
            var symbolsCopy = new List<SymbolInfo>();
            foreach (var s in symbols)
            {
                symbolsCopy.Add(new SymbolInfo
                {
                    Symbol = s.Symbol,
                    Count = s.Count,
                    P = s.P,
                    Code = ""
                });
            }

            ShannonFanoRecursive(symbolsCopy, 0, symbolsCopy.Count - 1);

            var codes = new Dictionary<char, string>();
            foreach (var s in symbolsCopy)
                codes[s.Symbol] = s.Code;

            return codes;
        }

        private void ShannonFanoRecursive(List<SymbolInfo> symbols, int start, int end)
        {
            if (start >= end) return;

            double total = 0;
            for (int i = start; i <= end; i++)
                total += symbols[i].P;

            double acc = 0;
            int split = start;
            for (int i = start; i <= end; i++)
            {
                acc += symbols[i].P;
                split = i;
                if (acc >= total / 2) break;
            }

            for (int i = start; i <= split; i++)
                symbols[i].Code += "0";
            for (int i = split + 1; i <= end; i++)
                symbols[i].Code += "1";

            ShannonFanoRecursive(symbols, start, split);
            ShannonFanoRecursive(symbols, split + 1, end);
        }
    }
}
