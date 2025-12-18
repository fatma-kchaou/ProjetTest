using metiers;

namespace API.Repositories.Salles
{
    public interface ISallesRepository
    {
        Task<List<Salle>> GetSalles();

        Task<Salle> GetSalle(int id);

        Task<Salle> AddSalle(Salle salle);

        Task<bool> UpdateSalle(Salle salle);

        Task<bool> DeleteSalle(int id);
    }
}
