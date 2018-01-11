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

            Menu.AddOption("6", "Exporter la liste des clients au format XML", ExporterXml);

            Menu.AddOption("7", "Enregistrer les modifications", EnregistrerModifs);

            Menu.AddOption("8", "saisir une facture", SaisirUneFacture);
            Menu.AddOption("9", "saisir les lignes d'une facture", SaisirLigne);
            Menu.AddOption("10", "Mettre a jour la date et le mode de paiement d'une facture", MettreJourDateModePaiement);

        }

        private void MettreJourDateModePaiement()
        {
            int id = Input.Read<int>("Id du facture a mettre a jour :");
            var datePaiement = Input.Read<DateTime>("Saisir la date de paiement: ");
            var code = Input.Read<string>("Saisir le code de mode paiement :");

            AppGrandHotel.Instance.Contexte.UpdateDateModePaiement(id, datePaiement, code);

        }

        private void SaisirLigne()
        {
            LigneFacture ligneFacture = new LigneFacture();
            ligneFacture.IdFacture = Input.Read<int>("Id du facture :");  // a verifier si id se creer automatiquement
            ligneFacture.NumLigne = Input.Read<int>("Saisir le numero de ligne:");    //verifier id client
            ligneFacture.Quantite = Input.Read<Int16>("Saisir la quantite:");
            ligneFacture.MontantHT = Input.Read<decimal>("Saisir le montant HT: ");
            ligneFacture.TauxTVA = Input.Read<decimal>("Saisir le tauxTva :");
            ligneFacture.TauxReduction = Input.Read<decimal>("Saisir le tauxReduction :");

            AppGrandHotel.Instance.Contexte.AjouterLignesFacture(ligneFacture);
        }

        private void SaisirUneFacture()
        {
            Facture newFacture = new Facture();
            newFacture.Id = Input.Read<int>("Id du facture :");  // a verifier si id se creer automatiquement
            newFacture.IdClient = Input.Read<int>("Saisir l'Id de client:");    //verifier id client
            newFacture.DateFacture = Input.Read<DateTime>("Saisir la date de facture:");
            newFacture.DatePaiement = Input.Read<DateTime>("Saisir la date de paiement: ");
            newFacture.CodeModePaiement = Input.Read<string>("Saisir le code de mode paiement :");

            AppGrandHotel.Instance.Contexte.AjouterUneFacture(newFacture);
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
            //1 pour ajouter telephone, 2 pour ajouter email



            int choix = 0;
            while (choix != 1 && choix != 2)
            {
                choix = Input.Read<int>("1 pour ajouter un N° Téléphone 2 pour Ajouter une adresse email:");
                if (choix == 1)
                {
                    Telephone tele = new Telephone();


                    int idToCheck = Input.Read<int>("Id du client :");
                    tele.IdClient = AppGrandHotel.Instance.Contexte.CheckIdClient(idToCheck);

                    bool NumeroOK = false;
                    do
                    {
                        tele.Numero = Input.Read<string>("Saisir le N° téléphone à ajouter :");
                        if (tele.Numero.Length < 13)
                        {
                            NumeroOK = true;
                        }
                        else
                        {
                            Console.WriteLine("");
                        }
                    }
                    while (!NumeroOK);

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
