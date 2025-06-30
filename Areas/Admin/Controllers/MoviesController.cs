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

        public IActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Category).Include(m => m.Cinema).ToList();
            return View(movies);
        }

        public IActionResult Create()
        {
            var vm = new MovieFormVM
            {
                Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Cinemas = _context.Cinemas.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Actors = _context.Actors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.FirstName} {a.LastName}" })
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieFormVM vm, IFormFile img)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.Cinemas = _context.Cinemas.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.Actors = _context.Actors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.FirstName} {a.LastName}" });
                return View(vm);
            }

            // ✅ حفظ صورة رئيسية
            if (img != null && img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                }
                vm.ImageUrl = fileName;
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
                CinemaId = vm.CinemaId,
                Actors = _context.Actors.Where(a => vm.SelectedActorIds.Contains(a.Id)).ToList()
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(); // علشان ناخد الـ movie.Id

            // ✅ حفظ الصور المتعددة
            if (vm.GalleryImage != null && vm.GalleryImage.Any())
            {
                foreach (var galleryImage in vm.GalleryImage)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(galleryImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await galleryImage.CopyToAsync(stream);
                    }

                    var movieImage = new MovieImage
                    {
                        ImageUrl = fileName,
                        MovieId = movie.Id
                    };
                    _context.MaovieImages.Add(movieImage);
                }
                await _context.SaveChangesAsync();
            }

            // ✅ حفظ الفيديو
            if (vm.TrailerVideo != null)
            {
                var videoName = Guid.NewGuid() + Path.GetExtension(vm.TrailerVideo.FileName);
                var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Videos", videoName);
                using (var stream = System.IO.File.Create(videoPath))
                {
                    await vm.TrailerVideo.CopyToAsync(stream);
                }

                movie.TrailerVideoUrl = "/Videos/" + videoName;
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Movie has been added successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.GalleryImage) // جلب الصور الإضافية
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
                SelectedActorIds = movie.Actors.Select(a => a.Id).ToList(),

                Categories = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Cinemas = _context.Cinemas.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                Actors = _context.Actors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = $"{a.FirstName} {a.LastName}" })
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieFormVM vm, IFormFile img1)
        {
            var movie = _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefault(m => m.Id == vm.Id);

            if (movie == null) return NotFound();

            // ✅ تحديث الصورة الرئيسية
            if (img1 != null && img1.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img1.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await img1.CopyToAsync(stream);
                }
                movie.ImageUrl = fileName;
            }

            // تحديث باقي بيانات الفيلم
            movie.Name = vm.Name;
            movie.Description = vm.Description;
            movie.Price = (decimal)vm.Price;
            movie.StartDate = vm.StartDate;
            movie.EndDate = vm.EndDate;
            //movie.MovieStatus = vm.MovieStatus;
            movie.CategoryId = vm.CategoryId;
            movie.CinemaId = vm.CinemaId;

            // ✅ تحديث الممثلين
            movie.Actors.Clear();
            movie.Actors = _context.Actors.Where(a => vm.SelectedActorIds.Contains(a.Id)).ToList();

            // ✅ إضافة صور جديدة
            if (vm.GalleryImage != null && vm.GalleryImage.Any())
            {
                foreach (var img in vm.GalleryImage)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var movieImage = new MovieImage
                    {
                        ImageUrl = fileName,
                        MovieId = movie.Id
                    };
                    _context.MaovieImages.Add(movieImage);
                }
            }

            // ✅ حفظ الفيديو الجديد إن وجد
            if (vm.TrailerVideo != null)
            {
                var videoName = Guid.NewGuid() + Path.GetExtension(vm.TrailerVideo.FileName);
                var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Videos", videoName);
                using (var stream = System.IO.File.Create(videoPath))
                {
                    await vm.TrailerVideo.CopyToAsync(stream);
                }

                movie.TrailerVideoUrl = "/Videos/" + videoName;
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Movie has been updated successfully!";
            return RedirectToAction("Index");
        }

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
