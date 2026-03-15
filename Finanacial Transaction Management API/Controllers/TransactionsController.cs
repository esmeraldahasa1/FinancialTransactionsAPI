using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Services;
using Microsoft.AspNetCore.Mvc;
using Financial_Transaction_Management_API.Models;

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
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            var created = await _service.CreateTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] Pagination pagination,
    [FromQuery] TransactionFilter filter)
        {
            var list = await _service.GetAllTransactionsAsync(pagination, filter);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _service.GetTransactionByIdAsync(id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteTransactionAsync(id);
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            var summary = await _service.GetTransactionsSummaryAsync();
            return Ok(summary);
        }
    }
}