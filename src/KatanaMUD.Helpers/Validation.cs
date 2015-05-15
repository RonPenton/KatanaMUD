using System;

namespace KatanaMUD
{
    public class Validation
    {
        public bool Allowed { get; set; }
        public string FirstPerson { get; set; }
        public string ThirdPerson { get; set; }

        public Validation()
        {
            Allowed = true;
        }

        public Validation(string firstPerson, string thirdPerson)
        {
            Allowed = false;
            FirstPerson = firstPerson;
            ThirdPerson = thirdPerson;
        }
    }
}