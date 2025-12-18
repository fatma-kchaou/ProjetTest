namespace API.DTOs.Dashboard
{
    public class RecentListingDto
    {
        public int LivreID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
