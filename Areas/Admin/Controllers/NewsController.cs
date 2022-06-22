using FastFood.Areas.Admin.Atrributes;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    //kiem tra login
    [CheckLogin]
    //khai bao bien toan cuc db de thao tac csdl
    public class NewsController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Index(int? page)
        {
            //lay trang  hien tai
            /*
             page ?? 1
                neu page khac null thi _CurrentPage = page
                neu page = null thi _CurrentPage = 1
             */
            int _CurrentPage = page ?? 1;
            //dinh nghia so ban ghi tren mot trang
            int _RecordPerPage = 20;
            //lay tat ca cac ban ghi trong table News
            List<ItemNews> listRecord = db.News.OrderByDescending(item => item.Id).ToList();
            //truyen gia tri ra view co phan trang
            //return Content(HttpContext.Session.GetString("id"));
            return View("Index", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        //mac dinh trang thai cua url la GET
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemNews record = db.News.Where(item => item.Id == _id).FirstOrDefault();
            //tao bien action de dua vao thuoc tinh action cua the form
            ViewBag.action = "/Admin/News/UpdatePost/" + id;
            //goi view, truyen du lieu ra view
            return View("CreateUpdate", record);
        }
        //khi an nut submit thi trang thai cua trang se la POST -> khai bao Attribute [HttpPost]
        [HttpPost]
        public IActionResult Update(IFormCollection fc, int? id)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            int _Hot = fc["Hot"] != "" && fc["Hot"] == "on" ? 1 : 0;
            //---
            int _id = id ?? 0;
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.News.Where(item => item.Id == _id).FirstOrDefault();
            //update ban ghi
            record.Name = _Name;
            record.Description = _Description;
            record.Content = _Content;
            record.Hot = _Hot;
            //--
            //lay thong tin o file co type = file
            string _FileName = "";
            try
            {
                //lay ten file
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_FileName))
            {
                //xoa anh cu
                if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.Photo)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.Photo));
                }
                //upload anh moi
                //lay thoi gian gan vao ten file -> de tranh cac file trung ten nhau luc upload file
                //doi thoi gian hien tai ra giay
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + _FileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _FileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _FileName;
            }
            //cap nhat ban ghi
            db.SaveChanges();
            //--
            //di chuyen den url
            return Redirect("/Admin/News");
            //---
        }
        //mac dinh trang thai cua url la GET
        public IActionResult Create()
        {
            //tao bien action de dua vao thuoc tinh action cua the form
            ViewBag.action = "/Admin/News/CreatePost";
            //goi view, truyen du lieu ra view
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            int _Hot = fc["Hot"] != "" && fc["Hot"] == "on" ? 1 : 0;
            //---
            ItemNews record = new ItemNews();
            //update ban ghi
            record.Name = _Name;
            record.Description = _Description;
            record.Content = _Content;
            record.Hot = _Hot;
            //--
            //lay thong tin o file co type = file
            string _FileName = "";
            try
            {
                //lay ten file
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_FileName))
            {
                //upload anh moi
                //lay thoi gian gan vao ten file -> de tranh cac file trung ten nhau luc upload file
                //doi thoi gian hien tai ra giay
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + _FileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _FileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _FileName;
            }
            //cap nhat ban ghi
            db.News.Add(record);
            db.SaveChanges();
            //--
            //di chuyen den url
            return Redirect("/Admin/News");
            //---
        }
        public IActionResult Delete(int? id)
        {
            //xoa ban ghi
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemNews record = db.News.Where(item => item.Id == _id).FirstOrDefault();
            //xoa anh
            if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.Photo)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.Photo));
            }
            //xoa ban ghi khoi csdl
            db.News.Remove(record);
            //cap nhat lai csdl
            db.SaveChanges();
            //di chuyen den url
            return Redirect("/Admin/News");
        }
    }
}
