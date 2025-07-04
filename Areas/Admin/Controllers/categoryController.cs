using CinemaHub.Data;
using CinemaHub.Models;
using CinemaHub.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class categoryController : Controller
    {
        // readonly ApplicationDbContext _context = new ApplicationDbContext();
        private CategoryRepository _CategoryRepository = new();


        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            var categories = await _CategoryRepository.GetAsync();
            return View(categories);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            //_context.Categories.Add(category);
            //_context.SaveChanges();
            await _CategoryRepository.createAsync(category);
            TempData["success"] = "Category has been added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category =await _CategoryRepository.GetOne(e => e.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/Category/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            //_context.Categories.Update(category);
            //_context.SaveChanges();
            await _CategoryRepository.UpdateAsync(category);
            TempData["success"] = "Category has been updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _CategoryRepository.GetOne(e => e.Id == id);
            if (category == null)
                return NotFound();

            //_context.Categories.Remove(category);
            //_context.SaveChanges();
            await _CategoryRepository.DeleteAsync(category);    
            TempData["success"] = "Category has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
