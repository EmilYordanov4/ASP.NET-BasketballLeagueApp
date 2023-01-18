using BasketballLeagueApp.Data.Models;
using BasketballLeagueApp.Models.Players;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasketballLeagueApp.Models.Teams
{
    public class TeamServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TeamImage { get; set; }

        public int Matches { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }
    }
}
