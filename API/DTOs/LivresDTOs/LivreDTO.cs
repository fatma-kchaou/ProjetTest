    namespace API.DTOs.LivresDTOs
    {
        public class LivreDTO
        {
            public int LivreID { get; set; }
            public string Title { get; set; } 
            public string Author { get; set; } 
            public int Year { get; set; }
            public int CategoryID { get; set; }
            public string CategoryName { get; set; } 

            // Optionnel : indique si le livre est déjà emprunté
            public bool IsBorrowed { get; set; } = false; // sera false par défaut
            public DateTime? BorrowedUntil { get; set; } = null;
            public string? Image { get; set; }

        }
    }
