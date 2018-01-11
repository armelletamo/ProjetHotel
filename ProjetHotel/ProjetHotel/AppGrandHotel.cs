
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outils.TConsole;

namespace ProjetHotel
{
    class AppGrandHotel: ConsoleApplication
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
        
        // Constructeur
        public AppGrandHotel()
        {
            // Définition des options de menu à ajouter dans tous les menus de pages
            MenuPage.DefaultOptions.Add(
               new Option("a", "Accueil", () => _instance.NavigateHome()));
        }
    }
}
