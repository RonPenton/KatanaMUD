using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Messages
{
    public class SearchCommand : MessageBase
    {
        public Direction? Direction { get; set; }

        public override void Process(Actor actor)
        {
            if(Direction == null)
            {
                // search room.
                var perception = actor.PerceptionFinal;

                var foundItems = new List<Item>();

                // you can only find items which you haven't already found. 
                foreach(var item in actor.Room.Items.Where(x => x.HiddenTime != null && !x.UsersWhoFoundMe.Contains(actor)).ToList())
                {
                    //TODO: Figure out if this fomula needs tweaking
                    var span = new TimeSpan(item.HiddenTime.Value);
                    var chance = 100.0 / ((span.TotalDays + 30.0) / 30.0);
                    chance = ((double)perception - chance) / 100.0;

                    if(Game.Random.NextDouble() < chance)
                    {
                        foundItems.Add(item);
                        item.UsersWhoFoundMe.Add(actor);
                    }
                }

                var foundCash = new List<CurrencyDescription>();
                var cash = actor.Room.GetTotalCashUserCanSee(actor);
                foreach (var currency in cash)
                {
                    // user already knows about all cash in the room.
                    if (currency.Visible == currency.Total)
                        continue;

                    var remaining = currency.Total - currency.Visible;

                    // perform a roll to see if the user finds any. 
                    // TODO: The search value is pegged at 50%. We may need to tweak this.
                    var chance = ((double)perception - 50.0) / 100.0;

                    if (Game.Random.NextDouble() < chance)
                    {
                        // now figure out how much the user finds. 
                        //TODO: tweak
                        //TODO: integer overflow?
                        var amount = Game.Random.Next(1, (int)remaining + 1);

                        actor.Room.FoundCash(actor, currency.Currency, amount);
                        foundCash.Add(new CurrencyDescription(currency.Currency, amount));
                    }
                }

                var message = new SearchMessage()
                {
                    FoundItems = foundItems.Select(x => new ItemDescription(x)).ToArray(),
                    FoundCash = foundCash.ToArray()
                };
                actor.SendMessage(message);
                actor.Room.ActiveActors.Except(actor).ForEach(x => x.SendMessage(new GenericMessage() { Class = "searching", Message = String.Format("{0} is searching the area.", actor.Name) }));
            }
        }
    }

    public class SearchMessage : MessageBase
    {
        public ItemDescription[] FoundItems { get; set; }
        public CurrencyDescription[] FoundCash { get; set; }
    }
}