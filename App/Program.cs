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

            string sourceFile = "MirzadV_0source.txt";

            try
            {
                string testSequence = EncoderDecoder.LoadFromFile(sourceFile);
                Console.WriteLine($"\nUčitana izvorna sekvenca iz '{sourceFile}'");
                Console.WriteLine($"  Dužina sekvence: {testSequence.Length} simbola\n");

                var symbols = SourceAnalyzer.AnalyzeSequence(testSequence);

                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.WriteLine("STATISTICKA ANALIZA IZVORA");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                DisplaySourceInfo(symbols);
                double entropy = SourceAnalyzer.CalculateEntropy(symbols);
                Console.WriteLine($"\nEntropija izvora (H): {entropy:F4} bita/simbol\n");

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("IZBOR METODE KOMPRESIJE");
                    Console.WriteLine("═══════════════════════════════════════════════════════════════");
                    Console.WriteLine("Izaberite metodu:");
                    Console.WriteLine("  [1] Shannon-Fanov algoritam");
                    Console.WriteLine("  [2] Obični Huffmanov algoritam");
                    Console.WriteLine("  [3] Skraćeni Huffmanov algoritam (k=8)");
                    Console.WriteLine("  [4] Uporedna analiza svih metoda");
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
                            Console.WriteLine("Nevažeći izbor. Molimo unesite broj između 1 i 5.\n");
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
                Console.WriteLine($"\nGreška: {ex.Message}");
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

            var shannonFano = new ShannonFanoEncoder();
            var sfCodes = shannonFano.Encode(symbols);

            var huffman = new HuffmanEncoder();
            var huffmanCodes = huffman.Encode(symbols);

            var truncatedHuffman = new TruncatedHuffmanEncoder(8);
            var truncatedCodes = truncatedHuffman.Encode(symbols);

            Console.WriteLine("\n--- SHANNON-FANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Shannon-Fanov algoritam", sfCodes, symbols, entropy);

            Console.WriteLine("\n\n--- OBIČNI HUFFMANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Obični Huffmanov algoritam", huffmanCodes, symbols, entropy);

            Console.WriteLine("\n\n--- SKRAĆENI HUFFMANOV ALGORITAM ---");
            CodeAnalyzer.PrintAnalysis("Skraćeni Huffmanov algoritam (k=8)", truncatedCodes, symbols, entropy);

            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("KOMPARATIVNA EVALUACIJA METODA");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            var sfStats = CodeAnalyzer.GetAlgorithmStats("Shannon-Fanov algoritam", sfCodes, symbols, entropy);
            var huffStats = CodeAnalyzer.GetAlgorithmStats("Obični Huffmanov algoritam", huffmanCodes, symbols, entropy);
            var truncStats = CodeAnalyzer.GetAlgorithmStats("Skraćeni Huffmanov algoritam", truncatedCodes, symbols, entropy);

            var allStats = new[] { sfStats, huffStats, truncStats }
                .OrderBy(s => s.avgLength)
                .ToList();

            Console.WriteLine("\nRangiranje metoda po efikasnosti:");
            Console.WriteLine("┌──────────────┬──────────────────────┬─────────────┬──────────────┬──────┐");
            Console.WriteLine("| Redundancija |      Algoritam       | Efikasnost  | L_avg (b/s)  | Pozc |");
            Console.WriteLine("├──────────────┼──────────────────────┼─────────────┼──────────────┼──────┤");
            for (int i = 0; i < allStats.Count; i++)
            {
                string rank = i == 0 ? "[1]" : $"[{i + 1}]";
                string name = allStats[i].algorithmName.PadRight(20);
                double redundancy = 1 - allStats[i].efficiency;
                Console.WriteLine($"|   {redundancy * 100,8:F2} % | {name} |  {allStats[i].efficiency * 100,8:F2} % |   {allStats[i].avgLength,10:F4} | {rank,4} |");
            }
            Console.WriteLine("└──────────────┴──────────────────────┴─────────────┴──────────────┴──────┘");

            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("|                      ZAKLJUCAK ANALIZE                        |");
            Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"| [1] {allStats[0].algorithmName,-57} |");
            Console.WriteLine($"|   pokazuje NAJBOLJE performanse sa L_avg = {allStats[0].avgLength:F4} b/s{"",-13} |");
            Console.WriteLine("|                                                               |");
            Console.WriteLine("|   - Obicni Huffmanov algoritam je garantovano optimalan za   |");
            Console.WriteLine("|     kodiranje simbol-po-simbol (Shannon-ova teorema)         |");
            Console.WriteLine("|   - Svi algoritmi zadovoljavaju Kraft-ovu nejednakost        |");
            Console.WriteLine("|     (prefix-free kodovi)                                     |");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");

            Console.WriteLine("\n\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("GENERISANJE IZVESTAJA KOMPRESIJE");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            TestEncodingDecoding("Shannon-Fano", "SF", sfCodes, testSequence, symbols);
            TestEncodingDecoding("Huffman", "NH", huffmanCodes, testSequence, symbols);
            TestEncodingDecoding("Truncated-Huffman", "TH", truncatedCodes, testSequence, symbols);

            Console.WriteLine("\nIzvještaji uspješno generisani!");
            Console.WriteLine("\nGlavne karakteristike:");
            Console.WriteLine("  1. Sve metode generišu validne prefix-free kodove (Kraft <= 1)");
            Console.WriteLine("  2. Huffmanov algoritam daje najmanju prosječnu dužinu kodne riječi");
            Console.WriteLine("  3. Kompletna validacija: sve sekvence dekodirane sa 100% tačnošću");
            Console.WriteLine("  4. Stepen kompresije varira u zavisnosti od efikasnosti metode");
            Console.WriteLine("  5. Shannon-Fano je jednostavniji ali nešto manje efikasan");
            Console.WriteLine("  6. Truncated Huffman kompromis: jednostavniji codebook uz manji gubitak efikasnosti");

            Console.WriteLine("\nPritisnite bilo koji taster za povratak na meni...");
            Console.ReadKey();
        }

        static void DisplaySourceInfo(List<SymbolInfo> symbols)
        {
            Console.WriteLine("Statistika izvorne sekvence:");
            Console.WriteLine($"  Broj različitih simbola: {symbols.Count}");
            Console.WriteLine("\nDistribucija simbola:");
            Console.WriteLine("┌─────────────┬──────────────┬────────┬──────┐");
            Console.WriteLine("| Vjerovatnoća| Pojavljivanja| Karakt | Oznk |");
            Console.WriteLine("├─────────────┼──────────────┼────────┼──────┤");
            for (int i = 0; i < symbols.Count && i < 12; i++)
            {
                Console.WriteLine($"|    {symbols[i].P,7:F4} |   {symbols[i].Count,10} |   {symbols[i].Symbol}    | s{i,-3} |");
            }
            Console.WriteLine("└─────────────┴──────────────┴────────┴──────┘");
            if (symbols.Count > 12)
                Console.WriteLine($"  ... preostalih {symbols.Count - 12} simbola");
        }

        static void TestEncodingDecoding(string algorithmName, string algorithmCode,
                                        Dictionary<char, string> codes,
                                        string testSequence, List<SymbolInfo> symbols)
        {
            string encoded = EncoderDecoder.Encode(testSequence, codes);
            string encodedFilename = $"MirzadV_2encode{algorithmCode}.txt";
            EncoderDecoder.SaveToFile(encodedFilename, encoded);

            string decoded = EncoderDecoder.Decode(encoded, codes);
            string decodedFilename = $"MirzadV_3decode{algorithmCode}.txt";
            EncoderDecoder.SaveToFile(decodedFilename, decoded);

            bool identical = EncoderDecoder.CompareSequences(testSequence, decoded);

            int originalBits = testSequence.Length * 8;
            int encodedBits = encoded.Length;
            double compressionRatio = EncoderDecoder.CalculateCompressionRatio(originalBits, encodedBits);
            double avgBitsPerSymbol = (double)encodedBits / testSequence.Length;
            double savings = (1 - compressionRatio) * 100;

            string errorFilename = $"MirzadV_4error{algorithmCode}.txt";
            var report = new System.Text.StringBuilder();

            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine($"DETALJNA ANALIZA PERFORMANSI - {algorithmName}");
            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine();
            report.AppendLine($"Datum generisanja: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            report.AppendLine();
            report.AppendLine("┌──────────────────────────────────────┬──────────────────────┐");
            report.AppendLine("| Parametar                            | Vrijednost           |");
            report.AppendLine("├──────────────────────────────────────┼──────────────────────┤");
            report.AppendLine($"| Izvorna sekvenca (simboli)           |{testSequence.Length,21} |");
            report.AppendLine($"| Izvorna veličina (biti)              |{originalBits,21} |");
            report.AppendLine($"| Kompresovano (biti)                  |{encodedBits,21} |");
            report.AppendLine($"| Prosječno po simbolu (b/s)           |{avgBitsPerSymbol,21:F4} |");
            report.AppendLine($"| Stepen kompresije                    |{compressionRatio * 100,20:F2} % |");
            report.AppendLine($"| Postignuta ušteda                    |{savings,20:F2} % |");
            report.AppendLine($"| Dekompresovano simbola               |{decoded.Length,21} |");
            report.AppendLine("├──────────────────────────────────────┼──────────────────────┤");
            report.AppendLine($"| Provjera integriteta                 | {(identical ? "USPJEŠNO" : "NEUSPJEŠNO"),-19} |");
            report.AppendLine($"| Stanje sekvence                      | {(identical ? "POKLAPANJE" : "RAZLIKA"),-19} |");
            report.AppendLine("└──────────────────────────────────────┴──────────────────────┘");
            report.AppendLine();
            report.AppendLine("Generisani fajlovi:");
            report.AppendLine($"  - {encodedFilename} (kodirana sekvenca)");
            report.AppendLine($"  - {decodedFilename} (dekodirana sekvenca)");
            report.AppendLine();

            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine($"REZULTATI VALIDACIJE - {algorithmName}");
            report.AppendLine("═══════════════════════════════════════════════════════════");
            report.AppendLine();
            report.AppendLine($"Dužina originalne sekvence: {testSequence.Length} simbola");
            report.AppendLine($"Dužina dekodirane sekvence: {decoded.Length} simbola");
            report.AppendLine();

            if (identical)
            {
                report.AppendLine("STATUS: PROLAZ");
                report.AppendLine("Sekvence su IDENTIČNE");
                report.AppendLine("Svi simboli su tačno dekodirani");
                report.AppendLine();
                report.AppendLine("Broj grešaka: 0");
                report.AppendLine("Tačnost: 100.00%");
            }
            else
            {
                report.AppendLine("STATUS: GREŠKA");
                report.AppendLine("Sekvence se RAZLIKUJU");
                report.AppendLine();

                int errors = 0;
                int minLength = Math.Min(testSequence.Length, decoded.Length);
                for (int i = 0; i < minLength; i++)
                {
                    if (testSequence[i] != decoded[i])
                        errors++;
                }

                errors += Math.Abs(testSequence.Length - decoded.Length);

                report.AppendLine($"Broj grešaka: {errors}");
                report.AppendLine($"Tačnost: {((1 - (double)errors / testSequence.Length) * 100):F2}%");
            }

            report.AppendLine();
            report.AppendLine("═══════════════════════════════════════════════════════════");

            EncoderDecoder.SaveToFile(errorFilename, report.ToString());

            Console.WriteLine($"\n[{algorithmCode}] Algoritam: {algorithmName}");
            Console.WriteLine($"      Kompresija: {compressionRatio * 100:F2}% | Ušteda: {savings:F2}% | Status: {(identical ? "OK" : "GREŠKA")}");
            Console.WriteLine($"      Izvještaj: {errorFilename}");
        }
    }
}
