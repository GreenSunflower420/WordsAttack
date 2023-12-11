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

namespace Development
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Label[] pokaz_slowa = new Label[20];
        Slowa[] tab = new Slowa[20];
        int szybkosc = 10;
        DispatcherTimer timer = new DispatcherTimer();
        private readonly DispatcherTimer appearanceTimer = new DispatcherTimer();
        int ile_slow = 20;
        int liczba_zyc = 3;
        private int currentLabelIndex = 0;
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            CreateMenu();
            //ShowMenu();

            /*
            string[] slowa_pl = wczytajPlik("pl.txt");
            string[] slowa_eng = wczytajPlik("eng.txt");


            Losuj_slowa(slowa_pl, slowa_eng);

            myCanvas.Focus();

            InitializeTimers();
            */
        }


        private void CreateMenu()
        {
            StackPanel mainStackPanel = new StackPanel();
            this.Content = mainStackPanel;

            // Duży Label z nazwą gry
            Label gameLabel = new Label
            {
                Content = "Words Attack!",
                FontSize = 72,
                FontFamily = new FontFamily("Roboto"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Dodano margines dla estetyki
            };
            mainStackPanel.Children.Add(gameLabel);

            // Przycisk "Graj"
            Button playButton = CreateStyledButton("Graj");
            playButton.Click += PlayButton_Click;
            mainStackPanel.Children.Add(playButton);

            // Przycisk "Pomoc"
            Button helpButton = CreateStyledButton("Pomoc");
            helpButton.Click += HelpButton_Click;
            mainStackPanel.Children.Add(helpButton);

            // Przycisk "Wyjście"
            Button dictionaryButton = CreateStyledButton("Słowniczek");
            dictionaryButton.Click += DictionaryButton_Click;
            mainStackPanel.Children.Add(dictionaryButton);

            // Przycisk "Wyjście"
            Button exitButton = CreateStyledButton("Wyjście");
            exitButton.Click += ExitButton_Click;
            mainStackPanel.Children.Add(exitButton);


            AnimateButtonColor(playButton, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(helpButton, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(dictionaryButton, System.Windows.Media.Colors.LightBlue);
            AnimateButtonColor(exitButton, System.Windows.Media.Colors.LightBlue);
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
            this.Visibility = Visibility.Hidden;

            var helpWindow = new Window1();
            helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
            helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            var helpWindow = new Categories();
            helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
            helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
            helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
            helpWindow.Show();
        }
        private void DictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            var helpWindow = new Dictionary();
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


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
