using KatanaMUD.Messages;
using KatanaMUD.Models;
using Microsoft.AspNet.Mvc;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KatanaMUD.Controllers
{
	public class HomeController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}

        [Authorize]
        public IActionResult Game()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChooseRace()
        {
            var actor = new Actor() { Name = Context.User.Identity.Name };
            var context = new GameContext();
            ViewBag.Races = context.RaceTemplates.ToList();
            return View(actor);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseRace(Actor actor)
        {
            return View(actor);
        }


        [Authorize]
        public IActionResult CreateCharacter()
        {
            var actor = new Actor() { Name = Context.User.Identity.Name };
            return View(actor);
        }

    }
}
