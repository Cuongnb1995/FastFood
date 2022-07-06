using FastFood.Areas.Admin.Atrributes;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [CheckLogin]
    [Area("Admin")]
    public class TagsController : Controller
    {
        private readonly FoodDbContext _db = new FoodDbContext();
        public IActionResult Index()
        {
            var record = _db.Tags.ToList();
            return View(record);
        }
        public IActionResult Add()
        {
            return View("AddEdit");
        }
        [HttpPost]
        public IActionResult Add(ItemTags model)
        {
            if (model != null)
            {
                _db.Tags.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("AddEdit", model);
        }
        public IActionResult Edit(int? id)
        {
            var record = _db.Tags.Find(id);
            if (record != null)
            {
                return View("AddEdit", record);
            }
            else
            {
                return Content("Ko co gi");
            }
        }
        [HttpPost]
        public IActionResult Edit(int? id, ItemTags model)
        {
            var record = _db.Tags.Find(id);
            if (record != null)
            {
                record.Name = model.Name;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("AddEdit", model);
        }
        public IActionResult Delete(int? id)
        {
            var record = _db.Tags.Find(id);
            if (record != null)
            {
                _db.Tags.Remove(record);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Ko co gi");
            }
        }
    }
}
