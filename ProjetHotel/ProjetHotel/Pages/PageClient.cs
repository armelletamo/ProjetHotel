using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel.Pages
{
    class PageClient : MenuPage
    {

        IList<Client> _listeClient;

        public PageClient() : base("Gestion de Client", false)
        {
            Menu.AddOption("1", "liste de Client",
                AfficherListeClient);
            Menu.AddOption("2", "Afficher les coordonnées d'un client",
                AfficherCoordonneClient);
            Menu.AddOption("3", "Ajouter un nouveau client",
               AjouterClient);

        }

        //private void AjouterClient()
        //{
        //    if (_listeClient == null)
        //        _listeClient = AppGrandHotel.Instance.Contexte.AfficherListeClient();
        //    ConsoleTable.From(_listeClient).Display("catégories");
        //}

     

        private void AfficherCoordonneClient()
        {   
            int id = Input.Read<int>("Id du client :");
            var coordonnees =AppGrandHotel.Instance.Contexte.AfficherCoordonneesClient(id);
            ConsoleTable.From(coordonnees).Display("Coordonnées");
        private void AfficherListeClient()
        {

            _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();
           
            ConsoleTable.From(_listeClient, "Client").Display("Client");



        }
    }
}
