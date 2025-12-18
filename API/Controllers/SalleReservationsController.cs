// 6. SalleReservationsController.cs
using API.DTOs.SalleReservationsDTOs;
using API.Repositories.SalleReservations;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalleReservationsController : ControllerBase
    {
        private readonly ISalleReservationsRepository repository;

        public SalleReservationsController(ISalleReservationsRepository repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<SalleReservationDTO>> Create(CreateSalleReservationDTO dto)
        {
            // Check availability
            var existingReservations = await repository.GetSalleReservationsBySalleIdAndDate(dto.SalleId, dto.ReservedAt);
            if (existingReservations.Any())
            {
                return BadRequest("La salle est déjà réservée pour cette date.");
            }

            var reservation = new SalleReservation
            {
                SalleReservationName = dto.SalleReservationName,
                ReservedAt = dto.ReservedAt,
                SalleId = dto.SalleId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var created = await repository.AddSalleReservation(reservation);

            return CreatedAtAction(nameof(GetAll), new { id = created.SalleReservationID }, new SalleReservationDTO
            {
                SalleReservationID = created.SalleReservationID,
                SalleReservationName = created.SalleReservationName,
                ReservedAt = created.ReservedAt,
                ReleasedAt = created.ReleasedAt,
                SalleId = created.SalleId,
                SalleName = created.Salle?.SalleName,
                UserId = created.UserId
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<List<SalleReservationDTO>>> GetAll()
        {
            List<SalleReservation> list;
            
            if (User.IsInRole("Admin"))
            {
                list = await repository.GetSalleReservations();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();
                list = await repository.GetSalleReservationsByUserId(userId);
            }

            return Ok(list.Select(r => new SalleReservationDTO
            {
                SalleReservationID = r.SalleReservationID,
                SalleReservationName = r.SalleReservationName,
                ReservedAt = r.ReservedAt,
                ReleasedAt = r.ReleasedAt,
                SalleId = r.SalleId,
                SalleName = r.Salle?.SalleName,
                UserId = r.UserId
            }));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Client")]

        public async Task<ActionResult<SalleReservationDTO>> GetById(int id)
        {
            var reservation = await repository.GetSalleReservation(id);
            if (reservation == null)
                return NotFound();
            return Ok(new SalleReservationDTO
            {
                SalleReservationID = reservation.SalleReservationID,
                SalleReservationName = reservation.SalleReservationName,
                ReservedAt = reservation.ReservedAt,
                ReleasedAt = reservation.ReleasedAt,
                SalleId = reservation.SalleId,
                SalleName = reservation.Salle?.SalleName,
                UserId = reservation.UserId
            });
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Client")]

        public async Task<ActionResult> Update(SalleReservationDTO dto)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            var reservation = await repository.GetSalleReservation(dto.SalleReservationID);
            if (reservation == null)
                return NotFound();
            reservation.SalleReservationName = dto.SalleReservationName;
            reservation.ReservedAt = dto.ReservedAt;
            reservation.ReleasedAt = dto.ReleasedAt;
            reservation.SalleId = dto.SalleId;
            await repository.UpdateSalleReservation(reservation);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Client")]

        public async Task<ActionResult> Delete(int id)
        {
            var reservation = await repository.GetSalleReservation(id);
            if (reservation == null)
                return NotFound();
            await repository.DeleteSalleReservation(id);
            return Ok();
        }
    }
}