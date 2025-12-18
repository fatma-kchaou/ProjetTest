using API.Data;
using metiers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.BorrowRecords
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly ApplicationContext context;

        public BorrowRecordRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<BorrowRecord> AddBorrowRecord(BorrowRecord borrowRecord)
        {
            await context.BorrowRecords.AddAsync(borrowRecord);
            await context.SaveChangesAsync();
            return borrowRecord;
        }

        public async Task<bool> DeleteBorrowRecord(int id)
        {
            var borrow = await context.BorrowRecords.FindAsync(id);
            if (borrow == null)
                return false;
            context.BorrowRecords.Remove(borrow);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<BorrowRecord> GetBorrowRecord(int id)
        {
            return await context.BorrowRecords.FindAsync(id);
        }

        public async Task<List<BorrowRecord>> GetBorrowRecords()
        {
            return await context.BorrowRecords.ToListAsync();
        }

        public async Task<bool> UpdateBorrowRecord(BorrowRecord borrowRecord)
        {
            var borrow = await context.BorrowRecords.FindAsync(borrowRecord.BorrowRecordID);
            if (borrow == null)
                return false;
            borrow.BorrowDate = borrowRecord.BorrowDate;
            borrow.ReturnDate = borrowRecord.ReturnDate;
            borrow.UserId = borrowRecord.UserId;
            borrow.LivreID = borrowRecord.LivreID;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BorrowRecord>> GetBorrowRecordsByUserId(string userId)
        {
            return await context.BorrowRecords
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}