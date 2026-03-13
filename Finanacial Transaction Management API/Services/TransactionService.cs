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
            return await _repository.CreateAsync(transaction);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(Pagination pagination)
        {
            return await _repository.GetAllAsync(pagination);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            if (transaction != null)
            {
                await _repository.DeleteAsync(transaction);
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsSummaryAsync()
        {
            return await _repository.GetAllForSummaryAsync();
        }

    }
}