namespace API.DTOs.SallesDTOs
{
    public class CreateSalleDTO
    {
        public string SalleName { get; set; }          
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;  
    }

}
