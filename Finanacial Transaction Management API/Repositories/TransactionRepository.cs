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
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
         .Include(x => x.Customer)
         .FirstOrDefaultAsync(x => x.TransactionId == id);
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