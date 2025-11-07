using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;

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
			var eventsList = await _db.Evenements.Where(e => e.Category == "musique").ToListAsync();
			return View("Category", eventsList);
		}

		[Route("nocturne")]
		public async Task<IActionResult> Nocturne()
		{
			var eventsList = await _db.Evenements.Where(e => e.Category == "nocturne").ToListAsync();
			return View("Category", eventsList);
		}

		[Route("arts")]
		public async Task<IActionResult> Arts()
		{
			var eventsList = await _db.Evenements.Where(e => e.Category == "arts").ToListAsync();
			return View("Category", eventsList);
		}

		[Route("fetes")]
		public async Task<IActionResult> Fetes()
		{
			var eventsList = await _db.Evenements.Where(e => e.Category == "fetes").ToListAsync();
			return View("Category", eventsList);
		}

		[Route("loisirs")]
		public async Task<IActionResult> Loisirs()
		{
			var eventsList = await _db.Evenements.Where(e => e.Category == "loisirs").ToListAsync();
			return View("Category", eventsList);
		}
	}
}


