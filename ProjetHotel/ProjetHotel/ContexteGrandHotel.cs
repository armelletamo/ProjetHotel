using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ProjetHotel
{
    public class ContexteGrandHotel : DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<Telephone> Telephones { get; set; }
        public DbSet<Adresse> Adresse { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<LigneFacture> LigneFactures { get; set; }

        public ContexteGrandHotel() : base("ProjetHotel.Settings1.GrandHotelConnect")
        {
            //turning off Proxy object creation on your DbContext
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #region Gestion des clients
        public IList<Client> GetListClient()
        {
            Client.OrderBy(c => c.Id).Load();
            return Client.Local.OrderBy(c => c.Id).ToList();
        }

        public IList<Client> GetCoordonneesClient()
        {
            return Client.Include(c => c.Adresse).Include(c => c.Telephones).Include(c => c.Emails).ToList();
        }

        public void AjoutClient(Client c)
        {
            Client.Add(c);

        }

        public void AjoutAdresseClient(Adresse a)
        {
            Adresse.Add(a);
        }

        public void AjouterTelephone(Telephone tele)
        {
            Telephones.Add(tele);
        }

        public void AjouterEmail(Email email)
        {
            Emails.Add(email);
        }

        //à revoir
        public void SupprimerUnClient(int id)
        {
            var client = Client.Find(id);
            var ad = Adresse.Find(id);
            var tel = Telephones.Where(t => t.IdClient == id);
            var em = Emails.Where(e => e.IdClient == id);

            Adresse.Remove(ad);
            Telephones.RemoveRange(tel);
            Emails.RemoveRange(em);
            Client.Remove(client);
        }

        public static void ExporterXML(List<Client> listeclient)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<Client>),
                                       new XmlRootAttribute("Clients"));

            using (var sw = new StreamWriter(@"..\..\Client.xml"))
            {
                serializer.Serialize(sw, listeclient);
            }
        }


        #endregion

        #region Gestion des factures
        public IList<Facture> GetListFactureClient(int z, DateTime? dateMin = null,
          DateTime? dateMax = null)
        {

            var fact = from f in Factures
                       where (f.IdClient == z)
                       select f;

            if (dateMin != null && dateMax != null)
            {
                fact = from f in fact
                       where (f.DateFacture >= dateMin && f.DateFacture < dateMax)
                       select f;
            }

            return fact.ToList();
        }

        public IList<LigneFacture> GetListligneFactureClient(int id)
        {
            LigneFactures.Where(l => l.IdFacture == id).Load();
            return LigneFactures.Local.Where(l => l.IdFacture == id).ToList();
        }

        public void AjouterUneFacture(Facture newFacture)
        {
            Factures.Add(newFacture);
        }

        public void AjouterLignesFacture(LigneFacture ligneFacture)
        {
            LigneFactures.Add(ligneFacture);
        }

        public void UpdateDateModePaiement(int id, DateTime datePaiement, string modePaiement)
        {
            var facture = Factures.Find(id);
            facture.DatePaiement = datePaiement;
            facture.CodeModePaiement = modePaiement;
        }

        public void ExporterXml_XmlWriter(int idClient)

        {
            var fac = Factures.Include(s => s.LigneFactures).Where(f => f.IdClient == idClient).ToList();

            // Définit les paramètres pour l'indentation du flux xml généré
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            // Utilisation d'un XmlWriter avec les paramètres définis précédemment pour écrire un fichier CollectionsBD_Writer.xml
            using (XmlWriter writer = XmlWriter.Create(@"..\..\Factures.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Factures");

                // Ecriture du contenu interne, avec une structure différente
                foreach (Facture f in fac)
                {
                    writer.WriteStartElement("Facture");
                    writer.WriteAttributeString("Id", f.Id.ToString());
                    writer.WriteAttributeString("IdClient", f.IdClient.ToString());
                    writer.WriteAttributeString("DateFacture", f.DateFacture.ToString());
                    writer.WriteAttributeString("DatePaiement", f.DatePaiement.ToString());
                    writer.WriteAttributeString("CodeModePaiement", f.CodeModePaiement.ToString());
                    decimal montant = f.LigneFactures.Sum(d => d.Quantite * d.MontantHT * (1 + d.TauxTVA) * (1 - d.TauxReduction));
                    montant = Math.Round(montant, 2);
                    writer.WriteAttributeString("Montant", montant.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public Facture GetUneFacture(int id)
        {
            return Factures.Find(id);
        }
        #endregion

        #region divers
        public void EnregistrerModifs()
        {
            SaveChanges();
        }

        //Vérification taille de string saisie par l'utilisateur
        public bool VerificationTaille(string s, int y)
        {
            return s.Length <= y;
        }

        #endregion
    }
}
