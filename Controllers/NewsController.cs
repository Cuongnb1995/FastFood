using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FastFood.Controllers
{
    public class NewsController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Index(int? page)
        {
            ViewBag.Title = "News";
            var kq = db.News.ToList();
            return View(kq.ToPagedList(page ?? 1, 6));
        }
        public IActionResult Detail(int id)
        {
            ViewBag.Title = "News";
            var kq = db.News.Find(id);
            return View(kq);
        }
    }
}
