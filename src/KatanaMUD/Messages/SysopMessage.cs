using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KatanaMUD.Models;
using System.Text.RegularExpressions;

namespace KatanaMUD.Messages
{
    public class SysopMessage : MessageBase
    {
        public string Command { get; set; }


        public override void Process(Actor actor)
        {
            if (actor.User?.AccessLevel != AccessLevel.Sysop)
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = "You do not have sysop access." });
                return;
            }

            Regex regex = new Regex("\\s+");

            var tokens = regex.Split(Command);

            if(tokens[0] == "spawn")
            {
                var tail = String.Join(" ", tokens.Skip(1));
                var templates = Game.Data.ItemTemplates.FindByName(tail, x => x.Name, true, true);
                if(!templates.Any())
                {
                    actor.SendMessage(new ActionNotAllowedMessage() { Message = "Item not found" });
                    return;
                }
                if(templates.Count() > 1)
                {
                    actor.SendMessage(new AmbiguousItemMessage() { Items = templates.Select(x => new ItemDescription(x)).ToArray() });
                    return;
                }


                var item = templates.First().SpawnInstance();
                item.Actor = actor;
                actor.SendMessage(new GenericMessage() { Message = "Spawned new " + item.Name });
            }
            if(tokens[0] == "minify")
            {
                Currency.Minify(actor.Cash);
                actor.SendMessage(new GenericMessage() { Message = "Cash minified." });
            }
        }
    }
}
