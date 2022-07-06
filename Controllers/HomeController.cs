using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        public FoodDbContext db = new FoodDbContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.GetAdv = db.Advs.ToList();
            ViewBag.ProductsHot = db.Products.Where(i => i.Hot == 1).Take(8).ToList();
            ViewBag.NewsHot = db.News.Where(i => i.Hot == 1).Take(4).ToList();
            return View();
        }
        [HttpGet]
        public JsonResult GetData(int? id)
        {
            var kq = from product in db.Products
                     join category in db.categories
                     on product.CategoryId equals category.Id
                     where category.ParentId == id
                     select product;
            return new JsonResult(Ok(kq));
        }
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact";
            return View();
        }
    }
}