﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{
    class Contexte:DbContext
    {
        public DbSet<Client> Client { get; set; }

        public Contexte() : base("ProjetHotel.Properties.Settings1.GrandHotel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }






    }
}
