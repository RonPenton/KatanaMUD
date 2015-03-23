using System;
using Microsoft.AspNet.Identity;

namespace KatanaMUD.Authorization
{
	public class User
	{
		public string Id { get; set; }
		public bool IsConfirmed { get; set; }
		public int AccessFailedCount { get; set; }
		public DateTimeOffset? LockoutEnd { get; set; }
		public string PasswordHash { get; set; }
	}
}