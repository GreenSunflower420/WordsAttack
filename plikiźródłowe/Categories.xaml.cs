using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

namespace Development
{
    /// <summary>
    /// Interaction logic for Categories.xaml
    /// </summary>
    public partial class Categories : Window
    {
        public Categories()
        {
            InitializeComponent();

            stylizujButton(btnDom);
            stylizujButton(btnOwoce);
            stylizujButton(btnPojazdy);
            stylizujButton(btnRodzina);
            stylizujButton(btnZwierzeta);
            AnimateButtonColor(btnDom, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(btnOwoce, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(btnPojazdy, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(btnRodzina, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(btnZwierzeta, System.Windows.Media.Colors.LightBlue);
        }

        public void stylizujButton(Button btn)
        {
            btn.FontSize = 24;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
            btn.MinWidth = 400;
            btn.Background = System.Windows.Media.Brushes.LightGray;
            btn.Foreground = System.Windows.Media.Brushes.Black;
            btn.BorderBrush = System.Windows.Media.Brushes.Gray;
            btn.BorderThickness = new Thickness(2);
            btn.Cursor = System.Windows.Input.Cursors.Hand;

            btn.MouseEnter += (sender, e) => AnimateButtonColor(btn, System.Windows.Media.Colors.Brown);
            btn.MouseLeave += (sender, e) => AnimateButtonColor(btn, System.Windows.Media.Colors.LightBlue);
        }

        private void AnimateButtonColor(Button button, System.Windows.Media.Color targetColor)
        {
            ColorAnimation colorAnimation = new ColorAnimation
            {
                To = targetColor,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            SolidColorBrush originalBrush = button.Background as SolidColorBrush;
            if (originalBrush != null)
            {
                SolidColorBrush newBrush = new SolidColorBrush(originalBrush.Color);
                button.Background = newBrush;

                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            if (sender is Button button)
            {
                string buttonText = button.Content.ToString();
                // Tutaj możesz użyć buttonText do dalszych operacji
                var helpWindow = new Game(buttonText.ToLower());
                helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
                helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
                helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                helpWindow.Show();
            }

            // Tworzenie nowego okna i ustawienie jego położenia
            
        }

        private void HelpWindow_Closed(object sender, EventArgs e)
        {
            // Pokazywanie ponownie oryginalnego okna po zamknięciu nowego okna
            this.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
