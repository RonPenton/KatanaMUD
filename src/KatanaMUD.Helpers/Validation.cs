using System;

namespace KatanaMUD
{
    public class Validation
    {
        public bool Allowed { get; set; }
        public string Reason { get; set; }

        public Validation()
        {
            Allowed = true;
        }

        public Validation(string reason)
        {
            Allowed = false;
            Reason = reason;
        }
    }
}