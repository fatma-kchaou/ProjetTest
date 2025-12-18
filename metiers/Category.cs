using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metiers
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string CategoryName { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(8000000)] 
        public string? Image { get; set; }
        public ICollection<Livre> Livres { get; set; }
    }
}
