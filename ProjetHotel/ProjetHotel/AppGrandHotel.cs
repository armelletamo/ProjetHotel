
using Outils.TConsole;

namespace ProjetHotel
{
    public class AppGrandHotel : ConsoleApplication
    {
        private static AppGrandHotel _instance;
        public ContexteGrandHotel Contexte { get; set; }

        /// <summary>
        /// Obtient l'instance unique de l'application
        /// </summary>
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
            Contexte = new ContexteGrandHotel();
        }
    }
}

