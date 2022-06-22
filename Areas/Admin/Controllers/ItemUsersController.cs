using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;
using FastFood.Areas.Admin.Atrributes;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;
using X.PagedList.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class ItemUsersController : Controller
    {
        /*private readonly FoodDbContext _context;

        public ItemUsersController(FoodDbContext context)
        {
            _context = context;
        }*/
        public readonly FoodDbContext _context = new FoodDbContext();
        // GET: Admin/ItemUsers
        public IActionResult Index(int? page)
        {
            var kq = _context.Users.ToList();
            return View("Index",kq.ToPagedList(page ?? 1, 2));
        }

        // GET: Admin/ItemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var itemUsers = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemUsers == null)
            {
                return NotFound();
            }

            return View(itemUsers);
        }

        // GET: Admin/ItemUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ItemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] ItemUsers itemUsers)
        {
            if (ModelState.IsValid)
            {
                var _user = new ItemUsers();
                _user.Id = itemUsers.Id;
                _user.Name = itemUsers.Name.ToString().Trim();
                _user.Email = itemUsers.Email.ToString().Trim();
                _user.Password = BC.HashPassword(itemUsers.Password);

                _context.Add(_user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemUsers);
        }

        // GET: Admin/ItemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var itemUsers = await _context.Users.FindAsync(id);
            if (itemUsers == null)
            {
                return NotFound();
            }
            return View(itemUsers);
        }

        // POST: Admin/ItemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ItemUsers itemUsers)
        {
            var _user = _context.Users.Where(i=>i.Id == id).FirstOrDefault();
            _user.Name = itemUsers.Name.ToString().Trim();
            if (itemUsers.Password!=null)
            {
                _user.Password = BC.HashPassword(itemUsers.Password);

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/ItemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var itemUsers = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemUsers == null)
            {
                return NotFound();
            }

            return View(itemUsers);
        }

        // POST: Admin/ItemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'FoodDbContext.Users'  is null.");
            }
            var itemUsers = await _context.Users.FindAsync(id);
            if (itemUsers != null)
            {
                _context.Users.Remove(itemUsers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemUsersExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
