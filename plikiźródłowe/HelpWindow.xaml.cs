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

namespace Development
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            HelpMenu();
        }

        private void HelpMenu()
        {
            StackPanel mainStackPanel = new StackPanel();
            this.Content = mainStackPanel;

            // Duży Label z nazwą gry
            Label gameLabel = new Label
            {
                Content = "Pomoc",
                FontSize = 72,
                FontFamily = new FontFamily("Roboto"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Dodano margines dla estetyki
            };
            mainStackPanel.Children.Add(gameLabel);

            // Duży Label z nazwą gry
            TextBlock opis = new TextBlock
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

            Button helpButton = CreateStyledButton("Wróc do menu");
            helpButton.Click += HelpButton_Click;
            mainStackPanel.Children.Add(helpButton);
        }
        private Button CreateStyledButton(string content)
        {
            Button button = new Button
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


        private void HelpButton_Click(object sender, RoutedEventArgs e)
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
    }
}
