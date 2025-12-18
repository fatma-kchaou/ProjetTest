// 3. CategoriesController.cs
using API.DTOs.CategoriesDTOs;
using API.DTOs.LivresDTOs;
using API.Repositories.Categories;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository repository;

        public CategoriesController(ICategoriesRepository repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> Create(CreateCategoryDTO dto)
        {
            var cat = new Category { CategoryName = dto.CategoryName, Description = dto.Description, Image = dto.Image };
            var created = await repository.AddCategory(cat);
            return CreatedAtAction(nameof(Get), new { id = created.CategoryID }, new CategoryDTO
            {
                CategoryID = created.CategoryID,
                CategoryName = created.CategoryName,
                Description = created.Description,
                Image = created.Image
            });
        }

        [HttpGet]

        public async Task<ActionResult<List<CategoryDTO>>> GetAll()
        {
            var cats = await repository.GetCategories();
            return Ok(cats.Select(c => new CategoryDTO
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Image = c.Image
            }));
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var cat = await repository.GetCategory(id);
            if (cat == null) return NotFound();
            return Ok(new CategoryDTO
            {
                CategoryID = cat.CategoryID,
                CategoryName = cat.CategoryName,
                Description = cat.Description,
                Image = cat.Image,
                Livres = cat.Livres?.Select(l => new LivreDTO 
                { 
                    LivreID = l.LivreID,
                    Title = l.Title,
                    Author = l.Author,
                    Year = l.Year,
                    CategoryID = l.CategoryID,
                    CategoryName = cat.CategoryName,
                    IsBorrowed = l.IsBorrowed,
                    BorrowedUntil = l.BorrowedUntil,
                    Image = l.Image
                }).ToList() ?? new List<LivreDTO>()
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> Update(int id, UpdateCategoryDTO dto)
        {
            var cat = await repository.GetCategory(id);
            if (cat == null) return NotFound();
            cat.CategoryName = dto.CategoryName ?? cat.CategoryName;
            cat.Description = dto.Description ?? cat.Description;
            cat.Image = dto.Image ?? cat.Image;
            await repository.UpdateCategory(cat);
            return Ok(new CategoryDTO
            {
                CategoryID = cat.CategoryID,
                CategoryName = cat.CategoryName,
                Description = cat.Description,
                Image = cat.Image
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return await repository.DeleteCategory(id) ? NoContent() : NotFound();
        }

    }
}