using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public class ScriptValidation : Validation
    {
        public string Module { get; private set; }
        public ValidationPriority Priority { get; private set; }
        public Action FailureAction { get; private set; }

        public ScriptValidation()
        {
            this.Priority = ValidationPriority.Medium;
        }

        public void Fail(string module, Action action, ValidationPriority priority = ValidationPriority.Medium)
        {
            Allowed = false;
            this.Module = module;
            this.Priority = priority;
            this.FailureAction = action;
            this.FirstPerson = null;
            this.ThirdPerson = null;
        }

        public void Fail(string firstPerson, string thirdPerson, string module, ValidationPriority priority = ValidationPriority.Medium)
        {
            Allowed = false;
            this.FirstPerson = firstPerson;
            this.ThirdPerson = thirdPerson;
            this.Module = module;
            this.Priority = priority;
            this.FailureAction = null;
        }

        public void HandleFailure(Actor actor, bool useThirdPersonMessage = false)
        {
            if (this.FailureAction != null)
            {
                this.FailureAction();
            }
            else
            {
                var message = FirstPerson;
                if (useThirdPersonMessage)
                    message = ThirdPerson;
                actor.SendMessage(new ActionNotAllowedMessage() { Message = message });
            }
        }

        public void CancelFailure()
        {
            Allowed = true;
            this.FirstPerson = this.ThirdPerson = this.Module = null;
            this.Priority = ValidationPriority.Medium;
            this.FailureAction = null;
        }
    }

    public static class ScriptHelper
    {
        public static ScriptValidation Validate<T>(this IEnumerable<T> scripts, Action<T, ScriptValidation> action) where T : IScript
        {
            var validation = new ScriptValidation();
            scripts.ForEach(x => action(x, validation));
            return validation;
        }
    }

    public enum ValidationPriority
    {
        Lowest,
        Low,
        Medium,
        High,
        Highest
    }
}
