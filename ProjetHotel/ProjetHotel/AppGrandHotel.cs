using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{
    public class AppGrandHotel:ConsoleApplication
    {
        private static AppGrandHotel _instance;

        public static AppGrandHotel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppGrandHotel();

                return _instance;
            }
        }

        public AppGrandHotel()
        {
            // Définition des options de menu à ajouter dans tous les menus de pages
            MenuPage.DefaultOptions.Add(
                new Option("a", "Accueil", () => _instance.NavigateHome()));

            //MenuPage.DefaultOptions.Add(
            //	new Option("p", "Page précédente", () => _instance.NavigateBack()));
        }
    }
}
