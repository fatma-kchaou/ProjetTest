using metiers;

namespace API.Repositories.Livres
{
    public interface ILivresRepository
    {
        Task<List<Livre>> GetLivres();

        Task<Livre> GetLivre(int id);

        Task<Livre> AddLivre(Livre livre);

        Task<bool> UpdateLivre(Livre livre);

        Task<bool> DeleteLivre(int id);
    }
}
