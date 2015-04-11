using System;

namespace KatanaMUD.Models
{
    public class ActorModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ClassTemplateId { get; set; }
        public int RaceTemplateId { get; set; }

        public long Strength { get; set; }
        public long Health { get; set; }
        public long Willpower { get; set; }
        public long Intellect { get; set; }
        public long Charm { get; set; }
        public long Agility { get; set; }
    }
}