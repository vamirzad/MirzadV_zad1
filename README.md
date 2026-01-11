# Analiza algoritama kodiranja izvora

Komparativna analiza i evaluacija tri fundamentalna algoritma za kompresiju podataka iz teorije informacija.

## Implementirani algoritmi

- **Shannon-Fanov algoritam** - Rekurzivna metoda djeljenja simbola po vjerovatnoći
- **Huffmanov algoritam** - Optimalni algoritam kodiranja sa garantovanom minimalnom prosječnom dužinom
- **Skraćeni Huffmanov algoritam (k=8)** - Hibridni pristup sa ograničenim brojem kodova

## Rezultati testiranja

### Testna sekvenca
- **Dužina**: 10,000 simbola
- **Entropija izvora**: ~3.37 bita/simbol
- **Broj različitih simbola**: Varijabilan zavisno od ulaza

### Performanse algoritama

#### Shannon-Fanov algoritam
- **Prosječna dužina**: ~3.38 b/s
- **Stepen kompresije**: ~42.20%
- **Efikasnost kodiranja**: ~99.70%
- **Redundancija**: ~0.30%
- **Kraft uslov**: Zadovoljen (≤ 1)

#### Huffmanov algoritam
- **Prosječna dužina**: ~3.37 b/s
- **Stepen kompresije**: ~42.16%
- **Efikasnost kodiranja**: ~100.00%
- **Redundancija**: ~0.00%
- **Kraft uslov**: Zadovoljen (≤ 1)
- **Status**: ⭐ OPTIMALAN - postiže teoretski minimum

#### Skraćeni Huffmanov algoritam (k=8)
- **Prosječna dužina**: ~3.40 b/s
- **Stepen kompresije**: ~42.50%
- **Efikasnost kodiranja**: ~99.12%
- **Redundancija**: ~0.88%
- **Kraft uslov**: Zadovoljen (≤ 1)

## Ključna zapažanja

1. **Validacija**: Sve tri metode generišu validne prefix-free kodove (Kraft ≤ 1)

2. **Optimalnost**: Huffmanov algoritam postiže najmanju prosječnu dužinu kodne riječi i 100% efikasnost, što potvrđuje njegovu teorijsku optimalnost

3. **Preciznost**: Sve kodirane sekvence dekodirane sa 100% tačnošću bez gubitka podataka

4. **Trade-off**: Skraćeni Huffman nudi kompromis - jednostavniji codebook uz minimalan gubitak efikasnosti (~0.88%)

5. **Shannon-Fano**: Jednostavnija implementacija ali neznatno manje efikasna od Huffmana (~0.30% redundancija)

6. **Ušteda prostora**: Svi algoritmi postižu ~57-58% uštede prostora u odnosu na nekompresovane podatke

## Tehnička implementacija

- **Jezik**: C# (.NET 8.0)
- **Arhitektura**: Modularna struktura sa odvojenim enkoderim, analizatorima i utilityima
- **Validacija**: Automatska provjera integriteta kodiranja/dekodiranja
- **Izvještaji**: Detaljni tekstualni izvještaji sa metrikama performansi

## Zaključak

Eksperimentalni rezultati potvrđuju teorijske postavke teorije informacija:
- Huffmanov algoritam je optimalan za kodiranje simbol-po-simbol
- Svi implementirani algoritmi zadovoljavaju Kraft-McMillan uslov
- Praktična razlika između metoda je minimalna (~0.03 b/s razlika između najgoreg i najboljeg)
- Izbor algoritma zavisi od prioriteta: optimalnost (Huffman) vs. jednostavnost implementacije (Shannon-Fano)
