using CinemaHub.Data;
using CinemaHub.Models;
using CinemaHub.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class brandController : Controller
    {
        //private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private brandRepository _brandRepository = new();


        // GET: Admin/Cinema
        public async Task<IActionResult> Index()
        {
            var cinemas = await _brandRepository.GetAsync();
            return View(cinemas);
        }

        // GET: Admin/Cinema/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cinema/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cinema cinema)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            //_context.Cinemas.Add(cinema);
            //_context.SaveChanges();
            await _brandRepository.createAsync(cinema);
            TempData["success"] = "Cinema has been added successfully!";
            return RedirectToAction("Index");
        }

        // GET: Admin/Cinema/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cinema =await _brandRepository.GetOne(e => e.Id == id);
            if (cinema == null)
                return NotFound();

            return View(cinema);
        }

        // POST: Admin/Cinema/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cinema cinema)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            //_context.Cinemas.Update(cinema);
            //_context.SaveChanges();
            await _brandRepository.UpdateAsync(cinema);
            TempData["success"] = "Cinema has been updated  successfully!";
            return RedirectToAction("Index");
        }

        // GET: Admin/Cinema/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cinema =await _brandRepository.GetOne(e => e.Id == id);
            if (cinema == null)
                return NotFound();

            //_context.Cinemas.Remove(cinema);
            //_context.SaveChanges();
            await _brandRepository.DeleteAsync(cinema);
            TempData["success"] = "Cinema has been deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
