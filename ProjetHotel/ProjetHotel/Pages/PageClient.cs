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
           
        }

        private void AfficherListeClient()
        {

            _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();
           
            ConsoleTable.From(_listeClient, "Client").Display("Client");



        }
    }
}
