using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{
    public class ContexteGrandHotel:DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<Adresse> Adresse { get; set; }
        public DbSet<Email> Email { get; set; }

        public ContexteGrandHotel() : base("ProjetHotel.Properties.Settings1.GrandHotel")
        {
           

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public IList<string> AfficherCoordonneesClient(int idClient)
        {
            var param = new System.Data.SqlClient.SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.NVarChar,
                ParameterName = "@id",
                Value = idClient
            };

            var result=Database.SqlQuery<string>(@"select a.CodePostal,t.Numero, e.Adresse
                                                from client c
                                                inner join Adresse a on a.IdClient=c.Id
                                                inner join Telephone t on t.IdClient=c.Id
                                                inner join Email e on e.IdClient=c.id
                                                where c.id=@id", param).ToList();

            return result;

        }


        public void AjouterUnNouveauClient(Client newClient)
        {
            Client.Add(newClient);
        }

    }
}
