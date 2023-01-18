using BasketballLeagueApp.Models.Teams;
using System.Collections.Generic;

namespace BasketballLeagueApp.Services.Teams
{
    public interface ITeamService
    {
        AllTeamsQueryServiceModel All(string searchTerm = null,
            TeamSorting teamSorting = TeamSorting.Wins);

        DetailsTeamServiceModel GetDetailedTeam(int teamId);

        List<TopTeamServiceModel> GetTopOffensiveTeams();
        List<TopTeamServiceModel> GetTopDefensiveTeams();
    }
}
