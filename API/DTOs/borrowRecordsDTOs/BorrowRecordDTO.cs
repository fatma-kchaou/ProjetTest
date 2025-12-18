namespace API.DTOs.borrowRecordsDTOs
{
    public class BorrowRecordDTO
    {
        public int BorrowRecordID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int LivreID { get; set; }
        public string LivreTitle { get; set; } 
        public string LivreAuthor { get; set; }
        public bool IsBorrowed { get; set; } 
        public string UserId { get; set; }

        public bool IsReturned => ReturnDate.HasValue;
        public bool IsOverdue => !IsReturned && BorrowDate.AddDays(14) < DateTime.Now; // 14 jours max
    }
}
