using System;

namespace KatanaMUD.Models
{
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class RegisterModel : LoginModel
	{
		public string ConfirmPassword { get; set; }
	}
}