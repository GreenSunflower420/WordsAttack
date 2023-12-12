using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa odpowiadająca za rozgrywkę
    /// </summary>
    public partial class Game : Window
    {
        /// <summary>
        /// Pole przechowujące liczbę grywalnych słów
        /// </summary>
        static readonly int ile_slow = 20;
        /// <summary>
        /// Tablica przechowująca słowa wylosowane na ekranie
        /// </summary>
        readonly Label[] pokaz_slowa = new Label[ile_slow];
        /// <summary>
        /// Tablica przechowująca informacje z klasy Slowa
        /// <see cref="Slowa.Slowa(string, string)"/>
        /// </summary>
        readonly Slowa[] tab = new Slowa[ile_slow];
        /// <summary>
        /// Zmienna odpowiadająca za szybkość słów na ekranie
        /// <para>Wartości <1;2></para>
        /// </summary>
        double szybkosc = 1;
        /// <summary>
        /// Zmienna czasowa odpowiadająca za prędkość przesuwania się słów
        /// </summary>
        readonly DispatcherTimer timer = new();
        /// <summary>
        /// Zmienna czasowa odpowiadająca za prędkość pojawiania się słów
        /// </summary>
        private readonly DispatcherTimer appearanceTimer = new();
        /// <summary>
        /// Zmienna całkowita od liczby żyć
        /// <para>Może przyjmować wartości <0;3></para>
        /// </summary>
        int liczba_zyc = 3;
        /// <summary>
        /// Zmienna przechowująca liczbę słów, które już pojawiły się na ekranie
        /// </summary>
        private int currentLabelIndex = 0;
        /// <summary>
        /// Pole odpowiadające za losowanie słów pojawiających się na ekranie z danej kategorii
        /// </summary>
        readonly Random rnd = new();
        /// <summary>
        /// Zmienna informująca o liczbie punktów
        /// </summary>
        int punkty = 0;
        /// <summary>
        /// Zmienna typu true/false użyta do akcji gracza "zamroź"
        /// </summary>
        private bool isTimerPaused = false;

        /// <summary>
        /// Konstruktor klasy rozgrywki
        /// </summary>
        /// <param name="kategoria">Informacja, który plik z kategorią należy wczytać</param>
        public Game(string kategoria)
        {
            InitializeComponent();

            ///<summary>
            /// Tablica przechowująca wszystkie słowa z pliku w języku polskim
            /// </summary>
            string[] slowa_pl = WczytajPlik("kategorie/" + kategoria + "_pl.txt");

            ///<summary>
            /// Tablica przechowująca tłumaczenia wszystkich słów z pliku w języku angielskim
            /// </summary>
            string[] slowa_eng = WczytajPlik("kategorie/" + kategoria + "_eng.txt"); 
            
            Losuj_slowa(slowa_pl, slowa_eng);

            myCanvas.Focus();

            InitializeTimers();
        }

        /// <summary>
        /// Po starcie gry wywołanie zmiennych czasowych od pojawiania się słów i ich prędkości 
        /// </summary>
        private void InitializeTimers()
        {
            appearanceTimer.Tick += AppearanceTimer_Tick;
            appearanceTimer.Interval = TimeSpan.FromMilliseconds(2000); // Interwał czasowy pojawiania się słów
            appearanceTimer.Start();

            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromMilliseconds(15); // Interwał czasowy przesuwania się słów
            timer.Start();
        }

        /// <summary>
        /// Metoda odpowiedzialna za pojawianie się słów na ekranie
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void AppearanceTimer_Tick(object? sender, EventArgs e)
        {
            ///<summary>
            /// Aktualna pozycja danego słowa
            /// <para>Jeśli słowo wyjdzie poza pewien obszar, wtedy będzie pewność, że się nie będzie stykać z kolejnym słowem, więc będzie można je wyświetlić na ekranie</para>
            /// </summary>
            double currentLeft = 600;
            ///<summary>
            ///Pole właściwości poprzedniego słowa z ekranu
            /// </summary>
            Label poprzedni = new();

            //Jeśli pierwsze słowo na ekranie - po prostu je pojaw
            if (currentLabelIndex == 0)
            {
                pokaz_slowa[currentLabelIndex].Visibility = Visibility.Visible; // Ustaw widoczność aktualnego słowa na true
                currentLabelIndex++;
            } // operacje pozwalające na oddzielenie od siebie słów, aby nie nachodziły na siebie po pojawieniu się na ekranie
            else
            {
                currentLeft = Canvas.GetLeft(pokaz_slowa[currentLabelIndex - 1]);
                poprzedni = pokaz_slowa[currentLabelIndex - 1];

                if (currentLabelIndex < ile_slow && (poprzedni.Visibility == Visibility.Visible || currentLeft < 700))
                {
                    pokaz_slowa[currentLabelIndex].Visibility = Visibility.Visible; // Ustaw widoczność aktualnego słowa na true
                    currentLabelIndex++;
                }
            }

            // Sprawdzenie czy wygenerowano wystarczającą liczbę słów
            if (currentLabelIndex >= ile_slow)
            {
                int licz_ile_zbitych = 0;
                for(int i=0; i<ile_slow; i++)
                {
                    if (pokaz_slowa[i].Visibility == Visibility.Collapsed)
                    {
                        licz_ile_zbitych++;
                    }
                }
                //sprawdzenie warunków wygranej
                if (licz_ile_zbitych == ile_slow)
                {
                    appearanceTimer.Stop(); // Zatrzymaj timer pojawiania się słów, gdy wszystkie słowa pojawią się
                    var helpWindow = new EndGame(punkty, "Wygrałeś!")
                    {
                        Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                        WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
                    };
                    helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                    helpWindow.Show();
                }
            }
        }

        /// <summary>
        /// Metoda odpowiadająca za szybkość przesuwania się słów
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        public void TimerTick(object? sender, EventArgs e)
        {
            for (int i = 0; i < ile_slow; i++)
            {
                if (pokaz_slowa[i].Visibility == Visibility.Visible)
                {
                    double currentLeft = Canvas.GetLeft(pokaz_slowa[i]);

                    // Przesunięcie labela w lewo

                    Canvas.SetLeft(pokaz_slowa[i], currentLeft - (rnd.Next() % 2) - szybkosc);

                    // Jeśli label osiągnął lewy koniec canvas, zatrzymaj go
                    if (currentLeft <= 0)
                    {
                        pokaz_slowa[i].Visibility = Visibility.Collapsed;
                        liczba_zyc--;
                        if (liczba_zyc == 2) zycie2.Visibility = Visibility.Collapsed;
                        else if (liczba_zyc == 1) zycie1.Visibility = Visibility.Collapsed;
                        else if (liczba_zyc < 1) zycie0.Visibility = Visibility.Collapsed;

                        if (Czy_koniec_gry())
                        {
                            timer.Stop();
                            appearanceTimer.Stop();
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Sprawdzenie warunków przegranej
        /// </summary>
        /// <returns></returns>
        private bool Czy_koniec_gry()
        {
            if (liczba_zyc <= 0)
            {
                this.Visibility = Visibility.Hidden;

                var helpWindow = new EndGame(punkty, "Koniec gry")
                {
                    Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                    WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
                };
                helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                helpWindow.Show();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda wczytująca wszystkie słowa z pliku i umieszczająca je do tablicy
        /// </summary>
        /// <param name="nazwa_pliku">Ścieżka dostępu do pliku</param>
        /// <returns>Zwraca tablicę ze słowami</returns>
        public static string[] WczytajPlik(string nazwa_pliku)
        {
            string[] lines = File.ReadAllLines("../../../" + nazwa_pliku);
            return lines;
        }

        /// <summary>
        /// Losowanie słów na potrzeby rozgrywki
        /// </summary>
        /// <param name="slowa_pl">Tablica przechowująca słowa polskie</param>
        /// <param name="slowa_eng">Tablica przechowująca tłumaczenia słów</param>
        public void Losuj_slowa(string[] slowa_pl, string[] slowa_eng)
        {
            for (int i = 0; i < ile_slow; i++)
            {
                int indeks = rnd.Next() % ile_slow;

                tab[i] = new Slowa(slowa_pl[indeks], slowa_eng[indeks]);

            }

            for (int i = 0; i < ile_slow; i++)
            {
                pokaz_slowa[i] = new Label
                {
                    Content = tab[i].Slowo_pl,
                    FontSize = 32,
                    Visibility = Visibility.Hidden
                };
                Canvas.SetLeft(pokaz_slowa[i], 780);
                Canvas.SetTop(pokaz_slowa[i], 155);
                myCanvas.Children.Add(pokaz_slowa[i]);
            }
        }

        /// <summary>
        /// Po wciśnięciu przycisku Enter, sprawdzenie czy użytkownik poprawnie przetłumaczył słowo z ekranu
        /// <para>W razie pomyłki prędkość słów się zwiększa</para>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void CzyEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                for (int i = 0; i < ile_slow; i++)
                {
                    if (pokaz_slowa[i].Visibility == Visibility.Visible)
                    {
                        string tlumaczenie_en = tab[i].Slowo_en.ToString().ToLower();
                        string tlumaczenie_pl = textBox1.Text.ToLower();
                        if (tlumaczenie_en == tlumaczenie_pl)
                        {
                            pokaz_slowa[i].Visibility = Visibility.Collapsed;

                            textBox1.Text = string.Empty;
                            punkty += 10;
                            label1.Content = "Punkty: " + punkty.ToString();
                        }
                        else
                        {
                            if (szybkosc <= 2.0)
                                szybkosc += 0.2;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Metoda odpowiedzialna za czyszczenie pola za każdą próbą wprowadzenia nowego słowa
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void TextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
        }

        /// <summary>
        /// Metoda odpowiedzialna za powrót do Menu głównego i zakończenie rozgrywki
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void MenuButton_Click(object? sender, RoutedEventArgs e)
        {

            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            timer.Stop();
            appearanceTimer.Stop();

            // Tworzenie nowego okna i ustawienie jego położenia
            var helpWindow = new MainWindow
            {
                Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
            };
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }

        /// <summary>
        /// Metoda odpowiedzialna za zamknięcie aktualnego okna gry, po przejściu do kolejnego
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void HelpWindow_Closed(object? sender, EventArgs e)
        {
            // Pokazywanie ponownie oryginalnego okna po zamknięciu nowego okna
            this.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Metoda odpowiedzialna za wciśnięcie przycisku bomby
        /// <list type="bullet">
        ///     <item><description>Po wciśnięciu następuje skanowanie planszy w poszukiwaniu najbliższego zamku słowa</description></item>
        ///     <item><description>Po zlokalizowaniu następuje odczytanie jego id</description></item>
        ///     <item><description>Następnie usunięcie słowa z planszy</description></item>
        /// </list>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void BombButton_Click(object sender, RoutedEventArgs e)
        {
            double maximum = 0;
            int id = 0;
            BombButton.IsEnabled = false;
            for (int i = 0; i < ile_slow; i++)
            {
                if (pokaz_slowa[i].Visibility == Visibility.Visible)
                {
                    double najblizej_zamku = Canvas.GetLeft(pokaz_slowa[i]);
                    if (najblizej_zamku < maximum)
                    {
                        maximum = najblizej_zamku;
                        id = i;
                    }
                }
            }
            pokaz_slowa[id].Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Metoda odpowiedzialna za zamrożenie słów (zatrzymanie ich przesuwania) na odpowiednią wartość czasu
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void SnowflakeButton_Click(object sender, RoutedEventArgs e)
        {
            SnowflakeButton.IsEnabled = false;
            if (!isTimerPaused)
            {
                // Jeśli timer nie jest zatrzymany, zatrzymaj go na 3 sekundy
                timer.Stop();
                appearanceTimer.Stop();
                isTimerPaused = true;
                DispatcherTimer pauseTimer = new()
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                pauseTimer.Tick += (s, args) =>
                {
                    pauseTimer.Stop();
                    timer.Start(); // Wznów timer po 3 sekundach
                    appearanceTimer.Start(); // Wznów timer po 3 sekundach
                    isTimerPaused = false;
                };
                pauseTimer.Start();
            }
            else
            {
                // Jeśli timer jest zatrzymany, wznow go
                timer.Start();
                appearanceTimer.Start();
                isTimerPaused = false;
            }
        }
    }
}
