using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Przestrzen projektowa gry
/// </summary>
namespace Development
{
    /// <summary>
    /// Klasa przechowująca słowa z danej kategorii, wraz z ich tłumaczeniem na język angielski
    /// </summary>
    public class Slowa
    {
        /// <summary>
        /// Zmienna tekstowa przechowująca dane słowo z tablicy
        /// </summary>
        private string slowo_pl = "";
        /// <summary>
        /// Zmienna tekstowa przechowująca dane słowo z tablicy
        /// </summary>
        private string slowo_en = "";

        /// <summary>
        /// Konstruktor klasy, który zapisuje w obiekcie wylosowane słowo, wraz z tłumaczeniem, w celu łatwiejszego dostępu
        /// <see cref="Game.Losuj_slowa(string[], string[])"/>
        /// </summary>
        /// <param name="slowo_pl">Słowo polskie przekazywane przez wartość</param>
        /// <param name="slowo_en">Słowo angielskie przekazywane przez wartość</param>
        public Slowa(string slowo_pl, string slowo_en)
        {
            this.slowo_en = slowo_en;
            this.Slowo_pl = slowo_pl;
        }

        /// <summary>
        /// Metoda get/set, która pozwala odczytać słowo polskie zapisane w obiekcie
        /// </summary>
        public string Slowo_pl { get { return slowo_pl; } set { slowo_pl = value; } }

        /// <summary>
        /// Metoda get/set, która pozwala odczytać angielskie tłumaczenie słowa zapisane w obiekcie
        /// </summary>
        public string Slowo_en { get { return slowo_en; } set { slowo_en = value; } }
    }
}
