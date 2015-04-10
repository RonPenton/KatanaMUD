using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KatanaMUD.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Security.Cookies;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KatanaMUD.Controllers
{
	public class AccountController : Controller
	{
		// GET: /<controller>/
		public IActionResult Login()
		{
			if (Context.User.Identity?.IsAuthenticated ?? false)
			{
				return RedirectToAction("Index", controllerName: "Home");
			}

			return View();
		}

		public IActionResult Register()
		{
			if (Context.User.Identity?.IsAuthenticated ?? false)
			{
				return RedirectToAction("Index", controllerName: "Home");
			}

			return View();
		}

		[HttpPost]
		public IActionResult Register(RegisterModel model)
		{
			using (var context = new EF7Context())
			{
				var user = context.Users.SingleOrDefault(x => x.Id == model.Username);
				if (user != null)
					return View(model: "Username Taken");

				if (model.ConfirmPassword != model.Password)
					return View(model: "Passwords do not match");

				var hash = HashString(model.Password);
				user = new EF7User()
				{
					Id = model.Username,
					PasswordHash = hash
				};

				context.Users.Add(user);
				context.SaveChanges();

				var claim = new Claim(ClaimTypes.Name, "Mithrandir");
				var identity = new ClaimsIdentity(new List<Claim>() { claim }, CookieAuthenticationDefaults.AuthenticationType);
				Context.Response.SignIn(identity);

				return RedirectToAction("Index", controllerName: "Home");
			}
		}

		[HttpPost]
		public IActionResult Login(LoginModel model)
		{
			using (var context = new EF7Context())
			{
				var user = context.Users.SingleOrDefault(x => x.Id == model.Username);
				if (user == null || user.LockoutEnd > DateTimeOffset.Now)
					return View(model: "Login Failed");

				var hash = HashString(model.Password);
				if (hash == user.PasswordHash)
				{
					var claim = new Claim(ClaimTypes.Name, "Mithrandir");
					var identity = new ClaimsIdentity(new List<Claim>() { claim }, CookieAuthenticationDefaults.AuthenticationType);
					Context.Response.SignIn(identity);

					return RedirectToAction("Index", controllerName: "Home");
				}

				user.AccessFailedCount++;
				if (user.AccessFailedCount > 10)
				{
					user.LockoutEnd = DateTimeOffset.Now.AddDays(1);
				}
				context.SaveChanges();
				return View(model: "Login Failed");
			}
		}

		[HttpPost]
		public IActionResult Logout()
		{
			Context.Response.SignOut(CookieAuthenticationDefaults.AuthenticationType);
			return RedirectToAction("Login");
		}


		private string HashString(string toHash)
		{
			var hasher = new HMACSHA512(Encoding.UTF8.GetBytes("KatanaMUDHashKey: 89012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567"));
			var bytes = Encoding.UTF8.GetBytes(toHash);
			var hash = hasher.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
	}
}
