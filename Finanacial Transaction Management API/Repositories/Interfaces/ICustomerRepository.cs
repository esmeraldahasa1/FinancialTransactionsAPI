using Finanacial_Transaction_Management_API.Entities;

namespace Finanacial_Transaction_Management_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<List<Customer>> GetAllAsync(bool includeDeleted = false);
        Task<Customer?> GetByIdAsync(int id, bool includeDeleted = false);
        Task<bool> ExistsAsync(int id);
    }
}