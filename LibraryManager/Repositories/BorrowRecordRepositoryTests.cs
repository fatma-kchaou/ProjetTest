using System;
using API.Data;
using API.Repositories.BorrowRecords;
using metiers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManager.Tests.Repositories
{
    public class BorrowRecordRepositoryTests
    {
        private DbContextOptions<ApplicationContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AddBorrowRecord_ShouldAddRecord()
        {
            // Arrange
            var options = GetInMemoryOptions();

            using (var context = new ApplicationContext(options))
            {
                var repository = new BorrowRecordRepository(context);

                // Création d'une catégorie et d'un livre
                var category = new Category { CategoryName = "Science" };
                var livre = new Livre
                {
                    Title = "Physique",
                    Author = "Albert",
                    Category = category
                };

                context.Categories.Add(category);
                context.Livres.Add(livre);
                context.SaveChanges(); // Génère les IDs

                // Création d'un enregistrement d'emprunt
                var borrowRecord = new BorrowRecord
                {
                    UserId = "1", // propriété obligatoire
                    LivreID = livre.LivreID,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7)
                };

                // Act
                repository.AddBorrowRecord(borrowRecord);
                context.SaveChanges();

                // Assert
                var savedRecord = context.BorrowRecords.Find(borrowRecord.BorrowRecordID);
                Assert.NotNull(savedRecord);
                Assert.Equal("1", savedRecord.UserId);
                Assert.Equal(livre.LivreID, savedRecord.LivreID);
            }
        }
    }
}
