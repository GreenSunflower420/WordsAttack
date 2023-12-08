using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Development
{

    public class Slowa
    {
        private string slowo_pl = "";
        private string slowo_en = "";
        private bool widoczne = false;
        public Slowa(string slowo_pl, string slowo_en, bool widoczne)
        {
            this.slowo_en = slowo_en;
            this.Slowo_pl = slowo_pl;
            this.widoczne = widoczne;


        }

        public string Slowo_pl { get { return slowo_pl; } set { slowo_pl = value; } }
        public string Slowo_en { get { return slowo_en; } set { slowo_en = value; } }
        public bool Widoczne { get { return widoczne; } set { widoczne = value; } }

    }
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        static int ile_slow = 30;
        Label[] pokaz_slowa = new Label[ile_slow];
        Slowa[] tab = new Slowa[ile_slow];
        double szybkosc = 0.5;
        DispatcherTimer timer = new DispatcherTimer();
        private readonly DispatcherTimer appearanceTimer = new DispatcherTimer();
        int liczba_zyc = 3;
        private int currentLabelIndex = 0;
        Random rnd = new Random();
        int punkty = 0;
        private bool isTimerPaused = false;

        public Game(string kategoria)
        {
            InitializeComponent();

            string[] slowa_pl = wczytajPlik("kategorie/" + kategoria + "_pl.txt");
            string[] slowa_eng = wczytajPlik("kategorie/" + kategoria + "_eng.txt"); 
            
            Losuj_slowa(slowa_pl, slowa_eng);

            myCanvas.Focus();

            InitializeTimers();
        }

        private void InitializeTimers()
        {
            appearanceTimer.Tick += AppearanceTimer_Tick;
            appearanceTimer.Interval = TimeSpan.FromMilliseconds(2000); // Interwał czasowy pojawiania się słów
            appearanceTimer.Start();

            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(30); // Interwał czasowy przesuwania się słów
            timer.Start();
        }

        private void AppearanceTimer_Tick(object sender, EventArgs e)
        {
            pokaz_slowa[currentLabelIndex].Visibility = Visibility.Visible; // Ustaw widoczność aktualnego słowa na true
            currentLabelIndex++;

            if (currentLabelIndex >= ile_slow)
            {
                appearanceTimer.Stop(); // Zatrzymaj timer pojawiania się słów, gdy wszystkie słowa pojawią się
            }
        }


        public void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
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

                        if (czy_koniec_gry())
                        {
                            timer.Stop();
                            appearanceTimer.Stop();
                        }
                    }
                }
            }


        }

        private bool czy_koniec_gry()
        {
            if (liczba_zyc <= 0)
            {
                this.Visibility = Visibility.Hidden;

                var helpWindow = new EndGame(punkty);
                helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
                helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
                helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                helpWindow.Show();
                return true;
            }
            return false;
        }

        public string[] wczytajPlik(string nazwa_pliku)
        {
            string[] lines = File.ReadAllLines("../../../" + nazwa_pliku);
            return lines;
        }

        public void Losuj_slowa(string[] slowa_pl, string[] slowa_eng)
        {

            int indeks = 0;
            for (int i = 0; i < ile_slow; i++)
            {
                indeks = rnd.Next() % 30;

                tab[i] = new Slowa(slowa_pl[indeks], slowa_eng[indeks], false);

            }

            for (int i = 0; i < ile_slow; i++)
            {
                pokaz_slowa[i] = new Label();
                pokaz_slowa[i].Content = tab[i].Slowo_pl;
                pokaz_slowa[i].FontSize = 22;
                pokaz_slowa[i].Visibility = Visibility.Hidden;
                Canvas.SetLeft(pokaz_slowa[i], 700);
                Canvas.SetTop(pokaz_slowa[i], 170);
                myCanvas.Children.Add(pokaz_slowa[i]);
            }
        }


        private void czyEnter(object sender, KeyEventArgs e)
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
                            do
                            {
                                szybkosc += 0.005;
                            } while (szybkosc < 2.0);
                        }
                    }
                }
            }

        }


        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            // Tworzenie nowego okna i ustawienie jego położenia
            var helpWindow = new MainWindow();
            helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
            helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }

        private void HelpWindow_Closed(object sender, EventArgs e)
        {
            // Pokazywanie ponownie oryginalnego okna po zamknięciu nowego okna
            this.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BombButton_Click(object sender, RoutedEventArgs e)
        {
            double najblizej_zamku = 0;
            double maximum = 0;
            int id = 0;
            BombButton.IsEnabled = false;
            for (int i = 0; i < ile_slow; i++)
            {
                if (pokaz_slowa[i].Visibility == Visibility.Visible)
                {
                    najblizej_zamku = Canvas.GetLeft(pokaz_slowa[i]);
                    if (najblizej_zamku < maximum)
                    {
                        maximum = najblizej_zamku;
                        id = i;
                    }
                }
            }
            pokaz_slowa[id].Visibility = Visibility.Collapsed;

        }

        private void SnowflakeButton_Click(object sender, RoutedEventArgs e)
        {
            SnowflakeButton.IsEnabled = false;
            if (!isTimerPaused)
            {
                // Jeśli timer nie jest zatrzymany, zatrzymaj go na 3 sekundy
                timer.Stop();
                appearanceTimer.Stop();
                isTimerPaused = true;
                DispatcherTimer pauseTimer = new DispatcherTimer();
                pauseTimer.Interval = TimeSpan.FromSeconds(3);
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
