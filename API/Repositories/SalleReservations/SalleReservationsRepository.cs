using API.Data;
using API.Repositories.SalleReservations;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Reservation
{
    public class SalleReservationsRepository : ISalleReservationsRepository
    {
        private readonly ApplicationContext context;

        public SalleReservationsRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<SalleReservation> AddSalleReservation(SalleReservation salleReservation)
        {
            await context.SalleReservations.AddAsync(salleReservation);
            await context.SaveChangesAsync();
            return salleReservation;
        }

        public async Task<bool> DeleteSalleReservation(int id)
        {
            var salleReservation = await context.SalleReservations.FindAsync(id);
            if (salleReservation == null)
                return false;
            context.SalleReservations.Remove(salleReservation);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SalleReservation> GetSalleReservation(int id)
        {
            return await context.SalleReservations
                .Include(r => r.Salle)
                .FirstOrDefaultAsync(r => r.SalleReservationID == id);
        }

        public async Task<List<SalleReservation>> GetSalleReservations()
        {
            return await context.SalleReservations
                .Include(r => r.Salle)
                .ToListAsync();
        }

        public async Task<bool> UpdateSalleReservation(SalleReservation salleReservation)
        {
            var salleRe = await context.SalleReservations.FindAsync(salleReservation.SalleReservationID);
            if (salleRe == null)
                return false;
            salleRe.SalleReservationName = salleReservation.SalleReservationName;
            salleRe.ReservedAt = salleReservation.ReservedAt;
            salleRe.ReleasedAt = salleReservation.ReleasedAt;
            await context.SaveChangesAsync();
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SalleReservation>> GetSalleReservationsBySalleIdAndDate(int salleId, DateTime date)
        {
            return await context.SalleReservations
                .Include(r => r.Salle)
                .Where(r => r.SalleId == salleId && r.ReservedAt.Date == date.Date)
                .ToListAsync();
        }

        public async Task<List<SalleReservation>> GetSalleReservationsByUserId(string userId)
        {
            return await context.SalleReservations
                .Include(r => r.Salle)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}