using BasketballLeagueApp.Services.Matches;
using Microsoft.AspNetCore.Mvc;

namespace BasketballLeagueApp.Controllers
{
	public class MatchController : Controller
	{
		private readonly IMatchService matches;
		public MatchController(IMatchService matches)
		{
			this.matches = matches;
		}

		public IActionResult All() 
		{
			return View(matches.GetAllMatches());
		}
		
		public IActionResult Highlight() 
		{
			return View(matches.GetHighlightedMatch());
		}
	}
}
