using BasketballLeagueApp.Models.Teams;
using BasketballLeagueApp.Services.Teams;
using Microsoft.AspNetCore.Mvc;

namespace BasketballLeagueApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teams;
        public TeamController(ITeamService teams)
        {
            this.teams = teams;
        }

        public IActionResult All([FromQuery] AllTeamsQueryServiceModel query)
        {
            var queryResults = this.teams.All(query.SearchTerm, query.Sorting);

            query.Teams = queryResults.Teams;

            return View(query);
        }

        public IActionResult Details(int teamId)
        {
            var teamDetails = teams.GetDetailedTeam(teamId);

            return View(teamDetails);
        }

        public IActionResult Offense()
        {
            return View(teams.GetTopOffensiveTeams());
        }

        public IActionResult Defense()
        {
            return View(teams.GetTopDefensiveTeams());
        }
    }
}
