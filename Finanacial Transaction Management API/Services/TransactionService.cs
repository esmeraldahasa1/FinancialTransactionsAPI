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

        public async Task<Transaction> CreateTransactionAsync(CreateTransactionDto dto)
        {
            var transaction = new Transaction
            {
                Amount = dto.Amount,
                TransactionType = dto.TransactionType,
                Description = dto.Description,
                Status = dto.Status,
                CustomerId = dto.CustomerId,
                TransactionDate = DateTime.UtcNow
            };

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

        public async Task<Transaction?> UpdateTransactionAsync(int id, Transaction transaction)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return null;

            existing.Amount = transaction.Amount;
            existing.TransactionType = transaction.TransactionType;
            existing.Description = transaction.Description;
            existing.Status = transaction.Status;
            existing.CustomerId = transaction.CustomerId;
            existing.TransactionDate = transaction.TransactionDate;

            return await _repository.UpdateAsync(existing);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);

            if (transaction != null)
                await _repository.DeleteAsync(transaction);
        }

        public async Task<TransactionSummaryDto> GetTransactionsSummaryAsync()
        {
            var transactions = await _repository.GetAllForSummaryAsync();

            var credits = transactions
                .Where(x => x.TransactionType.ToLower() == "credit")
                .Sum(x => x.Amount);

            var debits = transactions
                .Where(x => x.TransactionType.ToLower() == "debit")
                .Sum(x => x.Amount);

            return new TransactionSummaryDto
            {
                TotalTransactions = transactions.Count,
                TotalCredits = credits,
                TotalDebits = debits,
                NetBalance = credits - debits
            };
        }
    }
}