using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Development
{
    /// <summary>
    /// Interaction logic for Dictionary.xaml
    /// </summary>
    public partial class Dictionary : Window
    {
        public ObservableCollection<DictionaryEntry> DictionaryEntries { get; set; }
        public Dictionary()
        {
            InitializeComponent();

            DictionaryEntries = new ObservableCollection<DictionaryEntry>();
            // Wczytanie danych z plików
            LoadDictionary("../../../pl.txt", "../../../eng.txt");


            DictionaryEntries = new ObservableCollection<DictionaryEntry>(DictionaryEntries.OrderBy(entry => entry.PolishWord));

            // Przypisanie kolekcji do źródła danych ListView
            dictionaryListView.ItemsSource = DictionaryEntries;


        }

        private void LoadDictionary(string polishWordsFile, string englishTranslationsFile)
        {
            try
            {
                // Odczyt słów polskich z pliku
                string[] polishWords = File.ReadAllLines(polishWordsFile);

                // Odczyt tłumaczeń angielskich z pliku
                string[] englishTranslations = File.ReadAllLines(englishTranslationsFile);

                // Sprawdzenie, czy liczba słów w obu plikach jest taka sama
                if (polishWords.Length == englishTranslations.Length)
                {
                    for (int i = 0; i < polishWords.Length; i++)
                    {
                        // Dodanie słów do kolekcji
                        DictionaryEntries.Add(new DictionaryEntry
                        {
                            PolishWord = polishWords[i],
                            EnglishTranslation = englishTranslations[i]
                        });
                    }
                }
                else
                {
                    MessageBox.Show("Błąd: Liczba słów w plikach nie jest taka sama.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wczytywania danych: {ex.Message}");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
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

    public class DictionaryEntry
    {
        public string PolishWord { get; set; }
        public string EnglishTranslation { get; set; }

    }
}
