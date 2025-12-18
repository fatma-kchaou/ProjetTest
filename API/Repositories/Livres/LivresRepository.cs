using API.Data;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Livres
{
    public class LivresRepository : ILivresRepository
    {
        private readonly ApplicationContext context;

        public LivresRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Livre> AddLivre(Livre livre)
        {
            livre.IsBorrowed = false;
            await context.Livres.AddAsync(livre);
            await context.SaveChangesAsync();
            return livre;
        }

        public async Task<bool> DeleteLivre(int id)
        {
            var livre = await context.Livres.FindAsync(id);
            if (livre == null)
                return false;
            context.Livres.Remove(livre);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Livre> GetLivre(int id)
        {
            return await context.Livres.Include(l => l.Category) // ⚠️ Inclure la catégorie
            .FirstOrDefaultAsync(l => l.LivreID == id);
        }

        public async Task<List<Livre>> GetLivres()
        {
            return await context.Livres.Include(l => l.Category) // ⚠️ Inclure la catégorie
            .ToListAsync();
        }

        public async Task<bool> UpdateLivre(Livre livre)
        {
            var liv = await context.Livres.FindAsync(livre.LivreID);
            if (liv == null)
                return false;
            liv.Title = livre.Title;
            liv.Author = livre.Author;
            liv.Year = livre.Year;
            liv.Image = livre.Image;
            liv.IsBorrowed = livre.IsBorrowed;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
