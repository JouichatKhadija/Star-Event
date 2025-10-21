using Microsoft.AspNetCore.Mvc;
using StartEvent.Web.Models;
using System.Collections.Generic;

namespace StartEvent.Web.Controllers
{
    public class CelebritesController : Controller
    {
        // GET: /Celebrites
        public IActionResult Index()
        {
            // Exemple : liste statique pour tester
            var celebrites = new List<Celebrite>
            {
                new Celebrite { Id = 1, Nom = "Zendaya", Profession = "Actrice", PhotoPath = "~/images/celeb1.jpg" },
                new Celebrite { Id = 2, Nom = "Tom Hanks", Profession = "Acteur", PhotoPath = "~/images/celeb2.jpg" },
                new Celebrite { Id = 3, Nom = "Beyoncé", Profession = "Chanteuse", PhotoPath = "~/images/celeb3.jpg" }
            };

            return View(celebrites); // renvoie la liste au Razor
        }

        // GET: /Celebrites/Profil/1
        public IActionResult Profil(int id)
        {
            // Ici tu peux récupérer la célébrité par id depuis ta base
            var celebrite = new Celebrite
            {
                Id = id,
                Nom = "Zendaya",
                Profession = "Actrice",
                PhotoPath = "~/images/celeb1.jpg"
            };
            return View(celebrite);
        }
    }
}
