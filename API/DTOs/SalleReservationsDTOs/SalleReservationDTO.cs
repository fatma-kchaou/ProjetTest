namespace API.DTOs.SalleReservationsDTOs
{
    public class SalleReservationDTO
    {
        public int SalleReservationID { get; set; }
        public string SalleReservationName { get; set; } 
        public DateTime ReservedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public bool IsActive => !ReleasedAt.HasValue;

        public int SalleId { get; set; }
        public string? SalleName { get; set; } 
        public string? UserId { get; set; }
    }
}
