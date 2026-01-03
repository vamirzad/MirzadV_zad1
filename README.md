# Source Coding and Information Theory Analyzer

A C# console application that implements and compares three source coding algorithms: Shannon-Fano, Huffman, and Truncated Huffman encoding.

## Features

- **Three Encoding Algorithms**:
  - Shannon-Fano Encoding
  - Huffman Encoding (optimal)
  - Truncated Huffman Encoding (k=8)

- **Comprehensive Analysis**:
  - Source entropy calculation
  - Kraft inequality verification
  - Average codeword length (L_avg)
  - Efficiency and redundancy metrics
  - Compression ratio analysis

- **Interactive Console Menu**:
  - Run individual algorithms
  - Compare all algorithms side-by-side
  - Encode/decode test sequences
  - Automatic verification

## Project Structure

```
App/
├── Program.cs                      # Main application with interactive menu
├── Models/
│   └── SymbolInfo.cs              # Data structures
├── Encoders/
│   ├── ShannonFanoEncoder.cs      # Shannon-Fano implementation
│   ├── HuffmanEncoder.cs          # Huffman implementation
│   └── TruncatedHuffmanEncoder.cs # Truncated Huffman implementation
├── Analysis/
│   ├── SourceAnalyzer.cs          # Entropy and source analysis
│   └── CodeAnalyzer.cs            # Code metrics analysis
└── Utils/
    └── EncoderDecoder.cs          # Encoding/decoding utilities
```

## Requirements

- .NET 10.0 or higher
- Works on Windows, macOS, and Linux

## Usage

### Running the Application

```bash
cd App
dotnet run
```

Or in Visual Studio: Open `App.csproj` and press F5

### Input File Format

Place your test sequence in a text file (default: `testna_sekvenca.txt`). The application will:
1. Automatically detect all unique symbols
2. Calculate their probabilities from the data
3. Build optimal encoding dictionaries

### Example Output

```
╔════════════════════════════════════════════════════════════════╗
║          SOURCE CODING AND INFORMATION THEORY ANALYSIS          ║
╚════════════════════════════════════════════════════════════════╝

ENCODING ALGORITHM MENU
═══════════════════════════════════════════════════════════════
Choose an encoding algorithm:
  [1] Shannon-Fano Encoding
  [2] Huffman Encoding
  [3] Truncated Huffman Encoding (k=8)
  [4] Run All Algorithms & Compare
  [5] Exit

Your choice (1-5):
```

## Algorithm Results

Based on the test sequence (10,000 symbols):

| Algorithm | L_avg (bits) | Efficiency | Space Savings |
|-----------|--------------|------------|---------------|
| **Huffman** ★ | 3.2734 | 99.09% | 59.08% |
| Truncated Huffman | 3.2837 | 98.78% | 58.95% |
| Shannon-Fano | 3.3756 | 96.09% | 57.80% |

**Huffman coding is mathematically optimal** for symbol-by-symbol encoding.

## Output Files

The application generates:
- `encoded_[algorithm].txt` - Binary encoded sequences
- `decoded_[algorithm].txt` - Decoded sequences (for verification)

## Assignment Details

This project implements all tasks from the source coding assignment:
1. ✅ Source entropy calculation
2. ✅ Three encoding algorithms implementation
3. ✅ Kraft inequality verification, L_avg, efficiency, redundancy
4. ✅ Optimality analysis
5. ✅ Sequence encoding (10,000 symbols)
6. ✅ Sequence decoding
7. ✅ Verification (100% accuracy)
8. ✅ Results analysis and comments

## License

Educational project - Free to use and modify
