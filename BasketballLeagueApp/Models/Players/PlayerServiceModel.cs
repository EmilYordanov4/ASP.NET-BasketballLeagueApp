using BasketballLeagueApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BasketballLeagueApp.Models.Players
{
    public class PlayerServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Image { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public int Age { get; set; }

        public Team Team { get; set; }

        public int TeamId { get; set; }
    }
}
