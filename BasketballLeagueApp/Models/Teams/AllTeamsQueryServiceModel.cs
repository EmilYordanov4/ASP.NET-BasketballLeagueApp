using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BasketballLeagueApp.Models.Teams
{
    public class AllTeamsQueryServiceModel
    {
        public TeamSorting Sorting { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public IEnumerable<TeamServiceModel> Teams { get; set; }
    }
}
