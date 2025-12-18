using System.ComponentModel.DataAnnotations;

namespace API.DTOs.CategoriesDTOs
{
    public class CreateCategoryDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
