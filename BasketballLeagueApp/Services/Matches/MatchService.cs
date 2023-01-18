using BasketballLeagueApp.Data;
using BasketballLeagueApp.Data.Models;
using BasketballLeagueApp.Models.Matches;
using System.Collections.Generic;
using System.Linq;

namespace BasketballLeagueApp.Services.Matches
{
	public class MatchService : IMatchService
	{
		private readonly BasketballLeagueAppDbContext data;
		public MatchService(BasketballLeagueAppDbContext data)
		{
			this.data = data;
		}
		public List<MatchesServiceModel> GetAllMatches()
		{
			var matches = GetAllMatchesFromDatabase();
			var teams = data.Teams.ToList();

			var result = new List<MatchesServiceModel>();

			foreach (var match in matches)
			{
				var homeTeam = teams.FirstOrDefault(x => x.Id == match.HomeTeamId);
				var awayTeam = teams.FirstOrDefault(x => x.Id == match.AwayTeamId);


				var matchModel = new MatchesServiceModel()
				{
					HomeTeamName = homeTeam.Name,
					HomeTeamImage = homeTeam.TeamImage,
					HomeTeamScore = match.HomeTeamScore,
					AwayTeamName = awayTeam.Name,
					AwayTeamImage = awayTeam.TeamImage,
					AwayTeamScore = match.AwayTeamScore
				};

				result.Add(matchModel);
			}

			return result;
		}

		public List<Match> GetAllMatchesFromDatabase()
		{
            return data.Matches.ToList();
        }

        public MatchesServiceModel GetHighlightedMatch()
		{
			var matches = GetAllMatchesFromDatabase();

			int totalMatchScore = 0;
			int matchId = 0;

			for (int i = 0; i < matches.Count; i++)
			{
				var totalScore = matches[i].HomeTeamScore + matches[i].AwayTeamScore;

				if (totalScore > totalMatchScore)
				{
					totalMatchScore = totalScore;
					matchId = i;
				}
			}

			var homeTeam = data.Teams.FirstOrDefault(x => x.Id == matches[matchId].HomeTeamId);
			var awayTeam = data.Teams.FirstOrDefault(x => x.Id == matches[matchId].AwayTeamId);

			return new MatchesServiceModel()
			{
				HomeTeamImage = homeTeam.TeamImage,
				HomeTeamName = homeTeam.Name,
				HomeTeamScore = matches[matchId].HomeTeamScore,
				AwayTeamImage = awayTeam.TeamImage,
				AwayTeamName = awayTeam.Name,
				AwayTeamScore = matches[matchId].AwayTeamScore,
			};
		}
	}
}
