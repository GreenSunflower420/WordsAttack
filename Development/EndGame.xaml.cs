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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Development
{
    /// <summary>
    /// Interaction logic for EndGame.xaml
    /// </summary>
    public partial class EndGame : Window
    {
        public EndGame(int punkty)
        {
            InitializeComponent();

            pktLabel.Content = "Punkty: " + punkty.ToString();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

                var helpWindow = new MainWindow();
                helpWindow.Owner = this; // Ustawianie oryginalnego okna jako właściciela nowego okna
                helpWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustawianie nowego okna na środku względem właściciela
                helpWindow.Closed += HelpWindow_Closed; // Dodanie obsługi zdarzenia zamknięcia nowego okna
                helpWindow.Show();

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
