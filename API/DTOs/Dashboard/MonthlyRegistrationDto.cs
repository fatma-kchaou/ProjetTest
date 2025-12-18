namespace API.DTOs.Dashboard
{

    public class MonthlyRegistrationDto
    {
        public string Month { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
