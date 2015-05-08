using KatanaMUD.Messages;
using KatanaMUD.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Spam;
using System;
using System.Data.SqlClient;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KatanaMUD.Controllers
{
    public class HomeController : Controller
    {
        private User GetUser()
        {
            if (Context.User == null || Context.User.Identity == null || !Context.User.Identity.IsAuthenticated)
                return null;
            return KatanaMUD.Game.Data.Users.SingleOrDefault(x => x.Id.Equals(Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        private Actor GetCharacter()
        {
            return GetUser()?.Actors.FirstOrDefault();
        }

        private bool HasCharacter()
        {
            return GetCharacter() != null;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Game()
        {
            if (!HasCharacter())
                return RedirectToAction("ChooseRace");

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChooseRace()
        {
            if (HasCharacter())
                return RedirectToAction("EditStats");

            var actor = new ActorModel() { Name = Context.User.Identity.Name };
            ViewBag.Races = KatanaMUD.Game.Data.RaceTemplates.ToList();
            return View(actor);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseRace(ActorModel actor)
        {
            if (HasCharacter())
                return RedirectToAction("EditStats");

            ViewBag.Classes = KatanaMUD.Game.Data.ClassTemplates.ToList();
            return View("ChooseClass", actor);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseClass(ActorModel actor)
        {
            if (HasCharacter())
                return RedirectToAction("EditStats");
            return EditStatsHelper(actor);
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditStats()
        {
            if (!HasCharacter())
                return RedirectToAction("ChooseRace");
            return EditStatsHelper();
        }

        private IActionResult EditStatsHelper(ActorModel actor = null)
        {
            Actor dbActor;
            if (actor != null)
            {
                ViewBag.IsNew = true;
                dbActor = GetNewActor(actor);
            }
            else
            {
                ViewBag.IsNew = false;
                dbActor = GetCharacter();
            }

            return View("EditStats", dbActor);
        }

        private Actor GetNewActor(ActorModel actor)
        {
            var dbActor = new Actor();
            dbActor.Name = actor.Name ?? Context.User.Identity.Name;
            dbActor.Surname = actor.Surname;
            dbActor.CharacterPoints = 100;    // TODO: From settings?
            dbActor.RaceTemplate = KatanaMUD.Game.Data.RaceTemplates.Single(x => x.Id == actor.RaceTemplateId);
            dbActor.ClassTemplate = KatanaMUD.Game.Data.ClassTemplates.Single(x => x.Id == actor.ClassTemplateId);

            return dbActor;
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditStats(ActorModel actor)
        {
            if (!HasCharacter())
            {
                return NewCharacter(actor);
            }
            else
            {
                return EditedStats(actor);
            }
        }

        private IActionResult EditedStats(ActorModel actor)
        {
            var dbActor = GetCharacter();
            var cost = ValidateCPs(dbActor, actor);
            if (cost == Int32.MinValue)
                return HttpBadRequest();

            TransferStatsToDbActor(dbActor, actor, cost);
            return RedirectToAction("Game");
        }

        private IActionResult NewCharacter(ActorModel actor)
        {
            if (actor.Name == null || actor.Name.Length > 50 || actor.Surname?.Length > 50)
                return HttpBadRequest();
            // TODO: Name validation
            //if(!ValidateName(actor.Name) || !ValidateName(actor.Surname))
            // meh.

            var dbActor = GetNewActor(actor);
            var cost = ValidateCPs(dbActor, actor);
            if (cost == Int32.MinValue)
                return HttpBadRequest();

            // TODO: Load initial room from a configurable source.
            dbActor.Room = KatanaMUD.Game.Data.Rooms.First(x => x.Id == 100001);
            TransferStatsToDbActor(dbActor, actor, cost);
            dbActor.User = GetUser();
            KatanaMUD.Game.Data.Actors.Add(dbActor);
            return RedirectToAction("Game");
        }

        private void TransferStatsToDbActor(Actor dbActor, ActorModel actor, int cost)
        {
            dbActor.CharacterPoints -= cost;
            dbActor.Stats.SetCalculatedValue("Strength", actor.Strength);
            dbActor.Stats.SetCalculatedValue("Health", actor.Health);
            dbActor.Stats.SetCalculatedValue("Willpower", actor.Willpower);
            dbActor.Stats.SetCalculatedValue("Intellect", actor.Intellect);
            dbActor.Stats.SetCalculatedValue("Charm", actor.Charm);
            dbActor.Stats.SetCalculatedValue("Agility", actor.Agility);
        }

        private int ValidateCPs(Actor dbActor, ActorModel actor)
        {
            var cps = dbActor.CharacterPoints;

            var stats = new string[] {
                "Strength",
                "Health",
                "Willpower",
                "Intellect",
                "Charm",
                "Agility"
            };

            var cost = 0;
            foreach (var stat in stats)
            {
                var val = VerifyStat(stat, dbActor, actor);
                if (val == Int32.MinValue)
                    return Int32.MinValue;
                cost += val;
            }

            if (cost > cps)
                return Int32.MinValue;
            return cost;
        }
        private int VerifyStat(string stat, Actor dbActor, ActorModel actor)
        {
            var initial = (long)dbActor.RaceTemplate.JSONStats[stat];
            var current = dbActor.Stats.GetCalculatedValue<long>(stat);
            var modified = (long)(typeof(ActorModel).GetProperty(stat).GetValue(actor));
            var cap = dbActor.Stats.GetCalculatedValue<long>(stat + "Cap");

            if (modified < current || modified > cap)
                return Int32.MinValue;

            int cost = 0;
            for (var i = current; i < modified; i++)
            {
                cost += CPsPerPoint(i + 1, initial);
            }

            return cost;
        }

        private int CPsPerPoint(long point, long initial)
        {
            var difference = point - initial;
            if (difference < 0)
                throw new InvalidOperationException();

            // 0 -> 0
            // 1-10 -> 1
            // 11-20 -> 2
            // 21->30 -> 3
            // etc

            return (int)Math.Floor((difference + 9.0) / 10.0);
        }


        public static void EditControl(IHtmlHelper html, long initial, long max, long current, string label)
        {

            html.ViewContext.Writer.Write(html.Raw("<div class=\"statrow\">"));
            html.ViewContext.Writer.Write(html.Raw("<label>" + label + "</label>"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"hidden\" class=\"initial\" value=\"" + initial.ToString() + "\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"hidden\" class=\"current\" value=\"" + current.ToString() + "\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"hidden\" class=\"maxval\" value=\"" + max.ToString() + "\" />"));
            html.ViewContext.Writer.Write(html.Raw("<label class=\"range\">" + String.Format("{0}-{1}", initial, max) + "</label>"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"button\" class=\"reset\" value=\"Reset\" tabindex=\"-100\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"button\" class=\"down\" value=\"-\" tabindex=\"-100\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"text\" class=\"edit\" value=\"" + current.ToString() + "\" name=\"" + label + "\" id=\"" + label + "\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"button\" class=\"up\" value=\"+\" tabindex=\"-100\" />"));
            html.ViewContext.Writer.Write(html.Raw("<input type=\"button\" class=\"max\" value=\"Max\" tabindex=\"-100\" />"));
            html.ViewContext.Writer.Write(html.Raw("</div>"));
        }
}
}