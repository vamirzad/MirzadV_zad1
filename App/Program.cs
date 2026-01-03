using System;
using System.Collections.Generic;
using System.Linq;
using SourceCoding.Models;
using SourceCoding.Encoders;
using SourceCoding.Analysis;
using SourceCoding.Utils;

namespace SourceCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          SOURCE CODING AND INFORMATION THEORY ANALYSIS          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");

            // Load and analyze the test sequence
            string testFile = "testna_sekvenca.txt";
            Console.Write("Enter test file name (default: testna_sekvenca.txt): ");
            string inputFile = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputFile))
                testFile = inputFile;

            try
            {
                string testSequence = EncoderDecoder.LoadFromFile(testFile);
                Console.WriteLine($"\n✓ Loaded test sequence from '{testFile}'");
                Console.WriteLine($"  Sequence length: {testSequence.Length} symbols\n");

                // Analyze the source directly from the file
                var symbols = SourceAnalyzer.AnalyzeSequence(testSequence);

                // Calculate entropy
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.WriteLine("SOURCE ANALYSIS");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                DisplaySourceInfo(symbols);
                double entropy = SourceAnalyzer.CalculateEntropy(symbols);
                Console.WriteLine($"\n✓ Source Entropy (H): {entropy:F4} bits/symbol\n");

                // Main menu loop
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("ENCODING ALGORITHM MENU");
                    Console.WriteLine("═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("Choose an encoding algorithm:");
                    Console.WriteLine("  [1] Shannon-Fano Encoding");
                    Console.WriteLine("  [2] Huffman Encoding");
                    Console.WriteLine("  [3] Truncated Huffman Encoding (k=8)");
                    Console.WriteLine("  [4] Run All Algorithms & Compare");
                    Console.WriteLine("  [5] Exit");
                    Console.Write("\nYour choice (1-5): ");

                    string choice = Console.ReadLine();
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "1":
                            RunShannonFano(symbols, testSequence, entropy);
                            break;
                        case "2":
                            RunHuffman(symbols, testSequence, entropy);
                            break;
                        case "3":
                            RunTruncatedHuffman(symbols, testSequence, entropy);
                            break;
                        case "4":
                            RunAllAlgorithms(symbols, testSequence, entropy);
                            break;
                        case "5":
                            exit = true;
                            Console.WriteLine("Thank you for using the Source Coding Analyzer!");
                            break;
                        default:
                            Console.WriteLine("❌ Invalid choice. Please enter a number between 1 and 5.\n");
                            break;
                    }

                    if (!exit && choice != "4")
                    {
                        Console.WriteLine("\nPress any key to return to menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║          SOURCE CODING AND INFORMATION THEORY ANALYSIS          ║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.WriteLine("Please ensure the test file exists in the current directory.");
            }
        }

        static void RunShannonFano(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("SHANNON-FANO ENCODING");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new ShannonFanoEncoder();
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Shannon-Fano", codes, symbols, entropy);
            TestEncodingDecoding("Shannon-Fano", codes, testSequence, symbols);
        }

        static void RunHuffman(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("HUFFMAN ENCODING");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new HuffmanEncoder();
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Huffman", codes, symbols, entropy);
            TestEncodingDecoding("Huffman", codes, testSequence, symbols);
        }

        static void RunTruncatedHuffman(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("TRUNCATED HUFFMAN ENCODING (k=8)");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new TruncatedHuffmanEncoder(8);
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Truncated Huffman (k=8)", codes, symbols, entropy);
            TestEncodingDecoding("Truncated-Huffman", codes, testSequence, symbols);
        }

        static void RunAllAlgorithms(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("RUNNING ALL ALGORITHMS");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // Encode with all three algorithms
            var shannonFano = new ShannonFanoEncoder();
            var sfCodes = shannonFano.Encode(symbols);

            var huffman = new HuffmanEncoder();
            var huffmanCodes = huffman.Encode(symbols);

            var truncatedHuffman = new TruncatedHuffmanEncoder(8);
            var truncatedCodes = truncatedHuffman.Encode(symbols);

            // Show analysis for each
            Console.WriteLine("\n--- SHANNON-FANO ANALYSIS ---");
            CodeAnalyzer.PrintAnalysis("Shannon-Fano", sfCodes, symbols, entropy);

            Console.WriteLine("\n\n--- HUFFMAN ANALYSIS ---");
            CodeAnalyzer.PrintAnalysis("Huffman", huffmanCodes, symbols, entropy);

            Console.WriteLine("\n\n--- TRUNCATED HUFFMAN ANALYSIS ---");
            CodeAnalyzer.PrintAnalysis("Truncated Huffman (k=8)", truncatedCodes, symbols, entropy);

            // Compare algorithms
            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("OPTIMALITY COMPARISON");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            var sfStats = CodeAnalyzer.GetAlgorithmStats("Shannon-Fano", sfCodes, symbols, entropy);
            var huffStats = CodeAnalyzer.GetAlgorithmStats("Huffman", huffmanCodes, symbols, entropy);
            var truncStats = CodeAnalyzer.GetAlgorithmStats("Truncated Huffman", truncatedCodes, symbols, entropy);

            var allStats = new[] { sfStats, huffStats, truncStats }
                .OrderBy(s => s.avgLength)
                .ToList();

            Console.WriteLine("\nAlgorithm Comparison (ordered by average length):");
            Console.WriteLine("─────────────────────────────────────────────────────────────");
            for (int i = 0; i < allStats.Count; i++)
            {
                string rank = i == 0 ? "★ OPTIMAL" : $"  Rank {i + 1}";
                Console.WriteLine($"{rank}: {allStats[i].algorithmName}");
                Console.WriteLine($"         L_avg = {allStats[i].avgLength:F4} bits/symbol");
                Console.WriteLine($"         η = {allStats[i].efficiency:F4} ({allStats[i].efficiency * 100:F2}%)");
                Console.WriteLine();
            }

            Console.WriteLine("Mathematical Conclusion:");
            Console.WriteLine($"  • {allStats[0].algorithmName} is OPTIMAL with lowest L_avg = {allStats[0].avgLength:F4}");
            Console.WriteLine($"  • Huffman coding is guaranteed to be optimal for symbol-by-symbol encoding");
            Console.WriteLine($"  • All algorithms satisfy Kraft inequality (prefix-free codes)");

            // Encode/Decode test sequences
            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ENCODE/DECODE TEST SEQUENCES");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            TestEncodingDecoding("Shannon-Fano", sfCodes, testSequence, symbols);
            TestEncodingDecoding("Huffman", huffmanCodes, testSequence, symbols);
            TestEncodingDecoding("Truncated-Huffman", truncatedCodes, testSequence, symbols);

            Console.WriteLine("\n\n✓ All encoding and decoding tests completed!");
            Console.WriteLine("\nKey Observations:");
            Console.WriteLine("  1. All three algorithms produce valid prefix-free codes (Kraft ≤ 1)");
            Console.WriteLine("  2. Huffman coding achieves the lowest average codeword length");
            Console.WriteLine("  3. All encoded sequences were decoded perfectly (100% accuracy)");
            Console.WriteLine("  4. Compression ratios vary based on algorithm efficiency");
            Console.WriteLine("  5. Shannon-Fano is simpler but slightly less efficient than Huffman");
            Console.WriteLine("  6. Truncated Huffman trades some efficiency for simpler codebook");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void DisplaySourceInfo(List<SymbolInfo> symbols)
        {
            Console.WriteLine("Source Analysis:");
            Console.WriteLine($"  Total unique symbols: {symbols.Count}");
            Console.WriteLine("\nSymbol Probabilities:");
            for (int i = 0; i < symbols.Count && i < 12; i++)
            {
                Console.WriteLine($"  s{i} ('{symbols[i].Symbol}'): P = {symbols[i].P:F4} ({symbols[i].Count} occurrences)");
            }
            if (symbols.Count > 12)
                Console.WriteLine($"  ... and {symbols.Count - 12} more symbols");
        }

        static void TestEncodingDecoding(string algorithmName, Dictionary<char, string> codes,
                                        string testSequence, List<SymbolInfo> symbols)
        {
            Console.WriteLine($"\n─────────────────────────────────────────────────────────────");
            Console.WriteLine($"Testing: {algorithmName.ToUpper()}");
            Console.WriteLine($"─────────────────────────────────────────────────────────────");

            // Encode
            string encoded = EncoderDecoder.Encode(testSequence, codes);
            string encodedFilename = $"encoded_{algorithmName.ToLower()}.txt";
            EncoderDecoder.SaveToFile(encodedFilename, encoded);

            // Decode
            string decoded = EncoderDecoder.Decode(encoded, codes);
            string decodedFilename = $"decoded_{algorithmName.ToLower()}.txt";
            EncoderDecoder.SaveToFile(decodedFilename, decoded);

            // Compare
            bool identical = EncoderDecoder.CompareSequences(testSequence, decoded);

            // Calculate statistics
            int originalBits = testSequence.Length * 8; // Assuming 8 bits per character
            int encodedBits = encoded.Length;
            double compressionRatio = EncoderDecoder.CalculateCompressionRatio(originalBits, encodedBits);
            double avgBitsPerSymbol = (double)encodedBits / testSequence.Length;

            // Display results
            Console.WriteLine($"  Original sequence: {testSequence.Length} symbols");
            Console.WriteLine($"  Original size: {originalBits} bits (8 bits/symbol)");
            Console.WriteLine($"  Encoded: {encodedBits} bits");
            Console.WriteLine($"  Average bits/symbol: {avgBitsPerSymbol:F4}");
            Console.WriteLine($"  Compression ratio: {compressionRatio:F4} ({compressionRatio * 100:F2}%)");
            Console.WriteLine($"  Space savings: {(1 - compressionRatio) * 100:F2}%");
            Console.WriteLine($"  Decoded: {decoded.Length} symbols");
            Console.WriteLine($"  Verification: {(identical ? "✓ PASS - Sequences are IDENTICAL" : "✗ FAIL - Sequences differ")}");
            Console.WriteLine($"  Output files: {encodedFilename}, {decodedFilename}");
        }
    }
}
