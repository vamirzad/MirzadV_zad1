# Analizator kodiranja izvora

Program za analizu i poređenje algoritama kodiranja izvora: Shannon-Fanov, obični Huffmanov i skraćeni Huffmanov algoritam.

## Kako pokrenuti program

### Na Windows-u (Visual Studio):

1. Otvori `App/App.csproj` u Visual Studio-u
2. Pritisni F5 ili klikni "Start"

### Na Mac-u ili Linux-u:

```bash
cd App
dotnet run
```

## Kako koristiti program

Kada pokrenete program, pojaviće se meni sa 5 opcija:

```
[1] Shannon-Fanov algoritam
[2] Obični Huffmanov algoritam
[3] Skraćeni Huffmanov algoritam (k=8)
[4] Pokreni sve algoritme i uporedi
[5] Izlaz
```

- **Opcije 1-3**: Pokreću pojedinačni algoritam i prikazuju rezultate
- **Opcija 4**: Pokreće sva tri algoritma i poredi ih
- **Opcija 5**: Izlaz iz programa

## Testni fajl

Program učitava testnu sekvencu iz fajla. Kada pokrenete program, pita vas:

```
Unesite naziv datoteke (default: testna_sekvenca.txt):
```

- Samo pritisnite Enter ako želite koristiti `testna_sekvenca.txt`
- Ili unesite ime drugog fajla koji se nalazi u istom folderu

**Važno**: Testni fajl mora biti u istom folderu gdje se program pokreće.

## Šta program radi

1. **Učitava testnu sekvencu** iz fajla
2. **Analizira simbole** - broji koliko puta se svaki simbol pojavljuje
3. **Izračunava entropiju** izvora
4. **Kodira simbole** odabranim algoritmom
5. **Prikazuje rezultate**:
   - Kraft-ovu nejednakost
   - Prosječnu dužinu kodne riječi
   - Efikasnost i redundanciju
   - Kodne riječi za svaki simbol
6. **Testira kodiranje/dekodiranje** - kodira cijelu sekvencu i dekodira je nazad
7. **Provjerava tačnost** - poredi originalnu sa dekodiranom sekvencom

## Izlazni fajlovi

Nakon pokretanja algoritma, program kreira fajlove:

- `encoded_shannon-fano.txt` - kodirani tekst
- `decoded_shannon-fano.txt` - dekodirani tekst
- `encoded_huffman.txt` - kodirani tekst
- `decoded_huffman.txt` - dekodirani tekst
- `encoded_truncated-huffman.txt` - kodirani tekst
- `decoded_truncated-huffman.txt` - dekodirani tekst

## Primjer korištenja

1. Pokrenite program
2. Pritisnite Enter (koristi `testna_sekvenca.txt`)
3. Izaberite opciju `4` da vidite poređenje svih algoritama
4. Pogledajte rezultate i zaključak koji je algoritam najoptimalniji
5. Pritisnite bilo koji taster za povratak na meni
6. Izaberite `5` za izlaz

## Requirements

- .NET 8 ili noviji
- Testni fajl sa simbolima za kodiranje

## Microsoft Copilot chat:

URL: https://copilot.microsoft.com/shares/jqDcEBGfFv2XKXfMFF51B
