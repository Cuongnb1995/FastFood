using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace FastFood.Controllers
{
    public class AccountController : Controller
    {
        public FoodDbContext db = new FoodDbContext();
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _name = fc["Name"];
            string _email = fc["Email"];
            string _phone = fc["Phone"];
            string _address = fc["Address"];
            string _password = fc["Password"];
            _password = BC.HashPassword(_password);
            int checkMail = db.Customers.Where(item => item.Email == _email).Count();
            if (checkMail == 0)
            {
                ItemCustomers record = new ItemCustomers();
                record.Name = _name;
                record.Email = _email;
                record.Phone = _phone;
                record.Address = _address;
                record.Password = _password;
                //---
                db.Customers.Add(record);
                db.SaveChanges();
            }
            return Redirect("/Account/Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _email = fc["Email"];
            string _password = fc["Password"];
            ItemCustomers record = db.Customers.Where(item => item.Email == _email).FirstOrDefault();
            if (record != null && BC.Verify(_password, record.Password) == true)
            {
                HttpContext.Session.SetString("customer_email", record.Email.ToString());
                HttpContext.Session.SetString("customer_id", record.Id.ToString());
                return Redirect("/Cart");
            }
            else
                return Redirect("/Account/Login?notify=invalid");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_email");
            HttpContext.Session.Remove("customer_id");
            return Redirect("/Account/Login");
        }
    }
}
