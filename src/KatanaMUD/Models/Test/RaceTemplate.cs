using System;
using System.Data.SqlClient;
using Spam;

namespace KatanaMUD.Models.Test
{
	public partial class RaceTemplate : Entity<int>
	{
		public override int Key
		{
			get { return Id; }
			set { Id = value; }
		}

		private int _id;

		public int Id { get { return _id; } set { _id = value; this.Changed(); } }

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

		public string Description { get; set; }

		public virtual ICollection<Actor> Actors { get; set; }

		public virtual ICollection<ClassTemplate> ClassTemplates { get; set; }
	}
}