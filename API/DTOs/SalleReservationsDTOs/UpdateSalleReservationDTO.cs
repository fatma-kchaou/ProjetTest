namespace API.DTOs.SalleReservationsDTOs
{
    public class UpdateSalleReservationDTO
    {
        public string? SalleReservationName { get; set; }
        public DateTime? ReservedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public int? SalleId { get; set; }
    }
}
