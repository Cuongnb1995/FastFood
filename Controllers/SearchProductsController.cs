using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FastFood.Controllers
{
    public class SearchProductsController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Products(int? page)
        {
            string key = !string.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "";
            ViewBag.key = key;
            int _CurrentPage = page ?? 1;
            int _RecordPerPage = 20;
            List<ItemProducts> listRecord = db.Products.Where(item => item.Name.Contains(key)).OrderByDescending(item => item.Id).ToList();
            return View("SearchProducts", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }

        public IActionResult Search()
        {
            string key = !string.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "";
            List<ItemProducts> _ListRecord = db.Products.Where(item => item.Name.Contains(key)).OrderByDescending(item => item.Id).Take(5).ToList();
            return new JsonResult(Ok(_ListRecord));
        }
        public IActionResult SearchPrice(int? page)
        {
            double fromPrice = !string.IsNullOrEmpty(Request.Query["fromPrice"]) ? Convert.ToDouble(Request.Query["fromPrice"]) : 0;
            double toPrice = !string.IsNullOrEmpty(Request.Query["toPrice"]) ? Convert.ToDouble(Request.Query["toPrice"]) : 0;
            ViewBag.fromPrice = fromPrice;
            ViewBag.toPrice = toPrice;
            int _CurrentPage = page ?? 1;
            int _RecordPerPage = 20;
            List<ItemProducts> listRecord = db.Products.Where(item => (item.Price - (item.Price * item.Discount) / 100) >= fromPrice && (item.Price - (item.Price * item.Discount) / 100) <= toPrice).OrderByDescending(item => item.Id).ToList();
            return View("SearchProducts", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
    }
}
