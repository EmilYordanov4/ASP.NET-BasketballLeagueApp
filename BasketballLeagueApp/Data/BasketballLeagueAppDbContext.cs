using BasketballLeagueApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketballLeagueApp.Data
{
    public class BasketballLeagueAppDbContext : DbContext
    {
        public BasketballLeagueAppDbContext(DbContextOptions<BasketballLeagueAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Player> Players { get; set; }

    }
}
