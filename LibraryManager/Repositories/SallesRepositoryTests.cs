using System.Linq;
using System.Threading.Tasks;
using API.Repositories.Salles;
using metiers;
using Xunit;

namespace LibraryManager.Tests.Repositories
{
    public class SallesRepositoryTests
    {
        [Fact]
        public async Task AddSalle_ShouldAddSalle()
        {
            var context = TestDbContextFactory.Create();
            var repo = new SallesRepository(context);

            var salle = new Salle { SalleName = "Salle A" };
            var result = await repo.AddSalle(salle);

            Assert.NotNull(result);
            Assert.Equal(1, context.Salles.Count());
        }

        [Fact]
        public async Task DeleteSalle_ShouldReturnTrue_WhenExists()
        {
            var context = TestDbContextFactory.Create();
            var salle = new Salle { SalleName = "Salle B" };
            context.Salles.Add(salle);
            context.SaveChanges();

            var repo = new SallesRepository(context);
            var result = await repo.DeleteSalle(salle.SalleID);

            Assert.True(result);
            Assert.Empty(context.Salles);
        }

        [Fact]
        public async Task UpdateSalle_ShouldUpdateName()
        {
            var context = TestDbContextFactory.Create();
            var salle = new Salle { SalleName = "Ancienne" };
            context.Salles.Add(salle);
            context.SaveChanges();

            var repo = new SallesRepository(context);
            salle.SalleName = "Nouvelle";
            var result = await repo.UpdateSalle(salle);

            Assert.True(result);
            Assert.Equal("Nouvelle", context.Salles.First().SalleName);
        }

        [Fact]
        public async Task GetSalles_ShouldReturnAll()
        {
            var context = TestDbContextFactory.Create();
            context.Salles.Add(new Salle { SalleName = "S1" });
            context.Salles.Add(new Salle { SalleName = "S2" });
            context.SaveChanges();

            var repo = new SallesRepository(context);
            var salles = await repo.GetSalles();

            Assert.Equal(2, salles.Count);
        }
    }
}
