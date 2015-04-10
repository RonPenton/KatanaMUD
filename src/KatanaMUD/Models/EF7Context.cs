namespace KatanaMUD.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata;

    public partial class EF7Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlServer(@"Server=localhost;Database=KatanaMUD;integrated security=True;");
        }

        public virtual DbSet<EF7User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ClassTemplate>()
            //    .HasMany(e => e.RaceTemplates)
            //    .WithMany(e => e.ClassTemplates)
            //    .Map(m => m.ToTable("RaceClassRestrictions").MapLeftKey("ClassTemplateId").MapRightKey("RaceTemplateId"));
        }
    }
}