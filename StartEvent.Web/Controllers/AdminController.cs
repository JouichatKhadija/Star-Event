using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;
using StartEvent.Web.Models;
using System.Linq;
using System;

namespace StartEvent.Web.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _db;

		public AdminController(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			var vm = new AdminDashboardViewModel
			{
				TotalEvenements = await _db.Evenements.CountAsync(),
				TotalCelebrites = await _db.Celebrites.CountAsync(),
				TotalReservations = await _db.Reservations.CountAsync(),
				TotalMessages = await _db.ContactMessages.CountAsync(),
				Evenements = await _db.Evenements.OrderByDescending(e => e.CreatedAt).ToListAsync(),
				Celebrites = await _db.Celebrites.OrderBy(c => c.Nom).ToListAsync(),
				Reservations = await _db.Reservations.Include(r => r.Utilisateur).Include(r => r.Evenement).OrderByDescending(r => r.DateReservation).ToListAsync(),
				ContactMessages = await _db.ContactMessages.OrderByDescending(m => m.DateEnvoye).ToListAsync()
			};
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveEvenement(Evenement model)
		{
			if (string.IsNullOrWhiteSpace(model.Nom) || string.IsNullOrWhiteSpace(model.Description) || model.DateEvenement == default || string.IsNullOrWhiteSpace(model.Lieu))
			{
				TempData["AdminError"] = "Veuillez renseigner au minimum le nom, la description, la date et le lieu.";
				return RedirectToAction(nameof(Index));
			}

			if (model.Id > 0)
			{
				var existing = await _db.Evenements.FirstOrDefaultAsync(e => e.Id == model.Id);
				if (existing == null)
				{
					TempData["AdminError"] = "Événement introuvable.";
					return RedirectToAction(nameof(Index));
				}
				existing.Nom = model.Nom;
				existing.Description = model.Description;
				existing.DateEvenement = model.DateEvenement;
				existing.Lieu = model.Lieu;
				existing.Category = model.Category;
				existing.ImagePath = model.ImagePath;
				existing.Prix = model.Prix;
				existing.PlacesDisponibles = model.PlacesDisponibles;
			}
			else
			{
				model.CreatedAt = DateTime.UtcNow;
				_db.Evenements.Add(model);
			}
			await _db.SaveChangesAsync();
			TempData["AdminSuccess"] = model.Id > 0 ? "Événement mis à jour." : "Événement ajouté.";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveCelebrite(Celebrite model)
		{
			if (string.IsNullOrWhiteSpace(model.Nom))
			{
				TempData["AdminError"] = "Le nom de la célébrité est requis.";
				return RedirectToAction(nameof(Index));
			}

			if (model.Id > 0)
			{
				var existing = await _db.Celebrites.FirstOrDefaultAsync(c => c.Id == model.Id);
				if (existing == null)
				{
					TempData["AdminError"] = "Célébrité introuvable.";
					return RedirectToAction(nameof(Index));
				}
				existing.Nom = model.Nom;
				existing.DateNaissance = model.DateNaissance;
				existing.Profession = model.Profession;
				existing.LieuNaissance = model.LieuNaissance;
				existing.PhotoPath = model.PhotoPath;
				existing.Parcours = model.Parcours;
				existing.Loisirs = model.Loisirs;
			}
			else
			{
				_db.Celebrites.Add(model);
			}
			await _db.SaveChangesAsync();
			TempData["AdminSuccess"] = model.Id > 0 ? "Célébrité mise à jour." : "Célébrité ajoutée.";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteEvenement(int id)
		{
			var ev = await _db.Evenements.FirstOrDefaultAsync(e => e.Id == id);
			if (ev != null)
			{
				_db.Evenements.Remove(ev);
				await _db.SaveChangesAsync();
				TempData["AdminSuccess"] = "Événement supprimé.";
			}
			else
			{
				TempData["AdminError"] = "Événement introuvable.";
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCelebrite(int id)
		{
			var celeb = await _db.Celebrites.FirstOrDefaultAsync(c => c.Id == id);
			if (celeb != null)
			{
				_db.Celebrites.Remove(celeb);
				await _db.SaveChangesAsync();
				TempData["AdminSuccess"] = "Célébrité supprimée.";
			}
			else
			{
				TempData["AdminError"] = "Célébrité introuvable.";
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteReservation(int id)
		{
			var res = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id);
			if (res != null)
			{
				_db.Reservations.Remove(res);
				await _db.SaveChangesAsync();
				TempData["AdminSuccess"] = "Réservation supprimée.";
			}
			else
			{
				TempData["AdminError"] = "Réservation introuvable.";
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> ReservationTicket(int id)
		{
			var res = await _db.Reservations
				.Include(r => r.Evenement)
				.Include(r => r.Utilisateur)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (res == null)
			{
				TempData["AdminError"] = "Réservation introuvable.";
				return RedirectToAction(nameof(Index));
			}

			var ticket = new TicketItemViewModel
			{
				EvenementNom = res.Evenement?.Nom ?? string.Empty,
				DateEvenement = res.Evenement?.DateEvenement ?? DateTime.MinValue,
				Lieu = res.Evenement?.Lieu ?? string.Empty,
				ImagePath = res.Evenement?.ImagePath,
				NombrePlaces = res.NombrePlaces
			};

			var vm = new TicketsViewModel
			{
				NomComplet = res.Utilisateur?.Username ?? "Ticket",
				Tickets = new List<TicketItemViewModel> { ticket }
			};

			return View("ReservationTicket", vm);
		}
	}
}

