using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Repositories.Livres;
using metiers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManager.Tests.Repositories
{
    public class LivresRepositoryTests
    {
        private ApplicationContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationContext(options);


            return context;
        }

        [Fact]
        public async Task AddLivre_ShouldAddLivre()
        {
            var context = GetInMemoryContext();
            var repo = new LivresRepository(context);

            var livre = new Livre { Title = "Test Livre", Author = "Auteur", Year = 2025, Image = "image.png" };
            var result = await repo.AddLivre(livre);

            Assert.NotNull(result);
            Assert.Equal(1, await context.Livres.CountAsync());
            Assert.True(livre.LivreID > 0); // ID généré
        }

        [Fact]
        public async Task DeleteLivre_ShouldReturnTrue_WhenExists()
        {
            var context = GetInMemoryContext();
            var livre = new Livre { Title = "Livre1", Author = "Auteur" };
            context.Livres.Add(livre);
            await context.SaveChangesAsync(); // ID généré

            var repo = new LivresRepository(context);
            var result = await repo.DeleteLivre(livre.LivreID);

            Assert.True(result);
            Assert.Empty(await context.Livres.ToListAsync());
        }

        [Fact]
        public async Task UpdateLivre_ShouldUpdateTitle()
        {
            var context = GetInMemoryContext();
            var livre = new Livre { Title = "Ancien Titre", Author = "Auteur" };
            context.Livres.Add(livre);
            await context.SaveChangesAsync(); // ID généré

            var repo = new LivresRepository(context);
            livre.Title = "Nouveau Titre";
            var result = await repo.UpdateLivre(livre);

            Assert.True(result);

            var updatedLivre = await context.Livres.FirstOrDefaultAsync(l => l.LivreID == livre.LivreID);
            Assert.Equal("Nouveau Titre", updatedLivre.Title);
        }

        [Fact]
        public async Task GetLivres_ShouldReturnAll()
        {
            var context = GetInMemoryContext();
            context.Livres.Add(new Livre { Title = "L1", Author = "A1" });
            context.Livres.Add(new Livre { Title = "L2", Author = "A2" });
            await context.SaveChangesAsync();

            var repo = new LivresRepository(context);
            var livres = await repo.GetLivres();

            Assert.Equal(2, livres.Count);
            Assert.Contains(livres, l => l.Title == "L1");
            Assert.Contains(livres, l => l.Title == "L2");
        }
    }
}
