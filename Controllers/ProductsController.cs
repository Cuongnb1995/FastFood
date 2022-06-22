using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    public class ProductsController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Category(int? id)
        {
            if (db.categories.Find(id).ParentId==0)
            {
                ViewBag.Title = db.categories.Find(id).Name.ToString();
            }
            else
            {
                ViewBag.Title = db.categories.Find(db.categories.Find(id).ParentId).Name.ToString(); ;
            };
            string strOrder = "";
            if (!String.IsNullOrEmpty(Request.Query["order"]))
            {
                strOrder = Request.Query["order"];
            }
            ViewBag.CategoryId = db.categories.Find(id);
            var ketqua = new List<ItemProducts>();
            if (db.categories.Find(id).ParentId==0)
            {
                ketqua = (from product in db.Products
                          join category in db.categories
                          on product.CategoryId equals category.Id
                          where category.ParentId == id
                          select product).ToList();
            }
            else
            {
                ketqua = db.Products.Where(i=>i.CategoryId==id).ToList();
            }
            switch (strOrder)
            {
                case "nameAsc":
                    ketqua = ketqua.OrderBy(i => i.Name).ToList();
                    break;

                case "nameDesc":
                    ketqua = ketqua.OrderByDescending(i => i.Name).ToList();
                    break;

                case "priceAsc":
                    ketqua = ketqua.OrderBy(i => (i.Price - i.Price * i.Discount / 100)).ToList();
                    break;

                case "priceDesc":
                    ketqua = ketqua.OrderByDescending(i => (i.Price - i.Price * i.Discount / 100)).ToList();
                    break;
            }

            return View(ketqua);
        }

        public IActionResult Star(int? id)
        {
          
            int _ProductId = id ?? 0;
            int _Star = !String.IsNullOrEmpty(Request.Query["Star"]) ? Convert.ToInt32(Request.Query["Star"]) : 0;
            if (HttpContext.Session.GetString("checkstar") == id.ToString())
                return Redirect("/Products/Detail/" + _ProductId);
            ItemRating record = new ItemRating();
            record.ProductId = _ProductId;
            record.Star = _Star;
            db.Ratings.Add(record);
            db.SaveChanges();
            HttpContext.Session.SetString("checkstar", id.ToString());
            return Redirect("/Products/Detail/" + _ProductId);
        }
        public IActionResult Detail(int? id)
        {
            var kq = db.Products.Find(id);
            if (db.categories.Find(kq.CategoryId).ParentId == 0)
            {
                ViewBag.Title = db.categories.Find(kq.CategoryId).Name.ToString();
            }
            else
            {
                ViewBag.Title = db.categories.Find(db.categories.Find(kq.CategoryId).ParentId).Name.ToString(); ;
            };
            ViewBag.CategoryId = db.categories.Find(kq.CategoryId);

            return View(kq);
        }
    }
}

