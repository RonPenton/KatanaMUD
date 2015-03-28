namespace KatanaMUD.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata;

    public partial class GameContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlServer(@"Server=localhost;Database=KatanaMUD;integrated security=True;");
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<ArmorType> ArmorTypes { get; set; }
        public virtual DbSet<ClassTemplate> ClassTemplates { get; set; }
        public virtual DbSet<ClassTemplateArmorType> ClassTemplateArmorTypes { get; set; }
        public virtual DbSet<ClassTemplateWeaponType> ClassTemplateWeaponTypes { get; set; }
        public virtual DbSet<RaceTemplate> RaceTemplates { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WeaponType> WeaponTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ClassTemplate>()
            //    .HasMany(e => e.RaceTemplates)
            //    .WithMany(e => e.ClassTemplates)
            //    .Map(m => m.ToTable("RaceClassRestrictions").MapLeftKey("ClassTemplateId").MapRightKey("RaceTemplateId"));
        }
    }
}
