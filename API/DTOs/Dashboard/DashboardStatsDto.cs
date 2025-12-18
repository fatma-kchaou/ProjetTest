namespace API.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalListings { get; set; }
        public List<RecentListingDto> RecentListings { get; set; } = new();
      
    }
}
