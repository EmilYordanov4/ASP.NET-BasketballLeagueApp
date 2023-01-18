using BasketballLeagueApp.Models.Players;
using System.Collections.Generic;

namespace BasketballLeagueApp.Models.Teams
{
    public class DetailsTeamServiceModel
    {
        public TeamServiceModel Team { get; set; }

        public List<PlayerInfoServiceModel> Players { get; set; }
    }
}
