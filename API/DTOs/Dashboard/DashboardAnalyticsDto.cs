namespace API.DTOs.Dashboard
{
    public class DashboardAnalyticsDto
    {
      
        public List<CategoryStatsDto> ListingsByCategory { get; set; } = new();
        public List<MonthlyRegistrationDto> UserRegistrations { get; set; } = new();
    }
}
