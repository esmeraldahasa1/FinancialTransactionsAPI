using Finanacial_Transaction_Management_API.Entities;
using Financial_Transaction_Management_API.Models;

namespace Finanacial_Transaction_Management_API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);

        Task<List<Transaction>> GetAllAsync(Pagination pagination);

        Task<Transaction?> GetByIdAsync(int id);

        Task<Transaction?> UpdateAsync(Transaction transaction);

        Task DeleteAsync(Transaction transaction);

        Task<List<Transaction>> GetAllForSummaryAsync();
    }
}