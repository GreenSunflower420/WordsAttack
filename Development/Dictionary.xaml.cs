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

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa odpowiadająca za wyświetlanie słów, biorących udział w grze, w postaci słownika
    /// </summary>
    public partial class Dictionary : Window
    {
        /// <summary>
        /// Metoda get/set przechowująca wczytane z pliku słowa
        /// </summary>
        public ObservableCollection<DictionaryEntry> DictionaryEntries { get; set; }

        /// <summary>
        /// Konstruktor klasy przypisujący słowa z plików do kolekcji oraz sortującego słowa alfabetycznie
        /// </summary>
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

        /// <summary>
        /// Metoda wczytująca z plików słowa wraz z ich tłumaczniem
        /// </summary>
        /// <param name="polishWordsFile">Ścieżka do pliku tekstowego z polskimi słowami</param>
        /// <param name="englishTranslationsFile">Ścieżka do pliku tekstowego z tłumaczniem słów</param>
        private void LoadDictionary(string polishWordsFile, string englishTranslationsFile)
        {
            try
            {
                // Odczyt słów polskich z pliku
                ///<summary>
                /// Zmienna tekstowa tablicowa przechowująca wszystkie pliki
                /// </summary>
                string[] polishWords = File.ReadAllLines(polishWordsFile);

                // Odczyt tłumaczeń angielskich z pliku
                ///<summary>
                /// Zmienna tekstowa tablicowa przechowująca wszystkie pliki
                /// </summary>
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

        /// <summary>
        /// Metoda odpowiadająca za powrót do Menu Głownego gry
        /// <see cref="MainWindow.MainWindow"/>
        /// </summary>
        /// <param name="sender">Obiekt, który wysłał zdarzenie</param>
        /// <param name="e">Argumenty zdarzenia, zawierające dodatkowe informacje o zdarzeniu.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Ukrywanie oryginalnego okna
            this.Visibility = Visibility.Hidden;

            // Tworzenie nowego okna i ustawienie jego położenia
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

    /// <summary>
    /// Klasa, która pozwala na zapis do obiektu oraz odczyt słowa wraz z tłumaczeniem
    /// </summary>
    public class DictionaryEntry
    {
        /// <summary>
        /// Metoda get/set dla polskiego słowa
        /// </summary>
        public string? PolishWord { get; set; }
        /// <summary>
        /// Metoda get/set dla tłumaczenia
        /// </summary>
        public string? EnglishTranslation { get; set; }

    }
}
