using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;
using X.PagedList;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        public readonly FoodDbContext _context = new FoodDbContext();

        // GET: Admin/Categories
        public IActionResult Index(int? page)
        {
            var kq = _context.categories.Where(i => i.ParentId == 0).ToList();

            return _context.categories != null ?
                        View("Index", kq.ToPagedList(page ?? 1, 40)) :
                        Problem("Entity set 'FoodDbContext.categories'  is null.");

        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentId,Name")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var categories = await _context.categories.FindAsync(id);
            var a = _context.categories.Where(i=>i.Id == id).ToList();
            ViewBag.CategoriesEdit = a[0].ParentId;
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,Name")] Categories categories)
        {
            var kq = _context.categories.Where(i => i.Id == id).FirstOrDefault();
            kq.Name = categories.Name;
            kq.ParentId = categories.ParentId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var categories = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categories == null)
            {
                return Problem("Entity set 'FoodDbContext.categories'  is null.");
            }
            var categories = await _context.categories.FindAsync(id);
            if (categories != null)
            {
                _context.categories.Remove(categories);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return (_context.categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
