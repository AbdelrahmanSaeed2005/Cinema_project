using CinemaHub.Data;
using CinemaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActoreController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        

        // عرض كل الممثلين
        public IActionResult Index()
        {
            var actors = _context.Actors.ToList();
            return View(actors);
        }

        // صفحة الإضافة
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Actor actor)
        {
            if (!ModelState.IsValid)
                return View(actor);

            _context.Actors.Add(actor);
            _context.SaveChanges();
            TempData["success"] = "Actor has been added successfully!";
            return RedirectToAction("Index");
        }

        // صفحة التعديل
        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        public IActionResult Edit(Actor actor)
        {
            if (!ModelState.IsValid)
                return View(actor);

            _context.Actors.Update(actor);
            _context.SaveChanges();
            TempData["success"] = "Actor has been updated successfully!";
            return RedirectToAction("Index");
        }

        // حذف
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor == null) return NotFound();

            _context.Actors.Remove(actor);
            _context.SaveChanges();
            TempData["success"] = "Actor has been deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
