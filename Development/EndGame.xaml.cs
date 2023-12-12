using System;
using System.Windows;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa wyświetlająca planszę po zakończonej rozgrywce
    /// </summary>
    public partial class EndGame : Window
    {
        /// <summary>
        /// Konstruktor parametrowy klasy
        /// </summary>
        /// <param name="punkty">Liczba punktów zdobyta przez gracza</param>
        /// <param name="tekst">Informacja o wygranej bądź przegranej w rozgrywce</param>
        public EndGame(int punkty, string tekst)
        {
            InitializeComponent();

            label1.Content = tekst;
            pktLabel.Content = "Punkty: " + punkty.ToString();
        }

        /// <summary>
        /// Metoda odpowiedzialna za powrót do Menu głównego
        /// <see cref="MainWindow"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            ///<summary>
            ///Zmienna otwierająca okno Menu głównego
            ///<see cref="MainWindow.MainWindow"/>
            /// </summary>
            var helpWindow = new MainWindow
            {
                Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
            };
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();

            // Tworzenie nowego okna i ustawienie jego położenia

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
    }
}
