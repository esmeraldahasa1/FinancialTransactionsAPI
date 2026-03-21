using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using System.Text;

namespace Finanacial_Transaction_Management_API.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        // Krijon new customer
        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            var customer = new Customer
            {
                FullName = createDto.FullName
            };

            // Add main phone
            customer.Phones.Add(new CustomerPhone
            {
                PhoneNumber = createDto.MainPhone,
                IsMain = true
            });

            // Add main email
            customer.Emails.Add(new CustomerEmail
            {
                Email = createDto.MainEmail,
                IsMain = true
            });

            // Add main address
            customer.Addresses.Add(new CustomerAddress
            {
                Address = createDto.MainAddress,
                IsMain = true
            });

            var created = await _repository.CreateAsync(customer);
            return MapToDto(created);
        }

        // Get all
        public async Task<List<CustomerDto>> GetAllCustomersAsync(bool includeDeleted = false)
        {
            var customers = await _repository.GetAllAsync(includeDeleted);
            return customers.Select(MapToDto).ToList();
        }

        // Get by ID
        public async Task<CustomerDto?> GetCustomerByIdAsync(int id, bool includeDeleted = false)
        {
            var customer = await _repository.GetByIdAsync(id, includeDeleted);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<byte[]> ExportCustomersToCsvAsync()
        {
            var customers = await _repository.GetAllAsync(false);

            var sb = new StringBuilder();
            sb.AppendLine("Id,FullName,Phone,Email,Address");

            foreach (var c in customers)
            {
                sb.AppendLine(string.Join(",",
                    c.Id,
                    $"\"{c.FullName}\"",
                    c.GetMainPhone(),
                    c.GetMainEmail(),
                    $"\"{c.GetMainAddress()}\""
                ));
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var content = Encoding.UTF8.GetBytes(sb.ToString());
            return preamble.Concat(content).ToArray();
        }

        private CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                MainPhone = customer.GetMainPhone() ?? string.Empty,
                MainEmail = customer.GetMainEmail() ?? string.Empty,
                MainAddress = customer.GetMainAddress() ?? string.Empty
            };
        }
    }
}