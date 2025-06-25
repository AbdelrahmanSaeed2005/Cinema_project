using CinemaHub.Data;
using CinemaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class brandController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        

        // GET: Admin/Cinema
        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.ToList();
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
        public IActionResult Create(Cinema cinema)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            TempData["success"] = "Cinema has been added successfully!";
            return RedirectToAction("Index");
        }

        // GET: Admin/Cinema/Edit/5
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null)
                return NotFound();

            return View(cinema);
        }

        // POST: Admin/Cinema/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cinema cinema)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            _context.Cinemas.Update(cinema);
            _context.SaveChanges();
            TempData["success"] = "Cinema has been updated  successfully!";
            return RedirectToAction("Index");
        }

        // GET: Admin/Cinema/Delete/5
        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null)
                return NotFound();

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
            TempData["success"] = "Cinema has been deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
