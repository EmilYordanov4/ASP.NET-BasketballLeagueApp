using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasketballLeagueApp.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string TeamImage { get; set; }

        public List<Player> Roster {get; set;}  

        public int Matches { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }
    }
}
