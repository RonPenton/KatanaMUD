using System;
using KatanaMUD.Authorization;
using Microsoft.Data.Entity;

namespace KatanaMUD.Models
{
	public class GameContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptions options)
		{
			options.UseSqlServer(@"Server=localhost;Database=KatanaMUD;integrated security=True;");
		}

		public DbSet<User> Users { get; set; }
	}
}