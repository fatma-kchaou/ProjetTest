namespace API.DTOs.SallesDTOs
{
    public class UpdateSalleDTO
    {
        public string? SalleName { get; set; }
        public int? Capacity { get; set; }  
        public bool? IsAvailable { get; set; }
    }
}
