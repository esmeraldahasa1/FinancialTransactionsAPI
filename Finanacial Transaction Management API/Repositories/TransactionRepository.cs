using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using Financial_Transaction_Management_API.Models;
using FinancialTransactionAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Finanacial_Transaction_Management_API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<Transaction>> GetAllAsync(Pagination pagination)
        {
            return await _context.Transactions
                .Include(x => x.Customer)
                .OrderByDescending(x => x.TransactionDate)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.TransactionId == id);
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            var existing = await _context.Transactions
                .FirstOrDefaultAsync(x => x.TransactionId == transaction.TransactionId);

            if (existing == null)
                return null;

            existing.Amount = transaction.Amount;
            existing.TransactionType = transaction.TransactionType;
            existing.Description = transaction.Description;
            existing.Status = transaction.Status;
            existing.CustomerId = transaction.CustomerId;
            existing.TransactionDate = transaction.TransactionDate;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllForSummaryAsync()
        {
            return await _context.Transactions.ToListAsync();
        }
    }
}