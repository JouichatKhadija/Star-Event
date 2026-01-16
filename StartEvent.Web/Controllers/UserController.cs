using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;
using StartEvent.Web.Models;
using System.Linq;

namespace StartEvent.Web.Controllers
{
	[Authorize(Roles = "user")]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _db;

		public UserController(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			// Chaque utilisateur ne voit que ses réservations : on extrait l'id du claim
			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var reservations = await _db.Reservations
				.Include(r => r.Evenement)
				.Where(r => r.UtilisateurId == userId)
				.OrderByDescending(r => r.DateReservation)
				.ToListAsync();
			return View(reservations);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ModifierPlaces(int id, int places)
		{
			if (places < 1) places = 1;

			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var reservation = await _db.Reservations
				.Include(r => r.Evenement)
				.FirstOrDefaultAsync(r => r.Id == id && r.UtilisateurId == userId);

			if (reservation == null)
			{
				TempData["PaymentError"] = "Réservation introuvable.";
				return RedirectToAction(nameof(Index));
			}

			reservation.NombrePlaces = places;
			await _db.SaveChangesAsync();

			TempData["PaymentSuccess"] = "Le nombre de places a été mis à jour.";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Supprimer(int id)
		{
			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var reservation = await _db.Reservations
				.FirstOrDefaultAsync(r => r.Id == id && r.UtilisateurId == userId);

			if (reservation == null)
			{
				TempData["PaymentError"] = "Réservation introuvable.";
				return RedirectToAction(nameof(Index));
			}

			_db.Reservations.Remove(reservation);
			await _db.SaveChangesAsync();

			TempData["PaymentSuccess"] = "La réservation a été supprimée de votre panier.";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Payer()
		{
			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var reservationsNonPayees = await _db.Reservations
				.Include(r => r.Evenement)
				.Where(r => r.UtilisateurId == userId && !r.Payee)
				.ToListAsync();

			if (!reservationsNonPayees.Any())
			{
				TempData["PaymentError"] = "Aucune réservation en attente de paiement.";
				return RedirectToAction(nameof(Index));
			}

			var total = reservationsNonPayees.Sum(r => (r.Evenement?.Prix ?? 0) * r.NombrePlaces);
			var paymentVm = new PaymentViewModel
			{
				Total = total
			};

			return View("Paiement", paymentVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmerPaiement(PaymentViewModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Nom) ||
				string.IsNullOrWhiteSpace(model.Prenom) ||
				string.IsNullOrWhiteSpace(model.NumeroCarte) ||
				string.IsNullOrWhiteSpace(model.TypeCarte))
			{
				ModelState.AddModelError(string.Empty, "Veuillez remplir tous les champs de paiement.");
				return View("Paiement", model);
			}

			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var reservationsNonPayees = await _db.Reservations
				.Include(r => r.Evenement)
				.Where(r => r.UtilisateurId == userId && !r.Payee)
				.ToListAsync();

			if (!reservationsNonPayees.Any())
			{
				TempData["PaymentError"] = "Aucune réservation en attente de paiement.";
				return RedirectToAction(nameof(Index));
			}

			foreach (var reservation in reservationsNonPayees)
			{
				reservation.Payee = true;
			}

			await _db.SaveChangesAsync();

			var tickets = reservationsNonPayees
				.Select(r => new TicketItemViewModel
				{
					EvenementNom = r.Evenement?.Nom ?? string.Empty,
					DateEvenement = r.Evenement?.DateEvenement ?? DateTime.MinValue,
					Lieu = r.Evenement?.Lieu ?? string.Empty,
					ImagePath = r.Evenement?.ImagePath,
					NombrePlaces = r.NombrePlaces
				})
				.ToList();

			var ticketsVm = new TicketsViewModel
			{
				NomComplet = $"{model.Prenom} {model.Nom}",
				Tickets = tickets
			};

			return View("Tickets", ticketsVm);
		}

		[HttpGet]
		public async Task<IActionResult> Tickets()
		{
			var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(idClaim, out var userId) || userId <= 0)
			{
				return Forbid();
			}

			var user = await _db.Utilisateurs.FirstOrDefaultAsync(u => u.Id == userId);
			var paidReservations = await _db.Reservations
				.Include(r => r.Evenement)
				.Where(r => r.UtilisateurId == userId && r.Payee)
				.OrderByDescending(r => r.DateReservation)
				.ToListAsync();

			if (!paidReservations.Any())
			{
				TempData["PaymentError"] = "Aucun ticket disponible pour le moment.";
				return RedirectToAction(nameof(Index));
			}

			var tickets = paidReservations.Select(r => new TicketItemViewModel
			{
				EvenementNom = r.Evenement?.Nom ?? string.Empty,
				DateEvenement = r.Evenement?.DateEvenement ?? DateTime.MinValue,
				Lieu = r.Evenement?.Lieu ?? string.Empty,
				ImagePath = r.Evenement?.ImagePath,
				NombrePlaces = r.NombrePlaces
			}).ToList();

			var vm = new TicketsViewModel
			{
				NomComplet = user?.Username ?? "Mon ticket",
				Tickets = tickets
			};

			return View("Tickets", vm);
		}
	}
}

