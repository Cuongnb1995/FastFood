using FastFood.Areas.Admin.Atrributes;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class OrdersController : Controller
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
            List<ItemOrders> listRecord = db.Orders.OrderByDescending(item => item.Id).ToList();
            //truyen gia tri ra view co phan trang
            //return Content(HttpContext.Session.GetString("id"));
            return View("Index", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Detail(int? id)
        {
            int _OrderId = id ?? 0;
            ViewBag.OrderId = _OrderId;
            List<ItemOrdersDetail> _ListRecord = db.OrdersDetails.Where(i => i.OrderId == _OrderId).ToList();
            return View("Detail", _ListRecord);
        }
        public IActionResult Delivery(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrders record = db.Orders.Where(i => i.Id == _OrderId).FirstOrDefault();
            if (record != null)
            {
                record.Status = 1;
                db.SaveChanges();
            }
            return Redirect("/Admin/Orders");
        }
        public IActionResult CancelOrder(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrders record = db.Orders.Where(i => i.Id == _OrderId).FirstOrDefault();
            if (record != null)
            {
                record.Status = 2;
                db.SaveChanges();
            }
            return Redirect("/Admin/Orders");
        }
    }
}
