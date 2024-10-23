using Microsoft.AspNetCore.Mvc;

namespace Projeto.Areas.Admin.Controllers
{
    public class OpeningController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
