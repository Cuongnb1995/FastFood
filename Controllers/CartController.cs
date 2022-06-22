using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    public class CartController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Index()
        {
            //lay cac san pham trong gio  hang
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            if (_cart != null)
            {
                ViewBag._cart = _cart;
                ViewBag._total = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity);
            }
            return View(_cart);
        }
        //them san pham vao gio hang
        [HttpPost]
        public IActionResult Buy(Item model)
        {
            //goi ham CartAdd trong class Cart de them phan tu vao gio hang
            Cart.CartAdd(HttpContext.Session, model.Id, model.Quantity);
            //di chuyen den url: /Cart
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            var kq = new
            {
                tien = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity),
                soluong = _cart.Select(i=>i.Quantity)
            };
            return new JsonResult(Ok(kq));
        }
        //xoa san pham khoi gio hang
        [HttpPost]
        public IActionResult Remove(Item model)
        {
            //goi ham CartRemove trong class Cart de xoa phan tu khoi gio hang
            Cart.CartRemove(HttpContext.Session, model.Id);
            //di chuyen den url: /Cart
            List<Item> _cart = Cart.GetCart(HttpContext.Session);
            var kq = new
            {
                tien = _cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity),
                soluong = _cart.Select(i => i.Quantity)
            };
            return new JsonResult(Ok(kq));
        }
        //xoa toan bo san pham khoi gio hang
        public IActionResult Destroy()
        {
            //goi ham CartDestroy trong class Cart de xoa tat ca cac phan tu khoi gio hang
            Cart.CartDestroy(HttpContext.Session);
            //di chuyen den url: /Cart
            return Redirect("/Cart");
        }
        //cap nhat so luong sna pham trong gio hang
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
            //kiem tra neu user chua dang nhap thi yeu cau dang nhap
            if (HttpContext.Session.GetString("customer_email") == null)
                return Redirect("/Account/Login");
            else
            {
                List<Item> _cart = Cart.GetCart(HttpContext.Session);
                //lay customer_id cua session
                int customer_id = int.Parse(HttpContext.Session.GetString("customer_id"));
                //insert du lieu vao table Orders
                ItemOrders _RecordOrder = new ItemOrders();
                _RecordOrder.CustomerId = customer_id;
                _RecordOrder.Create = DateTime.Now;
                _RecordOrder.Price = _cart.Sum(tbl => tbl.ProductRecord.Price * tbl.Quantity);
                db.Orders.Add(_RecordOrder);
                db.SaveChanges();
                //lay id vua insert
                int order_id = _RecordOrder.Id;
                //duyet cac san pham trong session, moi phan tu se add thanh 1 ban ghi trong OrdersDetail
                foreach (var item in _cart)
                {
                    ItemOrdersDetail _RecordOrdersDetail = new ItemOrdersDetail();
                    _RecordOrdersDetail.OrderId = order_id;
                    _RecordOrdersDetail.ProductId = item.ProductRecord.Id;
                    _RecordOrdersDetail.Price = item.ProductRecord.Price - (item.ProductRecord.Price * item.ProductRecord.Discount) / 100;
                    _RecordOrdersDetail.Quantity = item.Quantity;
                    //---
                    db.OrdersDetails.Add(_RecordOrdersDetail);
                    db.SaveChanges();
                }
                //xoa tat cac cac phan tu trong gio hang
                Cart.CartDestroy(HttpContext.Session);
                return Redirect("/Cart?checkout=success");
            }
        }


    }
}
