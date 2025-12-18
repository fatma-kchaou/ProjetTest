using metiers;

namespace API.Repositories
{
    public interface IDepartementRepository
    {
        Task<List<Departement>> GetDepartements();

        Task<Departement> GetDepartement(int id);

        Task<Departement> AddDepartement(Departement departement);

        Task<bool> UpdateDepartement(Departement departement);

        Task<bool> DeleteDepartement(int id);
    }
}
