# Source Coding Assignment - Project Summary

## Project Structure

```
App/
├── Program.cs                          # Main program orchestration (190 lines)
├── Models/
│   └── SymbolInfo.cs                   # Data structures (SymbolInfo, Node)
├── Encoders/
│   ├── ShannonFanoEncoder.cs           # Shannon-Fano algorithm implementation
│   ├── HuffmanEncoder.cs               # Standard Huffman algorithm implementation
│   └── TruncatedHuffmanEncoder.cs      # Shortened Huffman algorithm (k=8)
├── Analysis/
│   ├── SourceAnalyzer.cs               # Entropy calculation and sequence analysis
│   └── CodeAnalyzer.cs                 # Kraft inequality, L_avg, efficiency, redundancy
└── Utils/
    └── EncoderDecoder.cs               # Encoding/decoding utilities and file I/O
```

## All Tasks Completed ✓

### Task 1: Source Entropy Calculation
- **Entropy (H):** 3.2579 bits/symbol
- Formula: H = -Σ p(i) * log₂(p(i))
- Source: 12 symbols with probabilities P = {0.21, 0.16, 0.13, 0.11, 0.09, 0.07, 0.07, 0.05, 0.04, 0.03, 0.02, 0.02}

### Task 2: Three Encoding Algorithms Implemented
1. **Shannon-Fano** - Recursive probability splitting
2. **Standard Huffman** - Bottom-up optimal tree construction
3. **Truncated Huffman (k=8)** - Huffman for top 8 symbols + escape code for remaining

### Task 3: Code Analysis Results

#### Shannon-Fano
- Kraft Sum: 1.000000 ✓ VALID
- L_avg: 3.3700 bits/symbol
- Efficiency: 96.67%
- Redundancy: 3.33%

#### Huffman (OPTIMAL)
- Kraft Sum: 1.000000 ✓ VALID
- L_avg: 3.2900 bits/symbol
- Efficiency: 99.02%
- Redundancy: 0.98%

#### Truncated Huffman
- Kraft Sum: 1.000000 ✓ VALID
- L_avg: 3.2900 bits/symbol
- Efficiency: 99.02%
- Redundancy: 0.98%

### Task 4: Optimality Conclusion
**Huffman coding is OPTIMAL** with the lowest average codeword length (3.2900 bits/symbol).
- All algorithms satisfy the Kraft inequality (prefix-free codes)
- Huffman is guaranteed to be mathematically optimal for symbol-by-symbol encoding
- Shannon-Fano is simpler but 2.4% less efficient than Huffman

### Task 5-7: Encoding/Decoding Test Results

Test sequence: 10,000 symbols from testna_sekvenca.txt

| Algorithm | Encoded Bits | Avg Bits/Symbol | Compression Ratio | Verification |
|-----------|--------------|-----------------|-------------------|--------------|
| Shannon-Fano | 33,756 | 3.3756 | 42.20% | ✓ PASS |
| Huffman | 32,837 | 3.2837 | 41.05% | ✓ PASS |
| Truncated Huffman | 32,837 | 3.2837 | 41.05% | ✓ PASS |

- Original size: 80,000 bits (8 bits/symbol)
- Space savings: 57.80% to 58.95%
- All sequences decoded perfectly (100% match)

### Task 8: Key Observations

1. **All three algorithms produce valid prefix-free codes** (Kraft inequality satisfied)
2. **Huffman and Truncated Huffman tied for best performance** (3.2837 bits/symbol actual)
3. **Perfect reconstruction achieved** - All decoded sequences matched original exactly
4. **Significant compression** - Reduced file size by ~58% compared to fixed 8-bit encoding
5. **Shannon-Fano trades efficiency for simplicity** - 2.4% less efficient but easier to implement
6. **Truncated Huffman performs surprisingly well** - Despite simplified codebook, matches Huffman on this distribution

## Output Files Generated

- `encoded_shannon-fano.txt` (33 KB) - Binary encoded sequence
- `encoded_huffman.txt` (32 KB) - Binary encoded sequence
- `encoded_truncated-huffman.txt` (32 KB) - Binary encoded sequence
- `decoded_shannon-fano.txt` (9.8 KB) - Decoded text sequence
- `decoded_huffman.txt` (9.8 KB) - Decoded text sequence
- `decoded_truncated-huffman.txt` (9.8 KB) - Decoded text sequence

## Algorithm Correctness

✓ Shannon-Fano follows the recursive splitting formula
✓ Huffman follows the bottom-up tree construction
✓ Truncated Huffman uses escape mechanism with fixed-length suffixes
✓ All Kraft inequalities satisfied: Σ 2^(-li) ≤ 1
✓ All codes are prefix-free (uniquely decodable)

## Test Sequence Details

- File: testna_sekvenca.txt
- Format: Uses 0-9, A, B to represent symbols s0-s11
- Length: 10,000 symbols
- Actual entropy: 3.2437 bits/symbol (vs 3.2579 theoretical)
- Distribution closely matches expected probabilities
