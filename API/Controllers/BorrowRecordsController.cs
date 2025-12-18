// 4. BorrowRecordsController.cs
using API.DTOs.borrowRecordsDTOs;
using API.Repositories.BorrowRecords;
using API.Repositories.Livres;
using metiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordRepository borrowRepo;
        private readonly ILivresRepository livreRepo;

        public BorrowRecordsController(IBorrowRecordRepository borrowRepo, ILivresRepository livreRepo)
        {
            this.borrowRepo = borrowRepo;
            this.livreRepo = livreRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<BorrowRecordDTO>> Create(CreateBorrowRecordDTO dto)
        {
            var livre = await livreRepo.GetLivre(dto.LivreID);
            if (livre == null) return NotFound("Livre not found");
            if (livre.IsBorrowed) return BadRequest("Already borrowed");

            var record = new BorrowRecord
            {
                LivreID = dto.LivreID,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                BorrowDate = dto.BorrowDate != default ? dto.BorrowDate : DateTime.Now,
                ReturnDate = dto.ReturnDate
            };

            var created = await borrowRepo.AddBorrowRecord(record);

            livre.IsBorrowed = true;
            livre.BorrowedUntil = dto.ReturnDate ?? DateTime.Now.AddDays(14);
            await livreRepo.UpdateLivre(livre);

            return CreatedAtAction(nameof(Get), new { id = created.BorrowRecordID }, new BorrowRecordDTO
            {
                BorrowRecordID = created.BorrowRecordID,
                BorrowDate = created.BorrowDate,
                ReturnDate = created.ReturnDate,
                LivreID = livre.LivreID,
                LivreTitle = livre.Title,
                LivreAuthor = livre.Author,
                UserId = created.UserId,
              
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<List<BorrowRecordDTO>>> GetAll()
        {
            List<BorrowRecord> records;
            
            if (User.IsInRole("Admin"))
            {
                records = await borrowRepo.GetBorrowRecords();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();
                records = await borrowRepo.GetBorrowRecordsByUserId(userId);
            }

            var dtos = new List<BorrowRecordDTO>();
            foreach (var r in records)
            {
                var livre = await livreRepo.GetLivre(r.LivreID);
                dtos.Add(new BorrowRecordDTO
                {
                    BorrowRecordID = r.BorrowRecordID,
                    BorrowDate = r.BorrowDate,
                    ReturnDate = r.ReturnDate,
                    LivreID = r.LivreID,
                    LivreTitle = livre?.Title ?? "Unknown",
                    LivreAuthor = livre?.Author ?? "",
                    UserId = r.UserId,
                   
                });
            }
            return Ok(dtos);
        }

        [HttpPut("{id}/return")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var record = await borrowRepo.GetBorrowRecord(id);
            if (record == null) return NotFound();

            record.ReturnDate = DateTime.Now;
            await borrowRepo.UpdateBorrowRecord(record);

            var livre = await livreRepo.GetLivre(record.LivreID);
            if (livre != null)
            {
                livre.IsBorrowed = false;
                livre.BorrowedUntil = null;
                await livreRepo.UpdateLivre(livre);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await borrowRepo.DeleteBorrowRecord(id);
            if (!success) return NotFound();
            return NoContent();
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<ActionResult<BorrowRecordDTO>> Get(int id)
        {
            var r = await borrowRepo.GetBorrowRecord(id);
            if (r == null) return NotFound();
            var livre = await livreRepo.GetLivre(r.LivreID);
            var dto = new BorrowRecordDTO
            {
                BorrowRecordID = r.BorrowRecordID,
                BorrowDate = r.BorrowDate,
                ReturnDate = r.ReturnDate,
                LivreID = r.LivreID,
                LivreTitle = livre?.Title ?? "Unknown",
                LivreAuthor = livre?.Author ?? "",
                UserId = r.UserId,
               
            };
            return Ok(dto);
        }
        
    }
}