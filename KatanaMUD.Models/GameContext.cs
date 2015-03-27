namespace KatanaMUD.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GameContext : DbContext
    {
        public GameContext()
            : base("name=GameContext")
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<ArmorType> ArmorTypes { get; set; }
        public virtual DbSet<ClassTemplate> ClassTemplates { get; set; }
        public virtual DbSet<ClassTemplateArmorType> ClassTemplateArmorTypes { get; set; }
        public virtual DbSet<ClassTemplateWeaponType> ClassTemplateWeaponTypes { get; set; }
        public virtual DbSet<RaceTemplate> RaceTemplates { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WeaponType> WeaponTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
