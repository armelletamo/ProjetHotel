using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel
{




    public class Client
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(4)]
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public bool CarteFidelite { get; set; }
        public string Societe { get; set; }
        //Propriété de navigation
        [Display(ShortName = "None")]
        public virtual Adresse Adresse { get; set; }
        [Display(ShortName = "None")]
        public virtual List<Telephone> Telephones { get; set; }
        [Display(ShortName = "None")]
        public virtual List<Email> Emails { get; set; }
    }

    public class Adresse
    {
        [Key]
        [ForeignKey("Client")]
        public int IdClient { get; set; }
        public string Rue { get; set; }
        public string Complement { get; set; }
        [MaxLength(5)]
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        // Propriété de navigation
        public virtual Client Client { get; set; }

        public override string ToString()
        {
            return IdClient + Rue +"  "+ Complement + "  "+CodePostal +"  " +Ville;
        }

    }

    public class Telephone
    {
        [Key]
        [MaxLength(12)]
        public string Numero { get; set; }
        [ForeignKey("Client")]
        [Display(ShortName = "None")]
        public int IdClient { get; set; }
        public string CodeType { get; set; }
        public bool Pro { get; set; }
        [Display(ShortName = "None")]
        public virtual Client Client { get; set; }
    }

    public class Email
    {
        [Key]
        public string Adresse { get; set; }
        [ForeignKey("Client")]
        public int IdClient { get; set; }
        public bool Pro { get; set; }
        public virtual Client Client { get; set; }
    }




}
