using metiers;

namespace API.Repositories.BorrowRecords
{
    public interface IBorrowRecordRepository
    {
        Task<List<BorrowRecord>> GetBorrowRecords();

        Task<BorrowRecord> GetBorrowRecord(int id);

        Task<BorrowRecord> AddBorrowRecord(BorrowRecord borrowRecord);

        Task<bool> UpdateBorrowRecord(BorrowRecord borrowRecord);

        Task<bool> DeleteBorrowRecord(int id);

        Task<List<BorrowRecord>> GetBorrowRecordsByUserId(string userId);
    }
}
