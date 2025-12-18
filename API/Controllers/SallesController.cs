// 5. SallesController.cs
using API.DTOs.SallesDTOs;
using API.Repositories.Salles;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SallesController : ControllerBase
    {
        private readonly ISallesRepository repository;

        public SallesController(ISallesRepository repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SalleDTO>> Create(CreateSalleDTO dto)
        {
            var salle = new Salle { SalleName = dto.SalleName, Capacity = dto.Capacity, IsAvailable = dto.IsAvailable };
            var created = await repository.AddSalle(salle);
            return CreatedAtAction(nameof(Get), new { id = created.SalleID }, new SalleDTO
            {
                SalleID = created.SalleID,
                SalleName = created.SalleName,
                Capacity = created.Capacity,
                IsAvailable = true
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<SalleDTO>>> GetAll() =>
            Ok((await repository.GetSalles()).Select(s => new SalleDTO
            {
                SalleID = s.SalleID,
                SalleName = s.SalleName,
                Capacity = s.Capacity,
                IsAvailable = s.IsAvailable
            }));

        [HttpGet("{id}")]

        public async Task<ActionResult<SalleDTO>> Get(int id)
        {
            var s = await repository.GetSalle(id);
            if (s == null) return NotFound();
            return Ok(new SalleDTO
            {
                SalleID = s.SalleID,
                SalleName = s.SalleName,
                Capacity = s.Capacity,
                IsAvailable = s.IsAvailable
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SalleDTO>> Update(int id, UpdateSalleDTO dto)
        {
            var s = await repository.GetSalle(id);
            if (s == null) return NotFound();
            s.SalleName = dto.SalleName ?? s.SalleName;
            s.Capacity = dto.Capacity ?? s.Capacity;
            s.IsAvailable = dto.IsAvailable ?? s.IsAvailable;
            await repository.UpdateSalle(s);
            return Ok(new SalleDTO
            {
                SalleID = s.SalleID,
                SalleName = s.SalleName,
                Capacity = s.Capacity,
                IsAvailable = s.IsAvailable
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) =>
            await repository.DeleteSalle(id) ? NoContent() : NotFound();
    }
}