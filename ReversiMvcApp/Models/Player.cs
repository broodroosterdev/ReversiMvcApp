using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Player
    {
        [Key]
        public string Guid { get; set; }
        public string Naam { get; set; }
        public string AantalGewonnen { get; set; }
        public string AantalVerloren { get; set; }
        public string AantalGelijk { get; set; }

    }
}
