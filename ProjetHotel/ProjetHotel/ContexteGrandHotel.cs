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

        public  ContexteGrandHotel() : base("ProjetHotel.Settings1.GrandHotelConnect")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public  IList<Client> GetListClient()
        {

            Client.OrderBy(c => c.Id).Load();
            return Client.Local.OrderBy(c => c.Id).ToList();
        }



    }
}
