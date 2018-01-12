using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel.Pages
{
    class PageClient : MenuPage
    {

        private IList<Client> _listeClient;
        private Client _client;
        private Adresse _adresse;
        private Telephone _telephone;
        private Email _email;


        public PageClient() : base("Gestion de Client", false)
        {
           
            Menu.AddOption("1", "Liste de client", AfficherListeClient);
            Menu.AddOption("2", "Afficher les coordonnées d'un client", AfficherCoordonneClient);
            Menu.AddOption("3", "Ajouter nouveau client et adresse", AjouterClientAdresse);
            Menu.AddOption("4", "Ajouter un téléphone ou adresser email à un client", AjouterTelephoneEmail);
            Menu.AddOption("5", "Supprimer un client", SupprimerClient);
            Menu.AddOption("6", "Exporter la liste des clients au format XML", ExporterXml);
            Menu.AddOption("7", "Enregistrer les modifications", Enregistrer);

            _client = new Client();
            _adresse = new Adresse();
            _telephone = new Telephone();
            _email = new Email();

        }

        private void AfficherListeClient()
        {
            _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();
            ConsoleTable.From(_listeClient, "Client").Display("Client");
        }

        private void AfficherCoordonneClient()
        {
            //vérifier si l'Id saisie existe
            int id = 0;
            bool idClientOk = false;
            while (!idClientOk)
            {
                AfficherListeClient();
                id = Input.Read<int>("Id du client :");
                idClientOk = _listeClient.Where(a => a.Id == id).Any();
            }

            //recuperer le client selon Id saisie et ses coordonnées
            var coordonneesClient = AppGrandHotel.Instance.Contexte.GetCoordonneesClient().Where(p => p.Id == id).FirstOrDefault();
            _adresse = coordonneesClient.Adresse;
            var telephones = coordonneesClient.Telephones;
            var emails = coordonneesClient.Emails;

            Console.WriteLine("Adresse: {0}", _adresse.ToString());
            ConsoleTable.From(telephones, "Telephones").Display("Telephones");
            ConsoleTable.From(emails, "Emails").Display("Emails");

        }

        //Ajouter un nouveau client et son adresse
        private void AjouterClientAdresse()
        {
            //Vérification de longueur saisie 
            bool civilite = false;
            while (!civilite)
            {
                _client.Civilite = (Input.Read<string>("Choisissez la civilite: M, MME  ")).ToUpper();
                civilite = _client.Civilite == "M" || _client.Civilite == " MME";
            }

            bool nom = false;
            while (!nom)
            {
                _client.Nom = Input.Read<string>("Saississez le nom du client: ");
                nom = AppGrandHotel.Instance.Contexte.VerificationTaille(_client.Nom, 40);
            }

            bool prenom = false;
            while (!prenom)
            {
                _client.Prenom = Input.Read<string>("Saissisez le prenom du client: ");
                prenom = AppGrandHotel.Instance.Contexte.VerificationTaille(_client.Prenom, 40);
            }

            _client.CarteFidelite = Input.Read<bool>("Le client a t-il une carte fidelité: tapez true pour oui et false pour non ");
            _client.Societe = Input.Read<string>("Saissisez le nom de la société du client: ");
            AppGrandHotel.Instance.Contexte.AjoutClient(_client);
            Output.WriteLine(ConsoleColor.Red, "Client créé avec succès ");


            bool rue = false;
            while (!rue)
            {
                _adresse.Rue = Input.Read<string>("Saississez la rue: ");
                rue = AppGrandHotel.Instance.Contexte.VerificationTaille(_adresse.Rue, 40);
            }

            _adresse.Complement = Input.Read<string>("Saississez le complement d'adresse: ");

            bool codePostal = false;
            while (!codePostal)
            {
                _adresse.CodePostal = Input.Read<string>("Saississez le code postal: ");
                codePostal = AppGrandHotel.Instance.Contexte.VerificationTaille(_adresse.CodePostal, 5);
            }

            bool ville = false;
            while (!ville)
            {
                _adresse.Ville = Input.Read<string>("Saississez la ville: ");
                ville = AppGrandHotel.Instance.Contexte.VerificationTaille(_adresse.Ville, 40);
            }

            AppGrandHotel.Instance.Contexte.AjoutAdresseClient(_adresse);
            Output.WriteLine(ConsoleColor.Red, " adresse créé avec succès ");
            Output.WriteLine(ConsoleColor.DarkYellow, " Veuillez enregistrer votre saisie. Pour enregistrer, appuyer 7");

        }

        private void AjouterTelephoneEmail()
        {
            //1 pour ajouter un numéro telephone, 2 pour ajouter une adresse email

            int choix = 0;
            while (choix != 1 && choix != 2)
            {
                choix = Input.Read<int>("1 pour ajouter un N° Téléphone 2 pour Ajouter une adresse email:");

                if (choix == 1)
                {
                    //check Id existe
                    int id = 0;
                    bool idClientOk = false;
                    AfficherListeClient();
                    while (!idClientOk)
                    {
                        id = Input.Read<int>("Id du client :");
                        idClientOk = _listeClient.Where(a => a.Id == id).Any();
                    }
                    _telephone.IdClient = id;

                    bool NumeroOK = false;
                    do
                    {
                        _telephone.Numero = Input.Read<string>("Saisir le N° téléphone à ajouter :");
                        if (_telephone.Numero.Length < 13)
                        {
                            NumeroOK = true;
                        }
                        else
                        {
                            Console.WriteLine("");
                        }
                    }
                    while (!NumeroOK);

                    bool codeType = false;
                    while (!codeType)
                    {
                        _client.Civilite = (Input.Read<string>("Saisir le Type de numéro F pour fixe ou M pour mobile : ")).ToUpper();
                        codeType = _client.Civilite == "F" || _client.Civilite == " M";

                    }

                    _telephone.CodeType = _client.Civilite.ToUpper();
                    _telephone.Pro = Input.Read<bool>("Saisir le Type de numéro : personnel: false ,  pro: true ");

                    AppGrandHotel.Instance.Contexte.AjouterTelephone(_telephone);
                    Output.WriteLine(ConsoleColor.Red, "Le numéro téléphone est ajouté! ");
                }

                else if (choix == 2)
                {
                    AfficherListeClient();
                    int id = 0;
                    bool idClientOk = false;
                    while (!idClientOk)
                    {
                        AfficherListeClient();
                        id = Input.Read<int>("Id du client :");
                        idClientOk = _listeClient.Where(a => a.Id == id).Any();
                    }
                    _email.IdClient = id;

                    _email.Adresse = Input.Read<String>("Saisir l'adresse email:");
                    _email.Pro = Input.Read<bool>("Saisir le Type de numéro : personnel: false ,  pro: true ");
                    AppGrandHotel.Instance.Contexte.AjouterEmail(_email);
                    Output.WriteLine(ConsoleColor.Red, "L'adresse email est ajouté! ");
                }
            }
        }


        private void SupprimerClient()
        {

            int i = Input.Read<int>("Id du client à supprimer :");

            AppGrandHotel.Instance.Contexte.SupprimerUnClient(i);

        }

        private void Enregistrer()
        {

            try
            {
                AppGrandHotel.Instance.Contexte.EnregistrerModifs();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                var innerEx = (ex.InnerException.InnerException as SqlException);
                if (innerEx != null && innerEx.Number == 547)
                    GererErreurSql(innerEx);
            }
            catch (SqlException ex)
            {
                GererErreurSql(ex);
            }

        }

        private void GererErreurSql(SqlException ex)
        {
            if (ex.Number == 547)
                Output.WriteLine(ConsoleColor.Red, "le client ne peut être supprimé car il est reférencer par une facture");
            else
                throw ex;
        }


        private void ExporterXml()
        {
            var listeClient = AppGrandHotel.Instance.Contexte.GetListClient().ToList();

            ContexteGrandHotel.ExporterXML(listeClient);
            Output.WriteLine(ConsoleColor.Red, "Le ficher client XML est crée!");
        }

        private void EnregistrerModifs()
        {
            AppGrandHotel.Instance.Contexte.EnregistrerModifs();
        }

    }
}
