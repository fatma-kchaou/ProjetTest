namespace API.DTOs.borrowRecordsDTOs
{
    public class CreateBorrowRecordDTO
    {
        public int LivreID { get; set; }
        public string UserId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }
    }

   
}
