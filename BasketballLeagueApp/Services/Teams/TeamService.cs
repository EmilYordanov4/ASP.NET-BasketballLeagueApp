using BasketballLeagueApp.Data;
using BasketballLeagueApp.Models.Players;
using BasketballLeagueApp.Models.Teams;
using BasketballLeagueApp.Services.Matches;
using System.Collections.Generic;
using System.Linq;

namespace BasketballLeagueApp.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly BasketballLeagueAppDbContext data;
        private readonly IMatchService matchService;

        public TeamService(BasketballLeagueAppDbContext data, IMatchService matchService)
        {
            this.data = data;
            this.matchService = matchService;
        }

        public AllTeamsQueryServiceModel All(string searchTerm = null, TeamSorting teamSorting = TeamSorting.Wins)
        {
            var teamsAsQuery = data.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teamsAsQuery = teamsAsQuery
                    .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            teamsAsQuery = teamSorting switch
            {
                TeamSorting.Loses => teamsAsQuery.OrderByDescending(x => x.Loses),
                TeamSorting.Matches => teamsAsQuery.OrderByDescending(x => x.Matches),
                _ => teamsAsQuery.OrderByDescending(x => x.Wins),
            };

            var allTeams = teamsAsQuery
                    .Select(x => new TeamServiceModel 
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Matches = x.Matches,
                        Loses = x.Loses,
                        Wins = x.Wins,
                        TeamImage = x.TeamImage
                    });

            return new AllTeamsQueryServiceModel
            {
                Teams = allTeams
            };
        }

        public DetailsTeamServiceModel GetDetailedTeam(int teamId)
        {
            var teamInfo = data
                    .Teams
                    .FirstOrDefault(x => x.Id == teamId);

            var team = new TeamServiceModel
            {
                Id = teamInfo.Id,
                Name = teamInfo.Name,
                Matches = teamInfo.Matches,
                TeamImage = teamInfo.TeamImage,
                Wins = teamInfo.Wins,
                Loses = teamInfo.Loses
            };

            var players = data
                    .Players
                    .Where(x => x.TeamId == teamId)
                    .Select(x => new PlayerInfoServiceModel 
                    {
                        Id = x.Id,
                        Age = x.Age,
                        Height = x.Height,
                        Weight = x.Weight,
                        Image = x.Image,
                        Name = x.Name,
                        Position = x.Position,
                    })
                    .ToList();

            var detailedTeam = new DetailsTeamServiceModel
            {
                Team = team,
                Players = players
            };

            return detailedTeam;
        }

        public List<TopTeamServiceModel> GetTopDefensiveTeams()
        {
            var teams = GetTotalScoredPointsByOpponent().OrderBy(x => x.Value).Take(5);

            var result = new List<TopTeamServiceModel>();

            foreach (var team in teams)
            {
                result.Add(data
                    .Teams
                    .Where(x => x.Id == team.Key)
                    .Select(x => new TopTeamServiceModel 
                    {
                        Id = x.Id,
                        Score = team.Value,
                        Name = x.Name,
                        TeamImage = x.TeamImage,
                    })
                    .FirstOrDefault());
            }

            return result;
        }

        private Dictionary<int,int> GetTotalScoredPoints()
        {
            var matches = matchService.GetAllMatchesFromDatabase();
            var teamScore = new Dictionary<int, int>();

            foreach (var match in matches)
            {
                if (!teamScore.ContainsKey(match.HomeTeamId))
                {
                    teamScore.Add(match.HomeTeamId, match.HomeTeamScore);
                }
                else
                {
                    teamScore[match.HomeTeamId] += match.HomeTeamScore;
                }
            }

            foreach (var match in matches)
            {
                if (!teamScore.ContainsKey(match.AwayTeamId))
                {
                    teamScore.Add(match.AwayTeamId, match.AwayTeamScore);
                }
                else
                {
                    teamScore[match.AwayTeamId] += match.AwayTeamScore;
                }
            }

            return teamScore;
        }
        
        private Dictionary<int,int> GetTotalScoredPointsByOpponent()
        {
            var matches = matchService.GetAllMatchesFromDatabase();
            var teamScore = new Dictionary<int, int>();

            foreach (var match in matches)
            {
                if (!teamScore.ContainsKey(match.HomeTeamId))
                {
                    teamScore.Add(match.HomeTeamId, match.AwayTeamScore);
                }
                else
                {
                    teamScore[match.HomeTeamId] += match.AwayTeamScore;
                }
            }

            foreach (var match in matches)
            {
                if (!teamScore.ContainsKey(match.AwayTeamId))
                {
                    teamScore.Add(match.AwayTeamId, match.HomeTeamScore);
                }
                else
                {
                    teamScore[match.AwayTeamId] += match.HomeTeamScore;
                }
            }

            return teamScore;
        }

        public List<TopTeamServiceModel> GetTopOffensiveTeams()
        {
            var teams = GetTotalScoredPoints().OrderByDescending(x => x.Value).Take(5);

            var result = new List<TopTeamServiceModel>();

            foreach (var team in teams)
            {
                result.Add(data
                    .Teams
                    .Where(x => x.Id == team.Key)
                    .Select(x => new TopTeamServiceModel
                    {
                        Id = x.Id,
                        Score = team.Value,
                        Name = x.Name,
                        TeamImage = x.TeamImage,
                    })
                    .FirstOrDefault());
            }

            return result;
        }
    }
}
