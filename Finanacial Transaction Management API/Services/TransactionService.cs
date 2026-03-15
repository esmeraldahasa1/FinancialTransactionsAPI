using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using Financial_Transaction_Management_API.Models;

namespace Finanacial_Transaction_Management_API.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            transaction.TransactionDate = DateTime.UtcNow;

            return await _repository.CreateAsync(transaction);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(
            Pagination pagination,
            TransactionFilter filter)
        {
            return await _repository.GetAllAsync(pagination, filter);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }

        public async Task<TransactionSummaryDto> GetTransactionsSummaryAsync(
            TransactionFilter? filter = null)
        {
            return await _repository.GetSummaryAsync(filter);
        }
    }
}