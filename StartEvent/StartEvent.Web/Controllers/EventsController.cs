using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;
using StartEvent.Web.Models;

namespace StartEvent.Web.Controllers
{
	public class EventsController : Controller
	{
		private readonly ApplicationDbContext _db;

		public EventsController(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			var eventsList = await _db.Evenements.OrderByDescending(e => e.CreatedAt).ToListAsync();
			return View(eventsList);
		}

		[Route("musique")]
		public async Task<IActionResult> Musique()
		{
			var eventsList = await _db.Evenements
				.Where(e => e.Category.ToLower() == "musique")
				.ToListAsync();
			return View("Musique", eventsList);
		}

		[Route("nocturne")]
		public async Task<IActionResult> Nocturne()
		{
			var eventsList = await _db.Evenements
				.Where(e => e.Category.ToLower() == "nocturne")
				.ToListAsync();
			return View("Nocturne", eventsList);
		}

		[Route("arts")]
		public async Task<IActionResult> Arts()
		{
			var eventsList = await _db.Evenements
				.Where(e => e.Category.ToLower() == "arts")
				.ToListAsync();
			return View("Arts", eventsList);
		}

		[Route("fetes")]
		public async Task<IActionResult> Fetes()
		{
			var eventsList = await _db.Evenements
				.Where(e => e.Category.ToLower() == "fetes")
				.ToListAsync();
			return View("Fetes", eventsList);
		}

		[Route("loisirs")]
		public async Task<IActionResult> Loisirs()
		{
			var eventsList = await _db.Evenements
				.Where(e => e.Category.ToLower() == "loisirs")
				.ToListAsync();
			return View("Loisirs", eventsList);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Reserver(int eventId, int places = 1, string? returnUrl = null)
		{
			bool HasReturnUrl() => !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl);

			if (eventId <= 0)
			{
				TempData["ReservationError"] = "Événement introuvable.";
				return HasReturnUrl() ? Redirect(returnUrl!) : RedirectToAction(nameof(Index));
			}

			if (places < 1)
			{
				places = 1;
			}

			var ev = await _db.Evenements.FirstOrDefaultAsync(e => e.Id == eventId);
			if (ev == null)
			{
				TempData["ReservationError"] = "Événement introuvable.";
				return HasReturnUrl() ? Redirect(returnUrl!) : RedirectToAction(nameof(Index));
			}

			var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Challenge();
			}

			var existing = await _db.Reservations
				.FirstOrDefaultAsync(r => r.UtilisateurId == userId && r.EvenementId == eventId && !r.Payee);

			if (existing != null)
			{
				existing.NombrePlaces += places;
				existing.DateReservation = DateTime.UtcNow;
			}
			else
			{
				var reservation = new Reservation
				{
					UtilisateurId = userId,
					EvenementId = eventId,
					NombrePlaces = places,
					Payee = false,
					DateReservation = DateTime.UtcNow
				};
				_db.Reservations.Add(reservation);
			}

			await _db.SaveChangesAsync();
			TempData["ReservationSuccess"] = "Votre choix a été enregistré et ajouté à votre panier.";

			if (HasReturnUrl())
			{
				return Redirect(returnUrl!);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}





