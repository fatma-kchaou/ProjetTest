using API.Repositories;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartementController : ControllerBase
    {
        private readonly IDepartementRepository repository;

        public DepartementController(IDepartementRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await repository.GetDepartements());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartementByID(int id)
        {
            var dep = await repository.GetDepartement(id);
            if (dep == null) return NotFound("Dep innexistant");
            return Ok(dep);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartement(Departement departement)
        {
            var deps = await repository.GetDepartements();
            if(deps.Any(d=> d.DepartementName.Equals(departement.DepartementName)))
            {
                return BadRequest("Exist!!");
            }
            var newdep =  await repository.AddDepartement(departement);
            if (newdep != null)
            {
                return CreatedAtAction(nameof(GetDepartementByID),

                    new
                    {
                        id = newdep.DepartementID
                    }, newdep);
            }
                return BadRequest("erreur d'ajout");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDepartement(Departement departement)
        {
            var result = await repository.UpdateDepartement(departement);
            if(result) return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            var result = await repository.DeleteDepartement(id);
            if (result) return NoContent();
            return BadRequest();
        }
    }
}
