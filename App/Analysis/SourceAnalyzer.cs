using System;
using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;

namespace SourceCoding.Analysis
{
    public class SourceAnalyzer
    {
        public static double CalculateEntropy(List<SymbolInfo> symbols)
        {
            double H = 0;
            foreach (var s in symbols)
            {
                if (s.P > 0)
                    H -= s.P * Math.Log(s.P, 2);
            }
            return H;
        }

        public static List<SymbolInfo> AnalyzeSequence(string content)
        {
            // Count occurrences of all symbols
            var counts = new Dictionary<char, int>();

            foreach (char c in content)
            {
                if (counts.ContainsKey(c))
                    counts[c]++;
                else
                    counts[c] = 1;
            }

            // Calculate probabilities and create SymbolInfo list (ordered by probability)
            var symbols = counts
                .Select(kv => new SymbolInfo
                {
                    Symbol = kv.Key,
                    Count = kv.Value,
                    P = (double)kv.Value / content.Length
                })
                .OrderByDescending(s => s.P)
                .ToList();

            return symbols;
        }
    }
}
