using CinemaHub.Data;
using CinemaHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace CinemaHub.Repository
{
    public class brandRepository
    {

        private readonly ApplicationDbContext _context = new();

        //CROD
        public async Task<bool> createAsync(Cinema Cinema)
        {
            try
            {
                await _context.AddAsync(Cinema);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }

        }
        public async Task<bool> UpdateAsync(Cinema Cinema)
        {
            try
            {
                _context.Update(Cinema);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }

        }
        public async Task<bool> DeleteAsync(Cinema Cinema)
        {
            try
            {
                _context.Remove(Cinema);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }

        }
        public async Task<IEnumerable<Cinema>> GetAsync(Expression<Func<Cinema, bool>>? expression = null,
            Expression<Func<Cinema, object>>[]? includes = null, bool tracked = true
            )
        {
            IQueryable<Cinema> categories = _context.Cinemas;
            if (expression is not null)
            {
                categories = categories.Where(expression);
            }
            if (includes is not null)
            {
                foreach (var item in includes)
                {
                    categories = categories.Include(item);
                }
            }
            if (!tracked)
            {
                categories = categories.AsNoTracking();
            }
            return (await categories.ToListAsync());
        }
        public async Task<Cinema?> GetOne(Expression<Func<Cinema, bool>>? expression = null,
            Expression<Func<Cinema, object>>[]? includes = null, bool tracked = true)
        {
            return (await GetAsync(expression, includes, tracked)).FirstOrDefault();
        }
        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }
        }
    }
}
