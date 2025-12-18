using API.Data;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Salles
{
    public class SallesRepository : ISallesRepository
    {
        private readonly ApplicationContext context;

        public SallesRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Salle> AddSalle(Salle salle)
        {
            await context.Salles.AddAsync(salle);
            await context.SaveChangesAsync();
            return salle;
        }

        public async Task<bool> DeleteSalle(int id)
        {
            var salle = await context.Salles.FindAsync(id);
            if (salle == null)
                return false;
            context.Salles.Remove(salle);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Salle>> GetSalles()
        {
            return await context.Salles.ToListAsync();
        }

        public async Task<Salle> GetSalle(int id)
        {
            return await context.Salles.FindAsync(id);
        }

        public async Task<bool> UpdateSalle(Salle salle)
        {
            var sal = await context.Salles.FindAsync(salle.SalleID);
            if (sal == null)
                return false;
            sal.SalleName = salle.SalleName;
            sal.Capacity = salle.Capacity;
            sal.IsAvailable = salle.IsAvailable;
            await context.SaveChangesAsync();
            return true;
        }
    }
}