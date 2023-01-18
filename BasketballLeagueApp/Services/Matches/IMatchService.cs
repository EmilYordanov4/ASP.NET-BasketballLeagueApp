using BasketballLeagueApp.Data.Models;
using BasketballLeagueApp.Models.Matches;
using System.Collections.Generic;

namespace BasketballLeagueApp.Services.Matches
{
	public interface IMatchService
	{
		List<MatchesServiceModel> GetAllMatches();

		MatchesServiceModel GetHighlightedMatch();

		List<Match> GetAllMatchesFromDatabase();
	}
}
