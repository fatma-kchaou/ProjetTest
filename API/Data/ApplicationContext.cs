using metiers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Departement> Departements { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        public DbSet<Livre> Livres { get; set; }

        public DbSet<SalleReservation> SalleReservations { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relation Category - Livre avec suppression en cascade
            builder.Entity<Category>()
                   .HasMany(c => c.Livres)
                   .WithOne(l => l.Category)
                   .HasForeignKey(l => l.CategoryID) 
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

