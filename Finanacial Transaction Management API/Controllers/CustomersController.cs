using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finanacial_Transaction_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomersController(CustomerService service)
        {
            _service = service;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var created = await _service.CreateCustomerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var list = await _service.GetAllCustomersAsync(includeDeleted);
            return Ok(list);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] bool includeDeleted = false)
        {
            var customer = await _service.GetCustomerByIdAsync(id, includeDeleted);
            if (customer == null)
                return NotFound(new { error = $"Customer with ID {id} not found" });
            return Ok(customer);
        }
    }
}