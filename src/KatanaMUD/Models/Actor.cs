namespace KatanaMUD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Actor")]
    public partial class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public int ActorType { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public int RoomId { get; set; }

        public int? ClassTemplateId { get; set; }

        public int? RaceTemplateId { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Intelligence { get; set; }

        public int Health { get; set; }

        public int Charm { get; set; }

        public int CharacterPoints { get; set; }

        public virtual ClassTemplate ClassTemplate { get; set; }

        public virtual RaceTemplate RaceTemplate { get; set; }

        public virtual User User { get; set; }
    }
}
