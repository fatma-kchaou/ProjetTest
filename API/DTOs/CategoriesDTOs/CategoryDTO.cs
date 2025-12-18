using API.DTOs.LivresDTOs;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.CategoriesDTOs
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<LivreDTO> Livres { get; set; } = new();
    }
}
