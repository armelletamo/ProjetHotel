﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjetHotel
{

    public class Client
    {
        [Key]
        [XmlAttribute]
        public int Id { get; set; }
        [MaxLength(4)]
        [XmlAttribute]
        public string Civilite { get; set; }
        [XmlAttribute]
        public string Nom { get; set; }
        [XmlAttribute]
        public string Prenom { get; set; }
        [XmlAttribute]
        public bool CarteFidelite { get; set; }
        [XmlAttribute]
        public string Societe { get; set; }
        //Propriété de navigation
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Adresse Adresse { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual List<Telephone> Telephones { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual List<Email> Emails { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual List<Facture> Factures { get; set; }
    }

    public class Facture
    {
        [Key]
        [Display(ShortName = "IdFacture")]
        public int Id { get; set; }
        [ForeignKey("Client")]
        [Display(ShortName = "None")]
        public int IdClient { get; set; }
        public DateTime DateFacture { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePaiement { get; set; }
        [MaxLength(3)]
        public string CodeModePaiement { get; set; }
        [Display(ShortName = "None")]
        public virtual List<LigneFacture> LigneFactures { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual Client Client { get; set; }
       
    }

    public class LigneFacture
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Facture")]
        [Display(ShortName = "None")]
        public int IdFacture { get; set; }
        [Key]
        [Column(Order = 1)]
        public int NumLigne { get; set; }
        public Int16 Quantite { get; set; }
        public decimal MontantHT { get; set; }
        public decimal TauxTVA { get; set; }
        public decimal TauxReduction { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual Facture Facture { get; set; }
    }


    public class Adresse
    {
        [Key]
        [ForeignKey("Client")]
        [XmlAttribute]
        public int IdClient { get; set; }
        [XmlAttribute]
        public string Rue { get; set; }
        [XmlAttribute]
        public string Complement { get; set; }
        [MaxLength(5)]
        [XmlAttribute]
        public string CodePostal { get; set; }
        [XmlAttribute]
        public string Ville { get; set; }
        // Propriété de navigation
        [XmlIgnore]
        public virtual Client Client { get; set; }

        public override string ToString()
        {
            return IdClient + Rue + "  " + Complement + "  " + CodePostal + "  " + Ville;
        }

    }

    public class Telephone
    {
        [Key]
        [MaxLength(12)]
        [XmlAttribute]
        public string Numero { get; set; }
        [ForeignKey("Client")]
        [Display(ShortName = "None")]
        [XmlAttribute]
        public int IdClient { get; set; }
        [XmlAttribute]
        public string CodeType { get; set; }
        [XmlAttribute]
        public bool Pro { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Client Client { get; set; }
    }

    public class Email
    {
        [Key]
        [XmlAttribute]
        public string Adresse { get; set; }
        [ForeignKey("Client")]
        [XmlAttribute]
        [Display(ShortName = "None")]
        public int IdClient { get; set; }
        [XmlAttribute]
        public bool Pro { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Client Client { get; set; }

        public override string ToString()
        {
            return Adresse + "  " + Pro;
        }
    }

}
