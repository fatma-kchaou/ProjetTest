using API.Data;
using API.Repositories.Categories;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationContext context;

        public CategoriesRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task<Category> AddCategory(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
                return false;
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Category> GetCategory(int id)
        {
            return await context.Categories
                .Include(c => c.Livres)
                .FirstOrDefaultAsync(c => c.CategoryID == id);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var categ = await context.Categories.FindAsync(category.CategoryID);
            if (categ == null)
                return false;
            categ.CategoryName = category.CategoryName;
            await context.SaveChangesAsync();
            return true;
        }
    }
}