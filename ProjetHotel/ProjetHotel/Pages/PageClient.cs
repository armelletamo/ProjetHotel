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
        public PageClient() : base("Page Client", false)
        {
            Menu.AddOption("1", "Liste de Client",
                AfficherListeClient);
           
        }

        private void AfficherListeClient()
        {
            
        }
    }
}
