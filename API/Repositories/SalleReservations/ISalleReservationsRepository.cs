using metiers;

namespace API.Repositories.SalleReservations
{
    public interface ISalleReservationsRepository
    {
        Task<List<SalleReservation>> GetSalleReservations();

        Task<SalleReservation> GetSalleReservation(int id);

        Task<SalleReservation> AddSalleReservation(SalleReservation SalleReservation);

        Task<bool> UpdateSalleReservation(SalleReservation SalleReservation);

        Task<bool> DeleteSalleReservation(int id);

        Task<List<SalleReservation>> GetSalleReservationsBySalleIdAndDate(int salleId, DateTime date);

        Task<List<SalleReservation>> GetSalleReservationsByUserId(string userId);
    }
}