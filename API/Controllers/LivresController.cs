// 2. LivresController.cs
using API.DTOs.LivresDTOs;
using API.Repositories.Livres;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivresController : ControllerBase
    {
        private readonly ILivresRepository repository;

        public LivresController(ILivresRepository repository)
        {
            this.repository = repository;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LivreDTO>> Create(CreateLivreDTO dto)
        {
            var livre = new Livre
            {
                Title = dto.Title,
                Author = dto.Author,
                Year = dto.Year,
                CategoryID = dto.CategoryID,
                Image = dto.Image,
                IsBorrowed = false
            };

            var created = await repository.AddLivre(livre);

            var resultDto = new LivreDTO
            {
                LivreID = created.LivreID,
                Title = created.Title,
                Author = created.Author,
                Year = created.Year,
                CategoryID = created.CategoryID,
                CategoryName = created.Category?.CategoryName ?? "Uncategorized",
                IsBorrowed = false,
                Image = created.Image
            };

            return CreatedAtAction(nameof(Get), new { id = created.LivreID }, resultDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<LivreDTO>>> GetAll()
        {
            var livres = await repository.GetLivres();
            var dtos = livres.Select(l => new LivreDTO
            {
                LivreID = l.LivreID,
                Title = l.Title,
                Author = l.Author,
                Year = l.Year,
                CategoryID = l.CategoryID,
                CategoryName = l.Category?.CategoryName ?? "Uncategorized",
                IsBorrowed = l.IsBorrowed,
                BorrowedUntil = l.BorrowedUntil,
                Image = l.Image
            }).ToList();

            return Ok(dtos);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<LivreDTO>> Get(int id)
        {
            var livre = await repository.GetLivre(id);
            if (livre == null) return NotFound();

            return Ok(new LivreDTO
            {
                LivreID = livre.LivreID,
                Title = livre.Title,
                Author = livre.Author,
                Year = livre.Year,
                CategoryID = livre.CategoryID,
                CategoryName = livre.Category?.CategoryName ?? "Uncategorized",
                IsBorrowed = livre.IsBorrowed,
                BorrowedUntil = livre.BorrowedUntil,
                Image = livre.Image
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LivreDTO>> Update(int id, UpdateLivreDTO dto)
        {
            var livre = await repository.GetLivre(id);
            if (livre == null) return NotFound();

            livre.Title = dto.Title ?? livre.Title;
            livre.Author = dto.Author ?? livre.Author;
            livre.Year = dto.Year ?? livre.Year;
            livre.CategoryID = dto.CategoryID ?? livre.CategoryID;
            livre.Image = dto.Image ?? livre.Image;

            await repository.UpdateLivre(livre);

            return Ok(new LivreDTO
            {
                LivreID = livre.LivreID,
                Title = livre.Title,
                Author = livre.Author,
                Year = livre.Year,
                CategoryID = livre.CategoryID,
                CategoryName = livre.Category?.CategoryName ?? "Uncategorized",
                IsBorrowed = livre.IsBorrowed,
                BorrowedUntil = livre.BorrowedUntil,
                Image = livre.Image
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await repository.DeleteLivre(id);
            return success ? NoContent() : NotFound();
        }
    }
}