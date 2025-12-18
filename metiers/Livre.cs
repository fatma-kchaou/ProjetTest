using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class Livre
    {
        [Key]
        public int LivreID { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Author { get; set; }
        
        public int Year { get; set; }
        // Store image data or path. Previous limit of 500 was too small for base64 data URIs.
        // NOTE: Large base64 strings can bloat the database; consider switching to file upload + path storage later.
        [StringLength(8000000)] // Allows ~7MB base64 string for a 5MB image
        public string? Image { get; set; }

        [ForeignKey("CategoryID")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        // Ajout de l’état d’emprunt
        public bool  IsBorrowed { get; set; } = false;
        public DateTime? BorrowedUntil { get; set; }

        public virtual ICollection<BorrowRecord> BorrowRecords { get; set; }

    }
}
