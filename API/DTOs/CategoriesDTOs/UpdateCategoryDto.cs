using System.ComponentModel.DataAnnotations;

namespace API.DTOs.CategoriesDTOs
{
    public class UpdateCategoryDTO
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
