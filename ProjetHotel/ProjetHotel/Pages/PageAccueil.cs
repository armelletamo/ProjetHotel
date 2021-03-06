﻿using Outils.TConsole;
using ProjetHotel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{

    class PageAccueil : MenuPage
    {
        public PageAccueil() : base("Accueil", false)
        {
            Menu.AddOption("0", "Quitter l'application",
                () => Environment.Exit(0));
            Menu.AddOption("1", "Gestion des client",
                 () => AppGrandHotel.Instance.NavigateTo(typeof(PageClient)));
            Menu.AddOption("2", "Gestion des facture",
                () => AppGrandHotel.Instance.NavigateTo(typeof(Factures)));

        }

    }
}
