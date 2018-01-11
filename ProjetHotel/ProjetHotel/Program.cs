using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetHotel.Pages;

namespace ProjetHotel
{
    class Program
    {
        static void Main(string[] args)
        {
            AppGrandHotel app = AppGrandHotel.Instance;
            app.Title = "Grand Hotel";

            // Ajout des pages
            Page Accueil = new PageAccueil();
            app.AddPage(Accueil);

            Page Client  = new PageClient();
            app.AddPage(Client);




            // Affichage de la page d'accueil
            app.NavigateTo(Accueil);

            app.NavigateTo(Client);

        }
    }
}
