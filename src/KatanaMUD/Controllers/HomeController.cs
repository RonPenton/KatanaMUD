using KatanaMUD.Messages;
using KatanaMUD.Models;
using Microsoft.AspNet.Mvc;
using Spam;
using System;
using System.Data.SqlClient;
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
            var actor = new ActorModel() { Name = Context.User.Identity.Name };
            ViewBag.Races = KatanaMUD.Game.Data.RaceTemplates.ToList();
            return View(actor);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseRace(ActorModel actor)
        {
            ViewBag.Classes = KatanaMUD.Game.Data.ClassTemplates.ToList();

            return View("ChooseClass", actor);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseClass(ActorModel actor)
        {
            ViewBag.IsNew = true;

            var dbActor = new Actor();

            dbActor.Name = Context.User.Identity.Name;
            dbActor.CharacterPoints = 100;    // TODO: From settings?
            dbActor.RaceTemplate = KatanaMUD.Game.Data.RaceTemplates.Single(x => x.Id == actor.RaceTemplateId);
            dbActor.ClassTemplate = KatanaMUD.Game.Data.ClassTemplates.Single(x => x.Id == actor.ClassTemplateId);
            JsonContainer.Merge(dbActor.Stats, dbActor.RaceTemplate.Stats, dbActor.ClassTemplate.Stats);

            return View("EditStats", dbActor);
        }

        [Authorize]
        public IActionResult CreateCharacter()
        {
            var actor = new Actor() { Name = Context.User.Identity.Name };
            return View(actor);
        }
    }
}
