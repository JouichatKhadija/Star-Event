using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StartEvent.Web.Data;
using StartEvent.Web.Models;

namespace StartEvent.Web.Controllers
{
    public class CelebritesController : Controller
    {
		private readonly ApplicationDbContext _db;
        private readonly ILogger<CelebritesController> _logger;

		public CelebritesController(ApplicationDbContext db, ILogger<CelebritesController> logger)
        {
			_db = db;
            _logger = logger;
        }

        // GET: /Celebrites
		public async Task<IActionResult> Index(string? q)
        {
			var query = _db.Celebrites.AsQueryable();

			if (!string.IsNullOrWhiteSpace(q))
			{
				var term = q.Trim();
				query = query.Where(c =>
					EF.Functions.Like(c.Nom, $"%{term}%") ||
					(EF.Functions.Like(c.Profession ?? string.Empty, $"%{term}%")) ||
					(EF.Functions.Like(c.LieuNaissance ?? string.Empty, $"%{term}%")));
			}

			var celebrites = await query
				.OrderBy(c => c.Nom)
				.ToListAsync();

			return View(celebrites);
        }

        // GET: /Celebrites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celebrite = await _db.Celebrites
                .FirstOrDefaultAsync(m => m.Id == id);

            if (celebrite == null)
            {
                return NotFound();
            }

            return View(celebrite);
        }
    }
}
