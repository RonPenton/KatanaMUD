namespace KatanaMUD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RaceTemplate")]
    public partial class RaceTemplate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RaceTemplate()
        {
            Actors = new HashSet<Actor>();
            ClassTemplates = new HashSet<ClassTemplate>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Hp { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Intelligence { get; set; }

        public int Health { get; set; }

        public int Charm { get; set; }

        public int StrengthCap { get; set; }

        public int AgilityCap { get; set; }

        public int WisdomCap { get; set; }

        public int IntelligenceCap { get; set; }

        public int HealthCap { get; set; }

        public int CharmCap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Actor> Actors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassTemplate> ClassTemplates { get; set; }
    }
}
