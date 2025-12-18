namespace API.DTOs.SalleReservationsDTOs
{
    public class CreateSalleReservationDTO
    {
        public string SalleReservationName { get; set; } 
        public DateTime ReservedAt { get; set; } = DateTime.Now;
        public int SalleId { get; set; }
    }
}
