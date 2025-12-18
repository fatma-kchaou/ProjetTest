using System.Linq;
using System.Threading.Tasks;
using API.Repositories.Categories;
using metiers;
using Xunit;

namespace LibraryManager.Tests.Repositories
{
    public class CategoriesRepositoryTests
    {
        [Fact]
        public async Task AddCategory_ShouldAddCategory()
        {
            var context = TestDbContextFactory.Create();
            var repo = new CategoriesRepository(context);

            var cat = new Category { CategoryName = "Informatique" };
            var result = await repo.AddCategory(cat);

            Assert.NotNull(result);
            Assert.Equal(1, context.Categories.Count());
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnTrue_WhenExists()
        {
            var context = TestDbContextFactory.Create();
            var cat = new Category { CategoryName = "Math" };
            context.Categories.Add(cat);
            context.SaveChanges();

            var repo = new CategoriesRepository(context);
            var result = await repo.DeleteCategory(cat.CategoryID);

            Assert.True(result);
            Assert.Empty(context.Categories);
        }

        [Fact]
        public async Task UpdateCategory_ShouldUpdateName()
        {
            var context = TestDbContextFactory.Create();
            var cat = new Category { CategoryName = "Ancien" };
            context.Categories.Add(cat);
            context.SaveChanges();

            var repo = new CategoriesRepository(context);
            cat.CategoryName = "Nouveau";
            var result = await repo.UpdateCategory(cat);

            Assert.True(result);
            Assert.Equal("Nouveau", context.Categories.First().CategoryName);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnAll()
        {
            var context = TestDbContextFactory.Create();
            context.Categories.Add(new Category { CategoryName = "A" });
            context.Categories.Add(new Category { CategoryName = "B" });
            context.SaveChanges();

            var repo = new CategoriesRepository(context);
            var categories = await repo.GetCategories();

            Assert.Equal(2, categories.Count);
        }
    }
}
