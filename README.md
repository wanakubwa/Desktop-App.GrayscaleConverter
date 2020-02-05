# Desktop App - Grayscale converter (Asm/Cpp)


Pliki źródłowe aplikacji desktopowej służącej do konwersji obrazów o formacie JPG na skalę szarości. Sam algorytm wykorzystany w programie napisany został w języku assemblera oraz C++ z wykorzystaniem modelu SIMD. Aplikacja utrzymana jest w ciemnej kolorystyce oraz umożlwia wybór ilośći watków oraz biblioteki DLL.<br>

## [0.1.0] - 2020-01-15

### Dodano

- Możliwość wyboru biblioteki dll (Asm/Cpp).
- Opcja wyboru ilości wątków (1-64).
- Możliwość zapisu przekonwertowanego obrazu.
- Opis wewnątrz aplikacji.
- Zabezpieczenie przed brakiem instrukcji wektorowych w procesorze.


## Technologie

Przy tworzeniu aplikacji zostały wykorzystane następujące technologie:

- Visual Studio 2017 - Środowisko programistyczne.
- .Net 4.7.1         - Framework wykorzystany do stworzenia aplikacji.
- C#                 - Język programowania, w którym została napisana część obsługi widoków.
- Asm                - Język programowania niskiego poziomu wykorzystany do napisania algorytmu konwersji.
- C++                - Język programowania wysokiego poziomu wykorzystany do napisania algorytm konwersji.
- SIMD               - Model architektury umożliwiający wykonywanie wielu operacji na dużej ilości danych jednocześnie. 

## Zrzuty ekranu z aplikacji

<p align="center">
<img src= "Img/App_result_view.PNG" width="80%">
  <br>
Okno wyboru ilości watków, biblioteki źródłowej oraz zapisu obrazu.  
</p>

<p align="center">
<img src= "Img/App_choose_img_view.PNG" width=80%">
  <br>
Widok wyboru obrazu do konwersji.  
</p>

<p align="center">
<img src= "Img/App_chart_small.PNG" width="90%">
  <br>
Wykres pokazujący różnice w czasie wykonywania operacji algorytmów napisanych w Asm oraz c++.  
</p>
