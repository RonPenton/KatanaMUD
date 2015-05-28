using KatanaMUD.Messages;
using KatanaMUD.Models;
using KatanaMUD.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD
{
    public class Validation
    {
        public bool Allowed { get; set; }
        public string FirstPerson { get; set; }
        public string ThirdPerson { get; set; }
        public string Module { get; private set; }
        public ValidationPriority Priority { get; private set; }
        public Action FailureAction { get; private set; }

        public Validation()
        {
            this.Priority = ValidationPriority.Medium;
            this.Allowed = true;
        }

        public Validation Fail(string module, Action action, ValidationPriority priority = ValidationPriority.Medium)
        {
            Allowed = false;
            this.Module = module;
            this.Priority = priority;
            this.FailureAction = action;
            this.FirstPerson = null;
            this.ThirdPerson = null;
            return this;
        }

        public Validation Fail(string firstPerson, string thirdPerson = null, string module = "Base", ValidationPriority priority = ValidationPriority.Medium)
        {
            Allowed = false;
            this.FirstPerson = firstPerson;
            this.ThirdPerson = thirdPerson;
            this.Module = module;
            this.Priority = priority;
            this.FailureAction = null;
            return this;
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

    public static class ValidationHelper
    {
        public static Validation Validate<T>(this IEnumerable<T> scripts, Action<T, Validation> action) where T : IScript
        {
            var validation = new Validation();
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
