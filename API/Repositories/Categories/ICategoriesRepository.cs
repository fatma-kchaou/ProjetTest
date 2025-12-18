using metiers;

namespace API.Repositories.Categories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetCategories();

        Task<Category> GetCategory(int id);

        Task<Category> AddCategory(Category category);

        Task<bool> UpdateCategory(Category category);

        Task<bool> DeleteCategory(int id);
    }
}

