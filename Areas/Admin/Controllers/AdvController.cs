using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvController : Controller
    {
        private readonly FoodDbContext db = new FoodDbContext();
        public IActionResult Index()
        {
            var record = db.Advs.ToList();
            return View(record);
        }
        public IActionResult Create()
        {
            return View("AddEdit");
        }
        [HttpPost]
        public IActionResult Create(ItemAdv model)
        {
            if (model != null)
            {
                db.Advs.Add(model);
                var fileName = Request.Form.Files[0].FileName;
                if (fileName != "")
                {
                    fileName = UploadImg(fileName);
                }
                model.Photo = fileName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("AddEdit");
        }
        public IActionResult Update(int? id)
        {
            var record = db.Advs.Find(id);
            return View("AddEdit", record);
        }
        [HttpPost]
        public IActionResult Update(int? id, ItemAdv model)
        {
            var record = db.Advs.Find(id);
            if (model != null && record != null)
            {
                record.Position = model.Position;
                record.Name = model.Name;
                DeleteImg(record.Photo);
                var fileName = Request.Form.Files[0].FileName;  
                if (fileName!=null)
                {
                    fileName = UploadImg(fileName);
                    record.Photo = fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("AddEdit", model);
        }
        public IActionResult Delete(int? id)
        {
            var record = db.Advs.Find(id);
            if (record != null)
            {
                db.Advs.Remove(record);
                DeleteImg(record.Photo);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string UploadImg(string fileName)
        {
            if (fileName != "")
            {
                var fileNamestamp = DateTime.Now.ToFileTime();
                fileName = fileNamestamp + "_" + fileName;
                var _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", fileName);
                using (var stream = new FileStream(_path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
            }
            return fileName;
        }
        public void DeleteImg(string fileName)
        {
            bool checkExit = System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", fileName));
            if (checkExit)
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", fileName));
            }
        }
    }
}
