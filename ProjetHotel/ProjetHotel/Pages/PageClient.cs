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
               AjouterNouveauClient);
            Menu.AddOption("4", "Ajouter un téléphone ou adresser email à un client",
              AjouterTelephoneEmail);

            Menu.AddOption("6", "Exporter la liste des clients au format XML",
              ExporterXml);

            Menu.AddOption("7", "Enregistrer les modifications", EnregistrerModifs);
        }

        private void ExporterXml()
        {
            List<Client> listeClient;
            listeClient = AppGrandHotel.Instance.Contexte.GetListClient().ToList();
            ContexteGrandHotel.ExporterXML(listeClient);
        }

        private void EnregistrerModifs()
        {
            AppGrandHotel.Instance.Contexte.EnregistrerModifs();
        }

        private void AjouterNouveauClient()
        {
            throw new NotImplementedException();
        }

        private void AfficherListeClient()

        {
            _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();
            ConsoleTable.From(_listeClient, "Client").Display("Client");
        }

        private void AjouterTelephoneEmail()
        {
            int choix = 0;
            while (choix != 1 && choix != 2)
            {
                choix = Input.Read<int>("1 pour ajouter un N° Téléphone 2 pour Ajouter une adresse email:");
                if (choix == 1)
                {
                    Telephone tele = new Telephone();
                    tele.IdClient = Input.Read<int>("Id du client :");//vérifier id client existe

                    //int longueurNumero = 0;
                    //while (longueurNumero>12 && longueurNumero==0)
                    //{   

                    tele.Numero = Input.Read<String>("Saisir le N° téléphone à ajouter :");   //vérifier longueur saisie
                    tele.CodeType = Input.Read<String>("Saisir le Type de numéro F pour fixe ou M pour mobile :");
                    tele.Pro = Input.Read<bool>("Saisir le Type de numéro : personnel: false ,  pro: true ");

                    AppGrandHotel.Instance.Contexte.AjouterTelephone(tele);
                }

                else if (choix == 2)
                {
                    Email email = new Email();
                    email.IdClient = Input.Read<int>("Id du client :");
                    email.Adresse = Input.Read<String>("Saisir l'adresse email:");
                    email.Pro = Input.Read<bool>("Saisir le Type de numéro : personnel: false ,  pro: true ");
                    AppGrandHotel.Instance.Contexte.AjouterEmail(email);

                }
            }


        }

        
        private void AfficherCoordonneClient()
        {
            int id = Input.Read<int>("Id du client :");
            // var c = AppGrandHotel.Instance.Contexte.Client.Where(p => p.Id == id).Get


            var coordonneesClient = AppGrandHotel.Instance.Contexte.GetCoordonneesClient().Where(p => p.Id == id).FirstOrDefault();
            var adresse = coordonneesClient.Adresse;
            var telephones = coordonneesClient.Telephones;


            var emails = coordonneesClient.Emails;

            Console.WriteLine("Adresse: {0}", adresse.ToString());
            //ConsoleTable.From(adresse,"adresse").Display("Adresse");
            ConsoleTable.From(telephones, "Telephones").Display("Telephones");
            ConsoleTable.From(emails, "Emails").Display("Emails");

        }
    }
}
