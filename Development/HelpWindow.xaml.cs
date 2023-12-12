using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa informująca o zasadach gry
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// Konstruktor klasy inicjalizujący obiekty na ekranie
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            HelpMenu();
        }

        /// <summary>
        /// Metoda tworząca Panel oraz wstawiająca przyciski programowalnie
        /// </summary>
        private void HelpMenu()
        {
            ///<summary>
            /// Panel na ekranie, na którym będą przyciski oraz tekst
            /// </summary>
            StackPanel mainStackPanel = new();
            this.Content = mainStackPanel;

            ///<summary>
            /// Duży Label z nazwą gry
            ///</summary>
            Label gameLabel = new()
            {
                Content = "Pomoc",
                FontSize = 72,
                FontFamily = new FontFamily("Roboto"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Dodano margines dla estetyki
            };
            mainStackPanel.Children.Add(gameLabel);

            ///<summary>
            /// Opis gry wraz z zasadami
            ///</summary>
            TextBlock opis = new()
            {
                Text = "Użytkownik uruchamia grę po czym wyświetla się menu główne.\r\n❖ Z poziomu menu gracz może rozpocząć rozgrywkę, opuścić grę oraz skorzystać ze słowniczka.\r\n❖ Po rozpoczęciu rozgrywki użytkownik zostaje zaatakowany przez armię słów, aby się bronić w wyznaczone do tego pole należy wpisać tłumaczenie wyświetlanego słowa.\r\n❖ Gracz będzie miał możliwość użycia umiejętności ułatwiających rozgrywkę, np. zamrożenia - aby zatrzymać słowa w miejscu i zyskać parę sekund więcej na odpowiedź. Będzie to ograniczone czasem odnowienia\r\n❖ W przypadku wpisaniu złego tłumaczenia, słowo przyspieszy zmniejszając czas na kolejną odpowiedź.\r\n❖ Gra kończy się w przypadku stracenia wszystkich żyć, pokonania wszystkich słów lub wyjścia do menu/wyjścia z aplikacji.",
                FontSize = 24,
                FontFamily = new FontFamily("Roboto"),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 36,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Dodano margines dla estetyki
            };
            mainStackPanel.Children.Add(opis);

            ///<summary>
            /// Przycisk powrotny do Menu głównego
            /// <see cref="MainWindow"/>
            ///</summary>
            Button helpButton = CreateStyledButton("Wróc do menu");
            helpButton.Click += HelpButton_Click;
            mainStackPanel.Children.Add(helpButton);
        }

        /// <summary>
        /// Metoda inicjalizująca stylizowanie przycisków na ekranie
        /// </summary>
        /// <param name="content">Zmienna przechowująca treść przycisku</param>
        /// <returns></returns>
        private static Button CreateStyledButton(string content)
        {
            ///<summary>
            /// Tworzenie przycisku wraz z jego właściwościami
            ///</summary>
            Button button = new()
            {
                Content = content,
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin = new Thickness(0, 50, 0, 20),
                Padding = new Thickness(20, 10, 20, 10), // Dostosowano padding dla estetyki
                MinWidth = 200, // Ustawiono minimalną szerokość przycisku
                Background = System.Windows.Media.Brushes.LightGray,
                Foreground = System.Windows.Media.Brushes.Black,
                BorderBrush = System.Windows.Media.Brushes.Gray,
                BorderThickness = new Thickness(2),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            button.MouseEnter += (sender, e) => AnimateButtonColor(button, System.Windows.Media.Colors.Brown);
            button.MouseLeave += (sender, e) => AnimateButtonColor(button, System.Windows.Media.Colors.LightBlue);

            return button;
        }

        /// <summary>
        /// Metoda odpowiadająca za animację kolorów przycisków
        /// </summary>
        /// <param name="button">Parametr przycisku</param>
        /// <param name="targetColor">Parametr koloru przycisku</param>
        private static void AnimateButtonColor(Button button, Color targetColor)
        {
            /// <summary>
            /// Ustawienie czasu oraz koloru dla animacji najechania myszką na przycisk
            /// </summary>
            ColorAnimation colorAnimation = new()
            {
                To = targetColor,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            /// <summary>
            /// Ustawienie tła dla przycisku
            /// </summary>
            if (button.Background is SolidColorBrush originalBrush)
            {
                SolidColorBrush newBrush = new(originalBrush.Color);
                button.Background = newBrush;

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }
        }

        /// <summary>
        /// Metoda odpowiadająca za powrót do Menu Głownego gry
        /// <see cref="MainWindow.MainWindow"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            ///<summary>
            /// Zmienna otwierająca okno Menu głównego
            ///<see cref="MainWindow.MainWindow"/>
            /// </summary>
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
    }
}
