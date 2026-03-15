using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Entities;
using Financial_Transaction_Management_API.Models;

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
        Task SoftDeleteAsync(int id); // Soft delete
        Task HardDeleteAsync(int id); // Hard delete
        Task RestoreAsync(int id); // Restore soft-deleted

        // Summary me filtera
        Task<TransactionSummaryDto> GetSummaryAsync(TransactionFilter? filter = null);
    }
}