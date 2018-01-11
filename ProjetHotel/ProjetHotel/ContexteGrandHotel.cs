using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IList<Client> GetListClient()
        {
            Client.OrderBy(c => c.Id).Load();
            return Client.Local.OrderBy(c => c.Id).ToList();
        }

        public IList<Client> GetCoordonneesClient()
        {
            return Client.Include(c => c.Adresse).Include(c => c.Telephones).Include(c => c.Emails).ToList();

        }

        public static void ExporterXML(List<Client> listeclient)
        {
            // On crée un sérialiseur, en spécifiant le type de l'objet à sérialiser
            // et le nom de l'élément xml racine
            XmlSerializer serializer = new XmlSerializer(typeof(List<Client>),
                                       new XmlRootAttribute("Clients"));

            using (var sw = new StreamWriter(@"..\..\Client.xml"))
            {
                serializer.Serialize(sw, listeclient);
            }
        }

        public void AjouterLignesFacture(LigneFacture ligneFacture)
        {
            LigneFactures.Add(ligneFacture);
        }

        public void AjouterUneFacture(Facture newFacture)
        {
            Factures.Add(newFacture);
        }

        public void AjouterTelephone(Telephone tele)
        {
            Telephones.Add(tele);
        }

        public void AjouterEmail(Email email)
        {
            Emails.Add(email);
        }

        public void UpdateDateModePaiement(int id, DateTime datePaiement, string modePaiement)
        {
            var facture = Factures.Find(id);
            facture.DatePaiement=datePaiement;
            facture.CodeModePaiement = modePaiement;
        }

        public void EnregistrerModifs()
        {
            SaveChanges();
        }

        public int CheckIdClient(int id)
        {
            bool idExiste = false;

            do
            {
                if (Client.Any(o => o.Id == id))
                {
                    idExiste = true;
                    return id;
                }
                else
                {
                    Console.WriteLine("Id n'existe pas");
                }
            }
            while (!idExiste);
            return -1;
        }

    }
}
