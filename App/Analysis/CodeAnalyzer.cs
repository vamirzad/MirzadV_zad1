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
            Console.WriteLine($"\n=== {algorithmName} ===");

            // Kraft inequality
            double kraft = CalculateKraftSum(codes);
            Console.WriteLine($"Kraft Sum: Σ2^(-li) = {kraft:F6}");
            Console.WriteLine($"Kraft Inequality: {kraft:F6} ≤ 1 ? {(kraft <= 1 ? "✓ VALID" : "✗ INVALID")}");

            // Average length
            double Lavg = CalculateAverageLength(codes, symbols);
            Console.WriteLine($"Average Codeword Length (L_avg): {Lavg:F4} bits/symbol");

            // Efficiency and redundancy
            double efficiency = CalculateEfficiency(entropy, Lavg);
            double redundancy = CalculateRedundancy(efficiency);
            Console.WriteLine($"Efficiency (η): {efficiency:F4} = {(efficiency * 100):F2}%");
            Console.WriteLine($"Redundancy (ρ): {redundancy:F4} = {(redundancy * 100):F2}%");

            // Show some codewords
            Console.WriteLine("\nCodewords:");
            int i = 0;
            foreach (var symbol in symbols.Take(12))
            {
                if (codes.ContainsKey(symbol.Symbol))
                {
                    Console.WriteLine($"  s{i} ({symbol.Symbol}): {codes[symbol.Symbol]} " +
                                    $"(length: {codes[symbol.Symbol].Length}, P={symbol.P:F2})");
                }
                i++;
            }
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
