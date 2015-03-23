using System;

namespace KatanaMUD.Authorization
{
	public class UserRole
	{
		public virtual string RoleId { get; set; }
		public virtual string UserId { get; set; }
	}
}