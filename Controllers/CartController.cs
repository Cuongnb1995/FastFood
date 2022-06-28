using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    public class CartController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Index()
        {
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            if (_cart != null)
            {
                ViewBag._cart = _cart;
                ViewBag._total = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity);
            }
            return View(_cart);
        }
        [HttpPost]
        public IActionResult Buy(Item model)
        {
            Cart.CartAdd(HttpContext.Session, model.Id, model.Quantity);
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            var kq = new
            {
                tien = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity),
                soluong = _cart.Select(i=>i.Quantity)
            };
            return new JsonResult(Ok(kq));
        }
        [HttpPost]
        public IActionResult Remove(Item model)
        {
            Cart.CartRemove(HttpContext.Session, model.Id);
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            var kq = new
            {
                tien = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity),
                soluong = _cart.Select(i => i.Quantity)
            };
            return new JsonResult(Ok(kq));
        }
        public IActionResult Destroy()
        {
            Cart.CartDestroy(HttpContext.Session);
            return Redirect("/Cart");
        }
        [HttpPost]
        public IActionResult Update(Item model)
        {
            Cart.CartUpdate(HttpContext.Session, model.Id, model.Quantity);
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            var kq = new
            {
                tien = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity),
                soluong = _cart.Select(i => i.Quantity)
            };
            return new JsonResult(Ok(kq));
        }

        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("customer_email") == null)
                return Redirect("/Account/Login");
            else
            {
                List<Item> _cart = Cart.GetCart(HttpContext.Session);
                int customer_id = int.Parse(HttpContext.Session.GetString("customer_id"));
                ItemOrders _RecordOrder = new ItemOrders();
                _RecordOrder.CustomerId = customer_id;
                _RecordOrder.Create = DateTime.Now;
                _RecordOrder.Price = _cart.Sum(tbl => tbl.ProductRecord.Price * tbl.Quantity);
                db.Orders.Add(_RecordOrder);
                db.SaveChanges();
                int order_id = _RecordOrder.Id;
                foreach (var item in _cart)
                {
                    ItemOrdersDetail _RecordOrdersDetail = new ItemOrdersDetail();
                    _RecordOrdersDetail.OrderId = order_id;
                    _RecordOrdersDetail.ProductId = item.ProductRecord.Id;
                    _RecordOrdersDetail.Price = item.ProductRecord.Price - (item.ProductRecord.Price * item.ProductRecord.Discount) / 100;
                    _RecordOrdersDetail.Quantity = item.Quantity;
                    db.OrdersDetails.Add(_RecordOrdersDetail);
                    db.SaveChanges();
                }
                Cart.CartDestroy(HttpContext.Session);
                return Redirect("/Cart?checkout=success");
            }
        }


    }
}
