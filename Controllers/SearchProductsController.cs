using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    public class SearchProductsController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search()
        {
            string key = !string.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "";
            List<ItemProducts> _ListRecord = db.Products.Where(item => item.Name.Contains(key)).OrderByDescending(item => item.Id).Take(5).ToList();
            return new JsonResult(Ok(_ListRecord));
        }
    }
}
