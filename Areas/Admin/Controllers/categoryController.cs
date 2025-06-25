using CinemaHub.Data;
using CinemaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class categoryController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        

        // GET: Admin/Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["success"] = "Category has been added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/Category/Edit
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["success"] = "Category has been updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Category/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["success"] = "Category has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
