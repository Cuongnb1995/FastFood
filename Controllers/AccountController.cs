using Microsoft.AspNetCore.Mvc;
//su dung doi tuong thao tac IFormCollection, session
using Microsoft.AspNetCore.Http;
//nhin thay cac file .cs ben trong folder Models
using FastFood.Models;
//su dung entity framework
using Microsoft.EntityFrameworkCore;
//phan trang
using X.PagedList;
//nhin thay file CheckLogin.cs tron thu muc Attributes
using FastFood.Areas.Admin.Atrributes;
//doi tuong thao tac file
using System.IO;
//su dung kieu List
using System.Collections.Generic;
using BC = BCrypt.Net.BCrypt;

namespace FastFood.Controllers
{
    public class AccountController : Controller
    {
        //doi tuong thao tac csdl
        public FoodDbContext db = new FoodDbContext();
        //dang ky thanh vien
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
            //ma hoa password
            _password = BC.HashPassword(_password);
            //kiem tra xem email da ton tai trong table customers chua, neu chua thi insert
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
        //dang nhap
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
        //dang xuat
        public IActionResult Logout()
        {
            //huy cac bien session
            HttpContext.Session.Remove("customer_email");
            HttpContext.Session.Remove("customer_id");
            return Redirect("/Account/Login");
        }
    }
}
