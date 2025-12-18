using API.Repositories.Reservation;
using API.Repositories.SalleReservations;
using metiers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManager.Tests.Repositories
{
    public class SalleReservationsRepositoryTests
    {
        [Fact]
        public async Task AddSalleReservation_ShouldAddReservation()
        {
            var context = TestDbContextFactory.Create();
            var repo = new SalleReservationsRepository(context);

            var res = new SalleReservation
            {
                SalleReservationName = "Réservation1",
                ReservedAt = DateTime.Now,
                ReleasedAt = DateTime.Now.AddHours(2)
            };

            var result = await repo.AddSalleReservation(res);

            Assert.NotNull(result);
            Assert.Equal(1, context.SalleReservations.Count());
        }

        [Fact]
        public async Task DeleteSalleReservation_ShouldReturnTrue_WhenExists()
        {
            var context = TestDbContextFactory.Create();
            var res = new SalleReservation { SalleReservationName = "Res1", ReservedAt = DateTime.Now, ReleasedAt = DateTime.Now.AddHours(1) };
            context.SalleReservations.Add(res);
            context.SaveChanges();

            var repo = new SalleReservationsRepository(context);
            var result = await repo.DeleteSalleReservation(res.SalleReservationID);

            Assert.True(result);
            Assert.Empty(context.SalleReservations);
        }

        [Fact]
        public async Task UpdateSalleReservation_ShouldUpdateName()
        {
            var context = TestDbContextFactory.Create();
            var res = new SalleReservation { SalleReservationName = "Ancien", ReservedAt = DateTime.Now, ReleasedAt = DateTime.Now.AddHours(1) };
            context.SalleReservations.Add(res);
            context.SaveChanges();

            var repo = new SalleReservationsRepository(context);
            res.SalleReservationName = "Nouveau";
            var result = await repo.UpdateSalleReservation(res);

            Assert.True(result);
            Assert.Equal("Nouveau", context.SalleReservations.First().SalleReservationName);
        }

        [Fact]
        public async Task GetSalleReservations_ShouldReturnAll()
        {
            var context = TestDbContextFactory.Create();
            context.SalleReservations.Add(new SalleReservation { SalleReservationName = "R1", ReservedAt = DateTime.Now, ReleasedAt = DateTime.Now.AddHours(1) });
            context.SalleReservations.Add(new SalleReservation { SalleReservationName = "R2", ReservedAt = DateTime.Now, ReleasedAt = DateTime.Now.AddHours(2) });
            context.SaveChanges();

            var repo = new SalleReservationsRepository(context);
            var res = await repo.GetSalleReservations();

            Assert.Equal(2, res.Count);
        }
    }
}
