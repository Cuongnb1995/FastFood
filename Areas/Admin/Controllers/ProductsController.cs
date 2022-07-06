using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;//doc thong tin trong file appsetting.json
using System.Data;//su dung DataTable
using System.Data.SqlClient; //su dung cho doi tuong SqlConnection,SqlDataAdapter, SqlCommand
using X.PagedList; //phan trang
using Microsoft.AspNetCore.Http;//su dung IFormCollection
using System.IO;//doi tuong thao tac voi file, folder
//doi tuong ma hoa password
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;
using FastFood.Areas.Admin.Atrributes;
using X.PagedList;
using X.PagedList.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    //khai bao tag Area Admin
    [Area("Admin")]
    //kiem tra login
    [CheckLogin]
    public class ProductsController : Controller
    {
        //khai bao bien toan cuc db de thao tac csdl
        public readonly FoodDbContext db = new FoodDbContext();
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
            int _RecordPerPage = 8;
            //lay tat ca cac ban ghi trong table Products
            List<ItemProducts> listRecord = db.Products.OrderByDescending(item => item.Id).ToList();
            //truyen gia tri ra view co phan trang
            //return Content(HttpContext.Session.GetString("id"));
            return View("Index", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        //mac dinh trang thai cua url la GET
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemProducts record = db.Products.Where(item => item.Id == _id).FirstOrDefault();
            //tao bien action de dua vao thuoc tinh action cua the form
            ViewBag.action = "/Admin/Products/UpdatePost/" + id;
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
            double _Price = Convert.ToDouble(fc["Price"].ToString().Trim());
            double _Discount = Convert.ToDouble(fc["Discount"].ToString().Trim());
            int _CategoryId = Convert.ToInt32(fc["CategoryId"].ToString().Trim());
            //---
            int _id = id ?? 0;
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.Products.Where(item => item.Id == _id).FirstOrDefault();
            //update ban ghi
            record.Name = _Name;
            record.Description = _Description;
            record.Content = _Content;
            record.Hot = _Hot;
            record.Price = _Price;
            record.Discount = _Discount;
            record.CategoryId = _CategoryId;
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
                if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo));
                }
                //upload anh moi
                //lay thoi gian gan vao ten file -> de tranh cac file trung ten nhau luc upload file
                //doi thoi gian hien tai ra giay
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + _FileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _FileName);
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

            var recordTag = db.Tags.ToList();
            var _productsTags = db.ProductsTags.Where(i => i.ProductId == id).ToList();
            db.ProductsTags.RemoveRange(_productsTags);
            foreach (var item in recordTag)
            {
                string name = "tag_" + item.Id;
                if (!String.IsNullOrEmpty(Request.Form[name]))
                {
                    ItemProductsTags recordProductsTags = new ItemProductsTags();
                    recordProductsTags.ProductId = record.Id;
                    recordProductsTags.TagId = item.Id;
                    db.ProductsTags.Add(recordProductsTags);
                }
            }
            db.SaveChanges();

            return Redirect("/Admin/Products");
            //---
        }
        //mac dinh trang thai cua url la GET
        public IActionResult Create()
        {
            //tao bien action de dua vao thuoc tinh action cua the form
            ViewBag.action = "/Admin/Products/CreatePost";
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
            double _Price = Convert.ToDouble(fc["Price"].ToString().Trim());
            double _Discount = Convert.ToDouble(fc["Discount"].ToString().Trim());
            int _CategoryId = Convert.ToInt32(fc["CategoryId"].ToString().Trim());
            //---
            ItemProducts record = new ItemProducts();
            //update ban ghi
            record.Name = _Name;
            record.Description = _Description;
            record.Content = _Content;
            record.Hot = _Hot;
            record.Price = _Price;
            record.Discount = _Discount;
            record.CategoryId = _CategoryId;
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
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _FileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _FileName;
            }
            //cap nhat ban ghi
            db.Products.Add(record);
            db.SaveChanges();


            var recordTag = db.Tags.ToList();
            foreach (var item in recordTag)
            {
                string name = "tag_" + item.Id;
                if (!String.IsNullOrEmpty(Request.Form[name]))
                {
                    ItemProductsTags recordProductsTags = new ItemProductsTags();
                    recordProductsTags.ProductId = record.Id;
                    recordProductsTags.TagId = item.Id;
                    db.ProductsTags.Add(recordProductsTags);
                }
            }
            db.SaveChanges();
            //--
            //di chuyen den url
            return Redirect("/Admin/Products");
            //---
        }
        public IActionResult Delete(int? id)
        {
            //xoa ban ghi
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemProducts record = db.Products.Where(item => item.Id == _id).FirstOrDefault();
            //xoa anh
            if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo));
            }
            //xoa ban ghi khoi csdl
            db.Products.Remove(record);
            //cap nhat lai csdl
            db.SaveChanges();
            //di chuyen den url
            return Redirect("/Admin/Products");
        }
    }
}
