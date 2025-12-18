namespace API.DTOs.LivresDTOs
{
    public class CreateLivreDTO
    {
        public string Title { get; set; }
        public string Author { get; set; } 
        public int Year { get; set; }
        public int CategoryID { get; set; }
        public string? Image { get; set; }

    }
}
