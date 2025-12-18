namespace API.DTOs.SallesDTOs
{
    public class SalleDTO
    {
        public int SalleID { get; set; }
        public string SalleName { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }  // calculé selon les réservations actives
    }
}
