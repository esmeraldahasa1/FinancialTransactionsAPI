using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Enums;
using Finanacial_Transaction_Management_API.Models;
using Finanacial_Transaction_Management_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finanacial_Transaction_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionsController(TransactionService service)
        {
            _service = service;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            try
            {
                var created = await _service.CreateTransactionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? customerFullName = null,
            [FromQuery] string? customerMainEmail = null,
            [FromQuery] string? customerMainPhone = null,
            [FromQuery] string? customerMainAddress = null,
            [FromQuery] TransactionType? transactionType = null,
            [FromQuery] TransactionStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] bool includeDeleted = false)
        {
            var pagination = new Pagination { PageNumber = pageNumber, PageSize = pageSize };
            var filter = new TransactionFilter
            {
                CustomerFullName = customerFullName,
                CustomerMainEmail = customerMainEmail,
                CustomerMainPhone = customerMainPhone,
                CustomerMainAddress = customerMainAddress,
                TransactionType = transactionType,
                Status = status,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeDeleted = includeDeleted
            };

            var response = await _service.GetAllTransactionsAsync(pagination, filter);
            return Ok(response);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _service.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound(new { error = $"Transaction with ID {id} not found" });
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
        {
            var updated = await _service.UpdateTransactionAsync(id, dto);
            if (updated == null)
                return NotFound(new { error = $"Transaction with ID {id} not found" });
            return Ok(updated);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string type = "soft")
        {
            var validTypes = new[] { "soft", "hard" };
            if (!validTypes.Contains(type.ToLower()))
            {
                return BadRequest(new { error = "Invalid delete type. Use 'soft' or 'hard'" });
            }

            var deleted = await _service.DeleteTransactionAsync(id, type.ToLower());

            if (!deleted)
                return NotFound(new { error = $"Transaction with ID {id} not found" });

            return NoContent();
        }

        
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var restored = await _service.RestoreTransactionAsync(id);

            if (!restored)
                return NotFound(new { error = $"Transaction with ID {id} not found or was not soft-deleted" });

            return NoContent();
        }

        
        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] TransactionFilter? filter = null)
        {
            var summary = await _service.GetTransactionsSummaryAsync(filter ?? new TransactionFilter());
            return Ok(summary);
        }
    }
}