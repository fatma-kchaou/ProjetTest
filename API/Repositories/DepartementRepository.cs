using API.Data;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class DepartementRepository : IDepartementRepository
    {
        private readonly ApplicationContext context;

        public DepartementRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task<Departement> AddDepartement(Departement departement)
        {
            await context.Departements.AddAsync(departement);
            await context.SaveChangesAsync();
            return departement;
        }

        public async Task<bool> DeleteDepartement(int id)
        {
            var deaprtement = await context.Departements.FindAsync(id);
            if (deaprtement == null)
                return false;
            context.Departements.Remove(deaprtement);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Departement> GetDepartement(int id)
        {
           return await context.Departements.FindAsync(id);
        }

        public async Task<List<Departement>> GetDepartements()
        {
           return await context.Departements.ToListAsync();
        }

        public async Task<bool> UpdateDepartement(Departement departement)
        {
            var dep = await context.Departements.FindAsync(departement.DepartementID);
            if (dep == null)
                return false;
            dep.DepartementName = departement.DepartementName;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
