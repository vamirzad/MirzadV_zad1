using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCoding.Utils
{
    public class EncoderDecoder
    {
        public static string Encode(string input, Dictionary<char, string> codebook)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                if (codebook.ContainsKey(c))
                    sb.Append(codebook[c]);
            }
            return sb.ToString();
        }

        public static string Decode(string bits, Dictionary<char, string> codebook)
        {
            var reverseCodebook = codebook.ToDictionary(kv => kv.Value, kv => kv.Key);

            var result = new StringBuilder();
            string current = "";

            foreach (char bit in bits)
            {
                current += bit;
                if (reverseCodebook.ContainsKey(current))
                {
                    result.Append(reverseCodebook[current]);
                    current = "";
                }
            }

            return result.ToString();
        }

        public static void SaveToFile(string filename, string content)
        {
            File.WriteAllText(filename, content);
        }

        public static string LoadFromFile(string filename)
        {
            return File.ReadAllText(filename);
        }

        public static bool CompareSequences(string original, string decoded)
        {
            return original == decoded;
        }

        public static double CalculateCompressionRatio(int originalBits, int compressedBits)
        {
            return (double)compressedBits / originalBits;
        }
    }
}
