using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa wyświetlacją początkowe okno gry - Menu główne
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor klasy wywołujacy metodę odpowiedzialną za tworzenie Menu na ekranie
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            CreateMenu();
        }

        /// <summary>
        /// Metoda odpowiedzialna za tworzenie Menu - panelu, przycisków oraz labelów
        /// </summary>
        private void CreateMenu()
        {
            StackPanel mainStackPanel = new();
            this.Content = mainStackPanel;

            ///<summary>
            // Duży Label z nazwą gry
            ///</summary>
            Label gameLabel = new()
            {
                Content = "Words Attack!",
                FontSize = 72,
                FontFamily = new FontFamily("Roboto"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Dodano margines dla estetyki
            };
            mainStackPanel.Children.Add(gameLabel);

            ///<summary>
            // Przycisk "Graj"
            ///</summary>
            Button playButton = CreateStyledButton("Graj");
            playButton.Click += PlayButton_Click;
            mainStackPanel.Children.Add(playButton);

            ///<summary>
            // Przycisk "Pomoc"
            ///</summary>
            Button helpButton = CreateStyledButton("Pomoc");
            helpButton.Click += HelpButton_Click;
            mainStackPanel.Children.Add(helpButton);

            ///<summary>
            // Przycisk "Słowniczek"
            ///</summary>
            Button dictionaryButton = CreateStyledButton("Słowniczek");
            dictionaryButton.Click += DictionaryButton_Click;
            mainStackPanel.Children.Add(dictionaryButton);

            ///<summary>
            // Przycisk "Wyjście"
            ///</summary>
            Button exitButton = CreateStyledButton("Wyjście");
            exitButton.Click += ExitButton_Click;
            mainStackPanel.Children.Add(exitButton);


            AnimateButtonColor(playButton, Colors.LightBlue);
            AnimateButtonColor(helpButton, Colors.LightBlue);
            AnimateButtonColor(dictionaryButton, Colors.LightBlue);
            AnimateButtonColor(exitButton, Colors.LightBlue);
        }

        /// <summary>
        /// Metoda odpowiadająca za stylizowanie przycisków
        /// <para>Każdy przycisk ma mieć te same właściwości, aby menu wyglądało estetycznie</para>
        /// </summary>
        /// <param name="btn">Reprezentant właściwości danego przycisku</param>
        private static Button CreateStyledButton(string content)
        {
            ///<summary>
            ///Tworzenie oraz umiejscowienie przycisku na planszy za pomocą jego właściwości
            /// </summary>
            Button button = new()
            {
                Content = content,
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Segoe UI"),
                Margin = new Thickness(0, 50, 0, 20),
                Padding = new Thickness(20, 10, 20, 10), // Dostosowano padding dla estetyki
                MinWidth = 200, // Ustawiono minimalną szerokość przycisku
                Background = Brushes.LightGray,
                Foreground = Brushes.Black,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(2),
                Cursor = Cursors.Hand
            };

            button.MouseEnter += (sender, e) => AnimateButtonColor(button, Colors.Brown);
            button.MouseLeave += (sender, e) => AnimateButtonColor(button, Colors.LightBlue);

            return button;
        }

        /// <summary>
        /// Metoda odpowiadająca za ustawienie koloru przycisku dla animacji
        /// </summary>
        /// <param name="button">Reprezentant właściwości danego przycisku</param>
        /// <param name="targetColor">Wybrany kolor w zależności od stanu przycisku</param>
        private static void AnimateButtonColor(Button button, Color targetColor)
        {
            ColorAnimation colorAnimation = new()
            {
                To = targetColor,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            if (button.Background is SolidColorBrush originalBrush)
            {
                SolidColorBrush newBrush = new(originalBrush.Color);
                button.Background = newBrush;

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za wyświetlenie okna zasad gry
        /// <see cref="Window1"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            ///<summary>
            ///Zmienna otwierająca okno zasad gry
            ///<seealso cref="Window1.Window1"/>
            /// </summary>
            var helpWindow = new Window1
            {
                Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
            };
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }

        /// <summary>
        /// Metoda odpowiedzialna za przejście do Menu wyboru katogorii
        /// <see cref="Categories"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            ///<summary>
            ///Zmienna otwierająca dalsze okno rozpoczęcia rozgrywki - Wybór kategorii
            ///<seealso cref="Categories.Categories"/>
            /// </summary>
            var helpWindow = new Categories
            {
                Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
            };
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }

        /// <summary>
        /// Metoda odpowiedzialna za przejście do Słownika gry
        /// <see cref="Dictionary"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void DictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            ///<summary>
            ///Zmienna otwierająca okno Słownika
            ///<seealso cref="Dictionary.Dictionary"/>
            /// </summary>
            var helpWindow = new Dictionary
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
        /// Metoda odpowiedzialna za wyjście z gry i zatrzymanie działania programu
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
