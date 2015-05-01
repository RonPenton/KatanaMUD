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
            //TODO: Verify sysop access here.

            Regex regex = new Regex("\\s+");

            var tokens = regex.Split(Command);

            if(tokens[0] == "spawn")
            {
                var tail = String.Join(" ", tokens.Skip(1));
                var template = Game.Data.ItemTemplates.FindByName(tail, x => x.Name, true, true);
                if(template == null)
                {
                    actor.SendMessage(new ActionNotAllowedMessage() { Message = "Item not found" });
                    return;
                }


                var item = template.SpawnInstance();
                item.Actor = actor;
                actor.SendMessage(new GenericMessage() { Message = "Spawned new " + item.Name });
            }
        }
    }
}
