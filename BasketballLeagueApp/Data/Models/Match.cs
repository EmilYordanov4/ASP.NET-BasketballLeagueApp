using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballLeagueApp.Data.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        [Required]
        public int HomeTeamScore { get; set; }

        [Required]
        public int AwayTeamScore { get; set; }

    }
}
