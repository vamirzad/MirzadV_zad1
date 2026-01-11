using System;
using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;

namespace SourceCoding.Analysis
{
    public class CodeAnalyzer
    {
        public static double CalculateKraftSum(Dictionary<char, string> codes)
        {
            return codes.Values.Sum(code => Math.Pow(2, -code.Length));
        }

        public static double CalculateAverageLength(Dictionary<char, string> codes, List<SymbolInfo> symbols)
        {
            double Lavg = 0;
            foreach (var symbol in symbols)
            {
                if (codes.ContainsKey(symbol.Symbol))
                    Lavg += symbol.P * codes[symbol.Symbol].Length;
            }
            return Lavg;
        }

        public static double CalculateEfficiency(double entropy, double averageLength)
        {
            return entropy / averageLength;
        }

        public static double CalculateRedundancy(double efficiency)
        {
            return 1 - efficiency;
        }

        public static void PrintAnalysis(string algorithmName, Dictionary<char, string> codes,
                                        List<SymbolInfo> symbols, double entropy)
        {
            Console.WriteLine($"\n=== {algorithmName} ===\n");

            double kraft = CalculateKraftSum(codes);
            double Lavg = CalculateAverageLength(codes, symbols);
            double efficiency = CalculateEfficiency(entropy, Lavg);
            double redundancy = CalculateRedundancy(efficiency);

            Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine("|                   KVALITET KODIRANJA                        |");
            Console.WriteLine("├──────────────────────────────────────┬──────────────────────┤");
            Console.WriteLine($"| Suma Kraft-a (Σ2^-li)               | {kraft,20:F6} |");
            Console.WriteLine($"| Kraft uslov                         | {(kraft <= 1 ? "ZADOVOLJEN" : "NIJE ZADOVOLJEN"),20} |");
            Console.WriteLine($"| Prosječna dužina (L_avg)            | {Lavg,17:F4} b/s |");
            Console.WriteLine($"| Efikasnost kodiranja (η)            | {efficiency * 100,18:F2} % |");
            Console.WriteLine($"| Redundancija koda (ρ)               | {redundancy * 100,18:F2} % |");
            Console.WriteLine("└──────────────────────────────────────┴──────────────────────┘");

            Console.WriteLine("\n┌─────────────┬────────┬────────────────────┬──────┐");
            Console.WriteLine("| Vjerovatnoća| Dužina |    Kodna riječ     | Oznk |");
            Console.WriteLine("├─────────────┼────────┼────────────────────┼──────┤");

            int i = 0;
            foreach (var symbol in symbols.Take(12))
            {
                if (codes.ContainsKey(symbol.Symbol))
                {
                    string codeword = codes[symbol.Symbol].PadRight(18);
                    Console.WriteLine($"|    {symbol.P,7:F4} |   {codes[symbol.Symbol].Length,-4} | {codeword} | s{i,-3} |");
                }
                i++;
            }
            Console.WriteLine("└─────────────┴────────┴────────────────────┴──────┘");
        }

        public static (string algorithmName, double efficiency, double avgLength)
            GetAlgorithmStats(string name, Dictionary<char, string> codes,
                            List<SymbolInfo> symbols, double entropy)
        {
            double Lavg = CalculateAverageLength(codes, symbols);
            double efficiency = CalculateEfficiency(entropy, Lavg);
            return (name, efficiency, Lavg);
        }
    }
}
