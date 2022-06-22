using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        public readonly FoodDbContext _context = new FoodDbContext();
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        //doi tuong IFormCollection fc su dung de lay gia tri cua cac the form
        public IActionResult Login(IFormCollection fc)
        {
            //ham Trim() de loai bo khoang trang ben trai, ben phai cua chuoi
            string _email = fc["email"].ToString().Trim();
            string _password = fc["password"].ToString().Trim();
            //lay mot ban ghi tuong ung voi email truyen vao
            ItemUsers record = _context.Users.Where(item => item.Email == _email).FirstOrDefault();
            if (record != null)
            {
                if (BC.Verify(_password, record.Password))
                {
                    //khoi tao bien session
                    HttpContext.Session.SetString("email", _email);
                    //di chuyen den url: /Admin/Home
                    return Redirect("/Admin/ItemUsers");
                }
            }
            return Redirect("/Admin/Account/Login?notify=invalid");
        }
        //dang xuat
        public IActionResult Logout()
        {
            //huy bien session email
            HttpContext.Session.Remove("email");
            return Redirect("/Admin/Account/Login");
        }

    }
}
