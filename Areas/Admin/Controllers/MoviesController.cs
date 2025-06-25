using CinemaHub.Data;
using CinemaHub.Models;
using CinemaHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
       
        //GET: Admin/Movies
        public IActionResult Index()
        {
            var movies = _context.Movies.Include(e => e.Category).Include(e => e.Cinema).ToList();
            return View(movies);
        }
        //GET: Admin/Movies/Create
        public IActionResult Create()
        {
            var vm = new MovieFormVM
            {
                Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Cinemas = _context.Cinemas.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                //Actors = _context.Actors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.LastName}) // ✅
                Actors = _context.Actors.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.FirstName} {a.LastName}"
                })

            };
            return View(vm);
        }

        // POST: Admin/Movies/Create
        [HttpPost]
        public IActionResult Create(MovieFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.Cinemas = _context.Cinemas.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.Actors = _context.Actors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.FirstName} {a.LastName}" });
                return View(vm);
            }

            var movie = new Movie
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = (decimal)vm.Price,
                ImageUrl = vm.ImageUrl,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                //MovieStatus = vm.MovieStatus,
                CategoryId = vm.CategoryId,
                CinemaId = vm.CinemaId
            };

            // ✅ ربط الممثلين بالفيلم
            movie.Actors = _context.Actors
                .Where(a => vm.SelectedActorIds.Contains(a.Id))
                .ToList();

            _context.Movies.Add(movie);
            _context.SaveChanges();
            TempData["success"] = "Movie has been added successfully!";
            return RedirectToAction("Index");
        }


        // GET: Admin/Movies/Edit/5
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Actors) // ✅ لازم Include علشان تجيب العلاقة مع الممثلين
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return NotFound();

            var vm = new MovieFormVM
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = (double)movie.Price,
                ImageUrl = movie.ImageUrl,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                MovieStatus = movie.MovieStatus,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,

                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),

                Cinemas = _context.Cinemas.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),

                // ✅ دي الإضافة المهمة عشان نعرض الممثلين المختارين في الـ Edit
                SelectedActorIds = movie.Actors.Select(a => a.Id).ToList(),
                Actors = _context.Actors.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.FirstName} {a.LastName}"
                })
            };

            return View(vm);
        }


        // POST: Admin/Movies/Edit
        [HttpPost]
        
        public IActionResult Edit(MovieFormVM vm)
        {
            var movie = _context.Movies
                .Include(m => m.Actors) // ✅ لازم عشان تقدر تتحكم في العلاقة
                .FirstOrDefault(m => m.Id == vm.Id);

            if (movie == null) return NotFound();

            // تحديث بيانات الفيلم
            movie.Name = vm.Name;
            movie.Description = vm.Description;
            movie.Price = (decimal)vm.Price;
            movie.ImageUrl = vm.ImageUrl;
            movie.StartDate = vm.StartDate;
            movie.EndDate = vm.EndDate;
            //movie.MovieStatus = vm.MovieStatus;
            movie.CategoryId = vm.CategoryId;
            movie.CinemaId = vm.CinemaId;

            // ✅ تحديث الممثلين المرتبطين
            movie.Actors.Clear(); // حذف الموجودين
            movie.Actors = _context.Actors
                .Where(a => vm.SelectedActorIds.Contains(a.Id))
                .ToList();

            _context.SaveChanges();
            TempData["success"] = "Movie has been updated successfully!";
            return RedirectToAction("Index");
        }


        // GET: Admin/Movies/Delete/5
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            TempData["success"] = "Movie has been deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
