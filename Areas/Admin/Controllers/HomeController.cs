using Microsoft.AspNetCore.Mvc;
using FastFood.Areas.Admin.Atrributes;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
