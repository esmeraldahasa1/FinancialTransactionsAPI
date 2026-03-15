using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Models;

namespace Finanacial_Transaction_Management_API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        // CRUD 
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<List<Transaction>> GetAllAsync(Pagination pagination, TransactionFilter filter);
        Task<Transaction?> GetByIdAsync(int id, bool includeDeleted = false);

        // Update 
        Task<Transaction> UpdateAsync(Transaction transaction);

        // Delete 
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);
        Task RestoreAsync(int id);

        // Summary 
        Task<TransactionSummaryDto> GetSummaryAsync(TransactionFilter? filter = null);
    }
}