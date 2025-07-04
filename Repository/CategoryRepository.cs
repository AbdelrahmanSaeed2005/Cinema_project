using CinemaHub.Data;
using CinemaHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CinemaHub.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context = new();

        //CROD
        public async Task<bool> createAsync(Category category)
        {
            try
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }
            
        }
        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                _context.Update(category);
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
        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                _context.Remove(category);
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
        public async Task<IEnumerable<Category>> GetAsync(Expression<Func<Category , bool>>? expression = null, 
            Expression<Func<Category , object>>[]? includes = null, bool tracked = true
            )
        {
            IQueryable<Category> categories = _context.Categories;
            if (expression is not null)
            {
                categories = categories.Where(expression);
            }
            if (includes is not null)
            {
                foreach(var item in includes)
                {
                    categories = categories.Include(item);
                }
            }
            if (!tracked) {
                categories = categories.AsNoTracking();
            }
            return (await categories.ToListAsync());
        }
        public async Task<Category?>  GetOne(Expression<Func<Category, bool>>? expression = null,
            Expression<Func<Category, object>>[]? includes = null, bool tracked = true) 
        {
            return (await GetAsync(expression, includes, tracked)).FirstOrDefault();
        }
        public async Task<bool> CommitAsync() {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine($"Ex : {ex}");
                // Log the exception (ex) if needed
                return false;
            }
        }
    }
}
