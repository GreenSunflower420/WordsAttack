using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa gry wyświetlająca menu wyboru kategorii
    /// </summary>
    public partial class Categories : Window
    {
        /// <summary>
        /// Konstruktor klasy, inicjujący wygląd okna oraz animacje przycisków
        /// </summary>
        /// 
        Color kolor = Colors.LightBlue;
        public Categories()
        {
            InitializeComponent();

            StylizujButton(btnDom);
            StylizujButton(btnOwoce);
            StylizujButton(btnPojazdy);
            StylizujButton(btnRodzina);
            StylizujButton(btnZwierzeta);
            AnimateButtonColor(btnDom, kolor);
            AnimateButtonColor(btnOwoce, kolor);
            AnimateButtonColor(btnPojazdy, kolor);
            AnimateButtonColor(btnRodzina, kolor);
            AnimateButtonColor(btnZwierzeta, kolor);
        }
        
        /// <summary>
        /// Metoda odpowiadająca za stylizowanie przycisków
        /// <para>Każdy przycisk ma mieć te same właściwości, aby menu wyglądało estetycznie</para>
        /// </summary>
        /// <param name="btn">Reprezentant właściwości danego przycisku</param>
        public void StylizujButton(Button btn)
        {
            btn.FontSize = 24;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.FontFamily = new FontFamily("Segoe UI");
            btn.MinWidth = 400;
            btn.Background = Brushes.LightGray;
            btn.Foreground = Brushes.Black;
            btn.BorderBrush = Brushes.Gray;
            btn.BorderThickness = new Thickness(2);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (sender, e) => AnimateButtonColor(btn, Colors.Brown);
            btn.MouseLeave += (sender, e) => AnimateButtonColor(btn, kolor);
        }

        /// <summary>
        /// Metoda odpowiadająca za ustawienie koloru przycisku dla animacji
        /// </summary>
        /// <param name="button">Reprezentant właściwości danego przycisku</param>
        /// <param name="targetColor">Wybrany kolor w zależności od stanu przycisku</param>
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
        /// Wybranie danej kategorii
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            if (sender is Button button)
            {
                /// <summary>
                /// Wartość tekstowa przekazywana do klasy Game.xaml.cs, która zawiera nazwę kategorii
                /// </summary>
                string buttonText = button.Content.ToString();
                // Tutaj możesz użyć buttonText do dalszych operacji
                ///<summary>
                /// Otwarcie nowego okna z klasą i przekazanie wybranej kategorii
                /// </summary>
                Game helpWindow = new(buttonText.ToLower())
                {
                    Owner = this, // Ustawianie oryginalnego okna jako właściciela nowego okna
                    WindowStartupLocation = WindowStartupLocation.CenterOwner // Ustawianie nowego okna na środku względem właściciela
                };
                helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                helpWindow.Show();
            }

            // Tworzenie nowego okna i ustawienie jego położenia
            
        }

        /// <summary>
        /// Metoda odpowiadająca za zamknięcie nieużywanego już danego okna klasy
        /// <para>Po przejściu do nowego okna</para>
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
