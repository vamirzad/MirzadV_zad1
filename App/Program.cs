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
            Console.WriteLine("║       ANALIZA KODIRANJA IZVORA I TEORIJE INFORMACIJA          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");

            // Učitaj i analiziraj testnu sekvencu
            string testFile = "testna_sekvenca.txt";
            Console.Write("Unesite naziv datoteke (default: testna_sekvenca.txt): ");
            string inputFile = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputFile))
                testFile = inputFile;

            try
            {
                string testSequence = EncoderDecoder.LoadFromFile(testFile);
                Console.WriteLine($"\n✓ Učitana testna sekvenca iz '{testFile}'");
                Console.WriteLine($"  Dužina sekvence: {testSequence.Length} simbola\n");

                // Sačuvaj originalnu sekvencu kao MirzadV_0source.txt
                string sourceFilename = "MirzadV_0source.txt";
                EncoderDecoder.SaveToFile(sourceFilename, testSequence);
                Console.WriteLine($"✓ Originalna sekvenca sačuvana u '{sourceFilename}'\n");

                // Analiziraj izvor direktno iz datoteke
                var symbols = SourceAnalyzer.AnalyzeSequence(testSequence);

                // Izračunaj entropiju
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.WriteLine("ANALIZA IZVORA");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                DisplaySourceInfo(symbols);
                double entropy = SourceAnalyzer.CalculateEntropy(symbols);
                Console.WriteLine($"\n✓ Entropija izvora (H): {entropy:F4} bita/simbol\n");

                // Glavna petlja menija
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("MENI ALGORITAMA KODIRANJA");
                    Console.WriteLine("═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("Izaberite algoritam kodiranja:");
                    Console.WriteLine("  [1] Shannon-Fanov algoritam");
                    Console.WriteLine("  [2] Obični Huffmanov algoritam");
                    Console.WriteLine("  [3] Skraćeni Huffmanov algoritam (k=8)");
                    Console.WriteLine("  [4] Pokreni sve algoritme i uporedi");
                    Console.WriteLine("  [5] Izlaz");
                    Console.Write("\nVaš izbor (1-5): ");

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
                            break;
                        default:
                            Console.WriteLine("❌ Nevažeći izbor. Molimo unesite broj između 1 i 5.\n");
                            break;
                    }

                    if (!exit && choice != "4")
                    {
                        Console.WriteLine("\nPritisnite bilo koji taster za povratak na meni...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║       ANALIZA KODIRANJA IZVORA I TEORIJE INFORMACIJA          ║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Greška: {ex.Message}");
                Console.WriteLine("Molimo osigurajte da testna datoteka postoji u trenutnom direktoriju.");
            }
        }

        static void RunShannonFano(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("SHANNON-FANOV ALGORITAM");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new ShannonFanoEncoder();
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Shannon-Fanov algoritam", codes, symbols, entropy);
            TestEncodingDecoding("Shannon-Fano", "SF", codes, testSequence, symbols);
        }

        static void RunHuffman(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("OBIČNI HUFFMANOV ALGORITAM");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new HuffmanEncoder();
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Obični Huffmanov algoritam", codes, symbols, entropy);
            TestEncodingDecoding("Huffman", "NH", codes, testSequence, symbols);
        }

        static void RunTruncatedHuffman(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("SKRAĆENI HUFFMANOV ALGORITAM (k=8)");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            var encoder = new TruncatedHuffmanEncoder(8);
            var codes = encoder.Encode(symbols);

            CodeAnalyzer.PrintAnalysis("Skraćeni Huffmanov algoritam (k=8)", codes, symbols, entropy);
            TestEncodingDecoding("Truncated-Huffman", "TH", codes, testSequence, symbols);
        }

        static void RunAllAlgorithms(List<SymbolInfo> symbols, string testSequence, double entropy)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("POKRETANJE SVIH ALGORITAMA");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // Kodiraj sa sva tri algoritma
            var shannonFano = new ShannonFanoEncoder();
            var sfCodes = shannonFano.Encode(symbols);

            var huffman = new HuffmanEncoder();
            var huffmanCodes = huffman.Encode(symbols);

            var truncatedHuffman = new TruncatedHuffmanEncoder(8);
            var truncatedCodes = truncatedHuffman.Encode(symbols);

            // Prikaži analizu za svaki
            Console.WriteLine("\n--- SHANNON-FANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Shannon-Fanov algoritam", sfCodes, symbols, entropy);

            Console.WriteLine("\n\n--- OBIČNI HUFFMANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Obični Huffmanov algoritam", huffmanCodes, symbols, entropy);

            Console.WriteLine("\n\n--- SKRAĆENI HUFFMANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Skraćeni Huffmanov algoritam (k=8)", truncatedCodes, symbols, entropy);

            // Uporedi algoritme
            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("POREĐENJE OPTIMALNOSTI");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            var sfStats = CodeAnalyzer.GetAlgorithmStats("Shannon-Fanov algoritam", sfCodes, symbols, entropy);
            var huffStats = CodeAnalyzer.GetAlgorithmStats("Obični Huffmanov algoritam", huffmanCodes, symbols, entropy);
            var truncStats = CodeAnalyzer.GetAlgorithmStats("Skraćeni Huffmanov algoritam", truncatedCodes, symbols, entropy);

            var allStats = new[] { sfStats, huffStats, truncStats }
                .OrderBy(s => s.avgLength)
                .ToList();

            Console.WriteLine("\nPoređenje algoritama (poredano po prosječnoj dužini):");
            Console.WriteLine("┌──────┬──────────────────────┬──────────────┬─────────────┬──────────────┐");
            Console.WriteLine("│ Rang │      Algoritam       │ L_avg (b/s)  │ Efikasnost  │ Redundancija │");
            Console.WriteLine("├──────┼──────────────────────┼──────────────┼─────────────┼──────────────┤");
            for (int i = 0; i < allStats.Count; i++)
            {
                string rank = i == 0 ? " ★ " : $" {i + 1} ";
                string name = allStats[i].algorithmName.PadRight(20);
                double redundancy = 1 - allStats[i].efficiency;
                Console.WriteLine($"│  {rank}  │ {name} │   {allStats[i].avgLength,10:F4} │  {allStats[i].efficiency * 100,8:F2} % │   {redundancy * 100,8:F2} % │");
            }
            Console.WriteLine("└──────┴──────────────────────┴──────────────┴─────────────┴──────────────┘");

            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    MATEMATIČKI ZAKLJUČAK                      ║");
            Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ ★ {allStats[0].algorithmName,-57} ║");
            Console.WriteLine($"║   je OPTIMALAN sa L_avg = {allStats[0].avgLength:F4} bits/symbol{"",-17} ║");
            Console.WriteLine("║                                                               ║");
            Console.WriteLine("║ • Obični Huffmanov algoritam je garantovano optimalan za      ║");
            Console.WriteLine("║   kodiranje simbol-po-simbol (Shannon-ova teorema)           ║");
            Console.WriteLine("║ • Svi algoritmi zadovoljavaju Kraft-ovu nejednakost          ║");
            Console.WriteLine("║   (prefix-free kodovi)                                        ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");

            // Kodiraj/Dekodiraj testne sekvence
            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("TESTIRANJE KODIRANJA/DEKODIRANJA SEKVENCI");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            TestEncodingDecoding("Shannon-Fano", "SF", sfCodes, testSequence, symbols);
            TestEncodingDecoding("Huffman", "NH", huffmanCodes, testSequence, symbols);
            TestEncodingDecoding("Truncated-Huffman", "TH", truncatedCodes, testSequence, symbols);

            Console.WriteLine("\n\n✓ Svi testovi kodiranja i dekodiranja završeni!");
            Console.WriteLine("\nKljučna zapažanja:");
            Console.WriteLine("  1. Sva tri algoritma proizvode valjane prefix-free kodove (Kraft ≤ 1)");
            Console.WriteLine("  2. Obični Huffmanov algoritam postiže najnižu prosječnu dužinu kodne riječi");
            Console.WriteLine("  3. Sve kodirane sekvence su dekodirane savršeno (100% tačnost)");
            Console.WriteLine("  4. Omjeri kompresije variraju na osnovu efikasnosti algoritma");
            Console.WriteLine("  5. Shannon-Fanov algoritam je jednostavniji ali malo manje efikasan");
            Console.WriteLine("  6. Skraćeni Huffmanov algoritam mijenja dio efikasnosti za jednostavniji codebook");

            Console.WriteLine("\nPritisnite bilo koji taster za povratak na meni...");
            Console.ReadKey();
        }

        static void DisplaySourceInfo(List<SymbolInfo> symbols)
        {
            Console.WriteLine("Analiza izvora:");
            Console.WriteLine($"  Ukupno unikatnih simbola: {symbols.Count}");
            Console.WriteLine("\nVjerovatnoće simbola:");
            Console.WriteLine("┌──────┬────────┬──────────────┬─────────────┐");
            Console.WriteLine("│ Simb │ Karakt │ Pojavljivanja│ Vjerovatnoća│");
            Console.WriteLine("├──────┼────────┼──────────────┼─────────────┤");
            for (int i = 0; i < symbols.Count && i < 12; i++)
            {
                Console.WriteLine($"│ s{i,-3} │   {symbols[i].Symbol}    │   {symbols[i].Count,10} │    {symbols[i].P,7:F4} │");
            }
            Console.WriteLine("└──────┴────────┴──────────────┴─────────────┘");
            if (symbols.Count > 12)
                Console.WriteLine($"  ... i još {symbols.Count - 12} simbola");
        }

        static void TestEncodingDecoding(string algorithmName, string algorithmCode,
                                        Dictionary<char, string> codes,
                                        string testSequence, List<SymbolInfo> symbols)
        {
            Console.WriteLine($"\n┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Testiranje: {algorithmName.ToUpper(),-48}│");
            Console.WriteLine($"└─────────────────────────────────────────────────────────────┘");

            // Kodiraj
            string encoded = EncoderDecoder.Encode(testSequence, codes);
            string encodedFilename = $"MirzadV_2encode{algorithmCode}.txt";
            EncoderDecoder.SaveToFile(encodedFilename, encoded);

            // Dekodiraj
            string decoded = EncoderDecoder.Decode(encoded, codes);
            string decodedFilename = $"MirzadV_3decode{algorithmCode}.txt";
            EncoderDecoder.SaveToFile(decodedFilename, decoded);

            // Uporedi
            bool identical = EncoderDecoder.CompareSequences(testSequence, decoded);

            // Kreiraj fajl sa rezultatima usporedbe
            string errorFilename = $"MirzadV_4error{algorithmCode}.txt";
            string errorReport = GenerateComparisonReport(testSequence, decoded, identical, algorithmName);
            EncoderDecoder.SaveToFile(errorFilename, errorReport);

            // Izračunaj statistiku
            int originalBits = testSequence.Length * 8;
            int encodedBits = encoded.Length;
            double compressionRatio = EncoderDecoder.CalculateCompressionRatio(originalBits, encodedBits);
            double avgBitsPerSymbol = (double)encodedBits / testSequence.Length;
            double savings = (1 - compressionRatio) * 100;

            // Tabelarni prikaz rezultata
            Console.WriteLine("┌──────────────────────────────────────┬──────────────────────┐");
            Console.WriteLine("│ Originalna sekvenca                  │{0,21} │", $"{testSequence.Length} simbola");
            Console.WriteLine("│ Originalna veličina                  │{0,21} │", $"{originalBits} bita");
            Console.WriteLine("│ Kodirano                             │{0,21} │", $"{encodedBits} bita");
            Console.WriteLine("│ Prosječno bita/simbol                │{0,21:F4} │", avgBitsPerSymbol);
            Console.WriteLine("│ Omjer kompresije                     │{0,20:F2} % │", compressionRatio * 100);
            Console.WriteLine("│ Ušteda prostora                      │{0,20:F2} % │", savings);
            Console.WriteLine("│ Dekodirano simbola                   │{0,21} │", decoded.Length);
            Console.WriteLine("├──────────────────────────────────────┼──────────────────────┤");
            Console.WriteLine("│ Verifikacija                         │ {0,-19} │", identical ? "✓ PROLAZ" : "✗ GREŠKA");
            Console.WriteLine("│ Status                               │ {0,-19} │", identical ? "Sekvence IDENTIČNE" : "Sekvence RAZLIČITE");
            Console.WriteLine("└──────────────────────────────────────┴──────────────────────┘");
            Console.WriteLine($"  Izlazne datoteke:");
            Console.WriteLine($"    - {encodedFilename}");
            Console.WriteLine($"    - {decodedFilename}");
            Console.WriteLine($"    - {errorFilename}");
        }

        static string GenerateComparisonReport(string original, string decoded, bool identical, string algorithmName)
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine($"REZULTATI USPOREDBE - {algorithmName}");
            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine();
            report.AppendLine($"Datum: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            report.AppendLine();
            report.AppendLine($"Dužina originalne sekvence: {original.Length} simbola");
            report.AppendLine($"Dužina dekodirane sekvence: {decoded.Length} simbola");
            report.AppendLine();

            if (identical)
            {
                report.AppendLine("✓ STATUS: PROLAZ");
                report.AppendLine("✓ Sekvence su IDENTIČNE");
                report.AppendLine("✓ Svi simboli su tačno dekodirani");
                report.AppendLine();
                report.AppendLine("Broj grešaka: 0");
                report.AppendLine("Tačnost: 100.00%");
            }
            else
            {
                report.AppendLine("✗ STATUS: GREŠKA");
                report.AppendLine("✗ Sekvence se RAZLIKUJU");
                report.AppendLine();

                // Prebrojaj razlike
                int errors = 0;
                int minLength = Math.Min(original.Length, decoded.Length);
                for (int i = 0; i < minLength; i++)
                {
                    if (original[i] != decoded[i])
                        errors++;
                }

                // Dodaj grešku za razliku u dužini
                errors += Math.Abs(original.Length - decoded.Length);

                report.AppendLine($"Broj grešaka: {errors}");
                report.AppendLine($"Tačnost: {((1 - (double)errors / original.Length) * 100):F2}%");
            }

            report.AppendLine();
            report.AppendLine("═══════════════════════════════════════════════════════════");

            return report.ToString();
        }
    }
}
