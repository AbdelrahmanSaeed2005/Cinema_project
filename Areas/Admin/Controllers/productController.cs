using Microsoft.AspNetCore.Mvc;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class productController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
