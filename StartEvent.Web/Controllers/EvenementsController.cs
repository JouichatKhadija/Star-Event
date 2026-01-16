using Microsoft.AspNetCore.Mvc;

namespace StartEvent.Web.Controllers
{
    public class EvenementsController : Controller
    {
        [HttpGet("/Evenements")]
        public IActionResult Index()
        {
            return View();
        }
    }
}






