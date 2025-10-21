using Microsoft.AspNetCore.Mvc;
using StartEvent.Web.Data;
using StartEvent.Web.Models;
using System.Linq;

namespace StartEvent.Web.Controllers
{
    public class AccueilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccueilController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Celebrites = _context.Celebrites.ToList(),
                Evenements = _context.Evenements.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Contact(ContactFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                // Ici tu peux enregistrer le message en base ou envoyer un email
                TempData["Message"] = "Message envoyé avec succès !";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Veuillez remplir correctement le formulaire.";
            return RedirectToAction("Index");
        }
    }
}
