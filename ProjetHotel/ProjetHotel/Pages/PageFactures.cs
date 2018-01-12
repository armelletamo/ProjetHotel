using Outils.TConsole;
using ProjetHotel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{
    class Factures : MenuPage
    {
        private IList<Client> _listeClient;
        private IList<Facture> _listeFacture;
        private IList<LigneFacture> _listeLigneFacture;
        private Facture _newFacture;
        private LigneFacture _ligneFacture;

        public Factures() : base("Gestion des Facture ", false)
        {
            Menu.AddOption("1", "Afficher la liste de facture d'un client", AfficherFactureClient);
            Menu.AddOption("2", "Afficher les lignes de facture d'un client", AfficherLignesFactureClient);
            Menu.AddOption("3", "Saisir une facture", SaisirUneFacture);
            Menu.AddOption("4", "Saisir les lignes d'une facture donnée", SaisirLigne);
            Menu.AddOption("5", "Mettre a jour la date et le mode de paiement d'une facture", MettreJourDateModePaiement);
            Menu.AddOption("6", "Exporter la liste des factures au format XML", ExporterXml);
            Menu.AddOption("7", "Enregistrer les modifications", Enregistrer);

            _newFacture = new Facture();
            _ligneFacture = new LigneFacture();
        }


        private void AfficherFactureClient()
        {
            _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();

            ConsoleTable.From(_listeClient, "Client").Display("Client");

            int i = 0;
            bool idClientOk = false;
            while (!idClientOk)
            {

                i = Input.Read<int>("Id du client pour afficher sa liste de facture :");
                idClientOk = _listeClient.Where(a => a.Id == i).Any();
            }
            DateTime date = Input.Read<DateTime>("Entrez la date des factures sous la forme jj/mm/aaaa: ");


            _listeFacture = AppGrandHotel.Instance.Contexte.GetListFactureClient(i, date.Date, date.Date.AddYears(1));
            ConsoleTable.From(_listeFacture, "Facture").Display("Facture");
        }

        private void AfficherLignesFactureClient()
        {
            int id = 0;
            do
            {
                id = Input.Read<int>("Saisir Id de la facture:");
                _newFacture = AppGrandHotel.Instance.Contexte.GetUneFacture(id);
            }
            while (_newFacture == null);

            _listeLigneFacture = AppGrandHotel.Instance.Contexte.GetListligneFactureClient(id);
            ConsoleTable.From(_listeLigneFacture, "LigneFacture").Display("LigneFacture");

        }


        private void SaisirUneFacture()
        {
            Facture newFacture = new Facture();

            //vérifier si Id client existe déjà
            int id = 0;
            bool idClientOk = false;
            while (!idClientOk)
            {
                id = Input.Read<int>("Id du client :");
                idClientOk = AppGrandHotel.Instance.Contexte.Factures.Where(a => a.IdClient == id).Any();
            }
            newFacture.IdClient = id;


            newFacture.DateFacture = Input.Read<DateTime>("Saisir la date de facture:");

            bool cmp = false;
            while (!cmp)
            {
                newFacture.CodeModePaiement = (Input.Read<string>("Saisir le code de mode paiement entre CB, CHQ et ESP: ")).ToUpper();
                cmp = newFacture.CodeModePaiement == "CB" || newFacture.CodeModePaiement == "CHQ" || newFacture.CodeModePaiement == "ESP";

            }
            Output.WriteLine(ConsoleColor.Red, " La facture est créé avec succès ");
            AppGrandHotel.Instance.Contexte.AjouterUneFacture(newFacture);
        }

        private void SaisirLigne()
        {
            do
            {
                int id = Input.Read<int>("Saisir Id de la facture:");
                _newFacture = AppGrandHotel.Instance.Contexte.GetUneFacture(id);
            }
            while (_newFacture == null);

            _ligneFacture.IdFacture = _newFacture.Id;
            _listeLigneFacture = AppGrandHotel.Instance.Contexte.GetListligneFactureClient(_ligneFacture.IdFacture);
            _ligneFacture.NumLigne = _listeLigneFacture.Count() + 1;
            _ligneFacture.Quantite = Input.Read<Int16>("Saisir la quantite:");
            _ligneFacture.MontantHT = Input.Read<decimal>("Saisir le montant HT: ");
            _ligneFacture.TauxTVA = Input.Read<decimal>("Saisir le tauxTva :");
            _ligneFacture.TauxReduction = Input.Read<decimal>("Saisir le tauxReduction :");

            AppGrandHotel.Instance.Contexte.AjouterLignesFacture(_ligneFacture);
            Output.WriteLine(ConsoleColor.Red, " La ligne de facture est créé avec succès ");
        }

        private void MettreJourDateModePaiement()
        {
            int id = 0;
            do
            {
                id = Input.Read<int>("Saisir Id du facture a mettre a jour :");
                _newFacture = AppGrandHotel.Instance.Contexte.GetUneFacture(id);
            }
            while (_newFacture == null);
            
            var datePaiement = Input.Read<DateTime>("Saisir la date de paiement: ");

            bool cmp = false;
            while (!cmp)
            {
                _newFacture.CodeModePaiement = (Input.Read<string>("Saisir le code de mode paiement entre CB, CHQ et ESP: ")).ToUpper();
                cmp = _newFacture.CodeModePaiement == "CB" || _newFacture.CodeModePaiement == "CHQ" || _newFacture.CodeModePaiement == "ESP";

            }
            Output.WriteLine(ConsoleColor.Red, " La facture est créé avec succès ");
            AppGrandHotel.Instance.Contexte.AjouterUneFacture(_newFacture);

            AppGrandHotel.Instance.Contexte.UpdateDateModePaiement(id, datePaiement, _newFacture.CodeModePaiement);

        }

        private void ExporterXml()
        {
            int id = 0;
            bool idClientOk = false;
            while (!idClientOk)
            {
                _listeClient = AppGrandHotel.Instance.Contexte.GetListClient();
                id = Input.Read<int>("Id de client pour la facture à exporter:");
                idClientOk = _listeClient.Where(a => a.Id == id).Any();
            }

            //var listeFacture = AppGrandHotel.Instance.Contexte.GetListFactureClient(id).ToList();

            AppGrandHotel.Instance.Contexte.ExporterXml_XmlWriter(id);
            Output.WriteLine(ConsoleColor.Red, "Le ficher facture XML est crée ");
            Console.Read();
        }

        private void Enregistrer()
        {
            AppGrandHotel.Instance.Contexte.EnregistrerModifs();
        }

    }

}
