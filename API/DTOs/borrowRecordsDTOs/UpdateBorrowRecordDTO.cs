namespace API.DTOs.borrowRecordsDTOs
{
    public class UpdateBorrowRecordDTO
    {
        public int? LivreID { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string UserId { get; set; }

    }
}
