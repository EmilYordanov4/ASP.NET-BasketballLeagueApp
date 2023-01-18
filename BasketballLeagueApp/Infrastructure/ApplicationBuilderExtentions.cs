using BasketballLeagueApp.Data;
using BasketballLeagueApp.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BasketballLeagueApp.Infrastructure
{
    public static class ApplicationBuilderExtentions
    {
        private const int numberOfMatches = 100;
        private const int minScore = 50;
        private const int maxScore = 140;

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;

            var data = services.GetRequiredService<BasketballLeagueAppDbContext>();

            data.Database.Migrate();

            SeedTeams(data);
            SeedPlayers(data);
            SeedMatches(data);
            CalculateTotalMatches(data);
            CalculateWins(data);
            CalculateLoses(data);

            return app;
        }

        private static void CalculateLoses(BasketballLeagueAppDbContext data)
        {
            if (data.Teams.All(x => x.Loses != 0))
            {
                return;
            }

            List<Team> teams = data.Teams.ToList();

            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Loses = teams[i].Matches - teams[i].Wins;
            }

            data.SaveChanges();
        }

        private static void CalculateWins(BasketballLeagueAppDbContext data)
        {
            if (data.Teams.All(x => x.Wins != 0))
            {
                return;
            }

            List<Team> teams = data.Teams.ToList();

            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Wins = data.Matches.Where(a => a.HomeTeamId == i + 1 && a.HomeTeamScore > a.AwayTeamScore).Count() + data.Matches.Where(a => a.AwayTeamId == i + 1 && a.AwayTeamScore > a.HomeTeamScore).Count();
            }

            data.SaveChanges();
        }

        private static void CalculateTotalMatches(BasketballLeagueAppDbContext data)
        {
            if (data.Teams.All(x => x.Matches != 0))
            {
                return;
            }

            List<Team> teams = data.Teams.ToList();

            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Matches = data.Matches.Where(a => a.HomeTeamId == i + 1).Count() + data.Matches.Where(a => a.AwayTeamId == i + 1).Count();
            }

            data.SaveChanges();
        }

        private static void SeedMatches(BasketballLeagueAppDbContext data)
        {
            if (data.Matches.Any())
            {
                return;
            }

            for (int i = 0; i < numberOfMatches; i++)
            {
                GenerateMatch(data);
            }

            data.SaveChanges();
        }

        private static void SeedPlayers(BasketballLeagueAppDbContext data)
        {
            if (data.Players.Any())
            {
                return;
            }

            GenerateChicagoBullsRoster(data);
            GenerateMiamiHeatRoster(data);
            GenerateDallasMavericksRoster(data);
            GenerateBostonCelticsRoster(data);
            GenerateBrooklynNetsRoster(data);
            GenerateLosAngelesLakersRoster(data);

            data.SaveChanges();

        }

        private static void SeedTeams(BasketballLeagueAppDbContext data)
        {
            if (data.Teams.Any())
            {
                return;
            }

            var teamOne = new Team
            {
                Name = "Chicago Bulls",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Chicago_Bulls_logo.svg/800px-Chicago_Bulls_logo.svg.png"
            };

            var teamTwo = new Team
            {
                Name = "Miami Heat",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/en/thumb/f/fb/Miami_Heat_logo.svg/800px-Miami_Heat_logo.svg.png"
            };

            var teamThree = new Team
            {
                Name = "Dallas Mavericks",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/en/thumb/9/97/Dallas_Mavericks_logo.svg/800px-Dallas_Mavericks_logo.svg.png"
            };

            var teamFour = new Team
            {
                Name = "Boston Celtics",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/en/thumb/8/8f/Boston_Celtics.svg/800px-Boston_Celtics.svg.png"
            };

            var teamFive = new Team
            {
                Name = "Brooklyn Nets",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Brooklyn_Nets_newlogo.svg/800px-Brooklyn_Nets_newlogo.svg.png"
            };

            var teamSix = new Team
            {
                Name = "Los Angeles Lakers",
                Roster = new List<Player>(),
                TeamImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Los_Angeles_Lakers_logo.svg/800px-Los_Angeles_Lakers_logo.svg.png"
            };

            data.Teams.AddRange(teamOne, teamTwo, teamThree, teamFour, teamFive, teamSix);

            data.SaveChanges();
        }

        private static void GenerateMatch(BasketballLeagueAppDbContext data)
        {
            List<Team> teams = data.Teams.ToList();

            Random rnd = new Random();

            int homeTeamPick = rnd.Next(1, teams.Count);

            var homeTeam = teams.FirstOrDefault(a => a.Id == homeTeamPick);

            int awayTeamPick = rnd.Next(1, teams.Count);

            while (homeTeamPick == awayTeamPick)
            {
                awayTeamPick = rnd.Next(1, teams.Count);
            }

            var awayTeam = teams.FirstOrDefault(a => a.Id == awayTeamPick);

            var match = new Match()
            {
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                HomeTeamScore = rnd.Next(minScore, maxScore),
                AwayTeamScore = rnd.Next(minScore, maxScore),
            };

            data.Matches.Add(match);
        }
        private static void GenerateChicagoBullsRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Chicago Bulls");


            var ZachLaVine = new Player()
            {
                Name = "Zach LaVine",
                Age = 27,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/203897.png",
                Height = 195.58,
                Weight = 90.72,
                TeamId = team.Id,
                Team = team,
            };

            var DeMarDeRozan = new Player()
            {
                Name = "DeMar DeRozan",
                Age = 33,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/201942.png",
                Height = 198.12,
                Weight = 99.79,
                TeamId = team.Id,
                Team = team
            };

            var NikolaVucevic = new Player()
            {
                Name = "Nikola Vucevic",
                Age = 32,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/202696.png",
                Height = 208.28,
                Weight = 117.9,
                TeamId = team.Id,
                Team = team
            };

            var LonzoBall = new Player()
            {
                Name = "Lonzo Ball",
                Age = 25,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1628366.png",
                Height = 198.12,
                Weight = 86.16,
                TeamId = team.Id,
                Team = team
            };

            var PatrickWilliams = new Player()
            {
                Name = "Patrick Williams",
                Age = 21,
                Position = "power forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1630172.png",
                Height = 200.66,
                Weight = 97.52,
                TeamId = team.Id,
                Team = team
            };

            data.Players.AddRange(ZachLaVine, DeMarDeRozan, NikolaVucevic, LonzoBall, PatrickWilliams);

            team.Roster.Add(ZachLaVine);
            team.Roster.Add(DeMarDeRozan);
            team.Roster.Add(NikolaVucevic);
            team.Roster.Add(LonzoBall);
            team.Roster.Add(PatrickWilliams);
        }
        private static void GenerateDallasMavericksRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Dallas Mavericks");

            var LukaDoncic = new Player()
            {
                Name = "Luka Doncic",
                Age = 23,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1629029.png",
                Height = 200.66,
                Weight = 104.3,
                TeamId = team.Id,
                Team = team
            };

            var TimHardawayJr = new Player()
            {
                Name = "Tim Hardaway Jr",
                Age = 30,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/203501.png",
                Height = 195.58,
                Weight = 92.99,
                TeamId = team.Id,
                Team = team
            };

            var ChristianWood = new Player()
            {
                Name = "Christian Wood",
                Age = 27,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1626174.png",
                Height = 205.74,
                Weight = 97.07,
                TeamId = team.Id,
                Team = team
            };

            var DorianFinneySmith = new Player()
            {
                Name = "Dorian Finney-Smith",
                Age = 29,
                Position = "power forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1627827.png",
                Height = 200.66,
                Weight = 99.79,
                TeamId = team.Id,
                Team = team
            };

            var JoshGreen = new Player()
            {
                Name = "Josh Green",
                Age = 29,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1630182.png",
                Height = 195.58,
                Weight = 90.72,
                TeamId = team.Id,
                Team = team
            };

            data.Players.AddRange(LukaDoncic, TimHardawayJr, ChristianWood, DorianFinneySmith, JoshGreen);

            team.Roster.Add(LukaDoncic);
            team.Roster.Add(TimHardawayJr);
            team.Roster.Add(ChristianWood);
            team.Roster.Add(DorianFinneySmith);
            team.Roster.Add(JoshGreen);
        }
        private static void GenerateMiamiHeatRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Miami Heat");

            var JimmyButler = new Player()
            {
                Name = "Jimmy Butler",
                Age = 33,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/202710.png",
                Height = 200.66,
                Weight = 104.3,
                TeamId = team.Id,
                Team = team
            };

            var BamAdebayo = new Player()
            {
                Name = "Bam Adebayo",
                Age = 25,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1628389.png",
                Height = 205.74,
                Weight = 115.7,
                TeamId = team.Id,
                Team = team
            };

            var KyleLowry = new Player()
            {
                Name = "Kyle Lowry",
                Age = 25,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/200768.png",
                Height = 182.88,
                Weight = 88.9,
                TeamId = team.Id,
                Team = team
            };

            var DuncanRobinson = new Player()
            {
                Name = "Duncan Robinson",
                Age = 25,
                Position = "power forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1629130.png",
                Height = 200.66,
                Weight = 97.52,
                TeamId = team.Id,
                Team = team
            };

            var CalebMartin = new Player()
            {
                Name = "Caleb Martin",
                Age = 25,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1628997.png",
                Height = 195.58,
                Weight = 92.99,
                TeamId = team.Id,
                Team = team
            };

            data.Players.AddRange(JimmyButler, BamAdebayo, KyleLowry, DuncanRobinson, CalebMartin);

            team.Roster.Add(JimmyButler);
            team.Roster.Add(BamAdebayo);
            team.Roster.Add(KyleLowry);
            team.Roster.Add(DuncanRobinson);
            team.Roster.Add(CalebMartin);
        }
        private static void GenerateBostonCelticsRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Boston Celtics");

            var JaylenBrown = new Player()
            {
                Name = "Jaylen Brown",
                Age = 26,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1627759.png",
                Height = 198.12,
                Weight = 101.2,
                TeamId = team.Id,
                Team = team
            };

            var JaysonTatum = new Player()
            {
                Name = "Jayson Tatum",
                Age = 24,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1628369.png",
                Height = 203.2,
                Weight = 95.25,
                TeamId = team.Id,
                Team = team
            };

            var AlHorford = new Player()
            {
                Name = "Al Horford",
                Age = 36,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/201143.png",
                Height = 205.74,
                Weight = 108.9,
                TeamId = team.Id,
                Team = team
            };

            var MalcolmBrogdon = new Player()
            {
                Name = "Malcolm Brogdon",
                Age = 29,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1627763.png",
                Height = 193.04,
                Weight = 103.9,
                TeamId = team.Id,
                Team = team
            };

            var GrantWilliams = new Player()
            {
                Name = "Grant Williams",
                Age = 24,
                Position = "power forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1629684.png",
                Height = 198.12,
                Weight = 107,
                TeamId = team.Id,
                Team = team
            };

            data.Players.AddRange(JaylenBrown, JaysonTatum, AlHorford, MalcolmBrogdon, GrantWilliams);

            team.Roster.Add(JaylenBrown);
            team.Roster.Add(JaysonTatum);
            team.Roster.Add(AlHorford);
            team.Roster.Add(MalcolmBrogdon);
            team.Roster.Add(GrantWilliams);
        }
        private static void GenerateBrooklynNetsRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Brooklyn Nets");

            var NicClaxton = new Player()
            {
                Name = "Nic Claxton",
                Age = 23,
                Position = "point forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1629651.png",
                Height = 210.82,
                Weight = 97.52,
                TeamId = team.Id,
                Team = team
            };

            var SethCurry = new Player()
            {
                Name = "Seth Curry",
                Age = 32,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/203552.png",
                Height = 187.96,
                Weight = 83.91,
                TeamId = team.Id,
                Team = team
            };

            var DavidDukeJr = new Player()
            {
                Name = "David Duke Jr",
                Age = 23,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1630561.png",
                Height = 193.04,
                Weight = 92.53,
                TeamId = team.Id,
                Team = team
            };

            var DayRonSharpe = new Player()
            {
                Name = "Day'Ron Sharpe",
                Age = 21,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1630549.png",
                Height = 205.74,
                Weight = 88.45,
                TeamId = team.Id,
                Team = team
            };

            var KyrieIrving = new Player()
            {
                Name = "KyrieIrving",
                Age = 30,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/202681.png",
                Height = 187.96,
                Weight = 120.2,
                TeamId = team.Id,
                Team = team
            };


            data.Players.AddRange(NicClaxton, SethCurry, DavidDukeJr, DayRonSharpe, KyrieIrving);

            team.Roster.Add(NicClaxton);
            team.Roster.Add(SethCurry);
            team.Roster.Add(DavidDukeJr);
            team.Roster.Add(DayRonSharpe);
            team.Roster.Add(KyrieIrving);
        }
        private static void GenerateLosAngelesLakersRoster(BasketballLeagueAppDbContext data)
        {
            var team = data.Teams.FirstOrDefault(a => a.Name == "Los Angeles Lakers");

            var RussellWestbrook = new Player()
            {
                Name = "Russell Westbrook",
                Age = 34,
                Position = "point guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/201566.png",
                Height = 190.5,
                Weight = 90.72,
                TeamId = team.Id,
                Team = team
            };

            var LeBronJames = new Player()
            {
                Name = "LeBron James",
                Age = 37,
                Position = "small forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/2544.png",
                Height = 205.74,
                Weight = 113.4,
                TeamId = team.Id,
                Team = team
            };

            var AnthonyDavis = new Player()
            {
                Name = "Anthony Davis",
                Age = 29,
                Position = "power  forward",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/203076.png",
                Height = 208.28,
                Weight = 114.8,
                TeamId = team.Id,
                Team = team
            };

            var DamianJones = new Player()
            {
                Name = "Damian Jones",
                Age = 27,
                Position = "center",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1627745.png",
                Height = 210.82,
                Weight = 111.1,
                TeamId = team.Id,
                Team = team
            };

            var AustinReaves = new Player()
            {
                Name = "Austin Reaves",
                Age = 24,
                Position = "shooting guard",
                Image = "https://cdn.nba.com/headshots/nba/latest/1040x760/1630559.png",
                Height = 195.58,
                Weight = 89.36,
                TeamId = team.Id,
                Team = team
            };

            data.Players.AddRange(RussellWestbrook, LeBronJames, AnthonyDavis, DamianJones, AustinReaves);

            team.Roster.Add(RussellWestbrook);
            team.Roster.Add(LeBronJames);
            team.Roster.Add(AnthonyDavis);
            team.Roster.Add(DamianJones);
            team.Roster.Add(AustinReaves);
        }
    }
}
