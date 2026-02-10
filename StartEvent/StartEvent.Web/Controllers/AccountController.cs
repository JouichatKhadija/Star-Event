using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;
using StartEvent.Web.Models;

namespace StartEvent.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _db;

		public AccountController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View(new LoginViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (!ModelState.IsValid)
				return View(model);

			var user = await _db.Utilisateurs.FirstOrDefaultAsync(u => u.Username == model.Username);
			if (user == null || user.Password != model.Password)
			{
				ModelState.AddModelError(string.Empty, "Identifiants invalides.");
				return View(model);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(user.Role) ? "user" : user.Role)
			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);

			return user.Role?.ToLower() == "admin"
				? RedirectToAction("Index", "Admin")
				: RedirectToAction("Index", "User");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View(new RegisterViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var exists = await _db.Utilisateurs.AnyAsync(u => u.Username == model.Username);
			if (exists)
			{
				ModelState.AddModelError(nameof(model.Username), "Ce nom d'utilisateur existe déjà.");
				return View(model);
			}

			var newUser = new Utilisateur
			{
				Username = model.Username,
				Password = model.Password,
				Role = "user"
			};

			_db.Utilisateurs.Add(newUser);
			await _db.SaveChangesAsync();

			TempData["RegisterSuccess"] = "Compte créé avec succès. Vous pouvez vous connecter.";
			return RedirectToAction(nameof(Login));
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Accueil");
		}
	}
}






