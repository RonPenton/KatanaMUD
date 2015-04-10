using System;

namespace KatanaMUD.Models
{
    public class ActorModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ClassTemplateId { get; set; }
        public int RaceTemplateId { get; set; }
    }
}