using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using Financial_Transaction_Management_API.Models;
using FinancialTransactionAPI.Data;
using Microsoft.EntityFrameworkCore;
using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Enums;

namespace Finanacial_Transaction_Management_API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        // Krijojme nje transaksion te ri
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.IsDeleted = false;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Kthe transaksionin me te dhenat e klientit
            return await _context.Transactions
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Phones)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Emails)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Addresses)
                .FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
        }

        // Merr te gjithe transaksionet me filtera dhe pagination
        public async Task<List<Transaction>> GetAllAsync(Pagination pagination, TransactionFilter filter)
        {
            var query = BuildTransactionQuery(filter);

            // Total count para pagination
            pagination.TotalCount = await query.CountAsync();
            pagination.TotalPages = (int)Math.Ceiling(pagination.TotalCount / (double)pagination.PageSize);

            // Aplikimi i  pagination
            return await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        // Marrim nje transaksion sipas ID
        public async Task<Transaction?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var query = _context.Transactions
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Phones)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Emails)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Addresses)
                .AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(t => !t.IsDeleted);
            }

            return await query.FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        // Perditesojme nje transaksion ekzistues 
        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(transaction.TransactionId, true);
        }

        // Soft delete
        public async Task SoftDeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.IsDeleted = true;
                transaction.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        // Hard delete
        public async Task HardDeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        // Rikthen nje trnsaksion te fshire me soft delete
        public async Task RestoreAsync(int id)
        {
            var transaction = await _context.Transactions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.IsDeleted);

            if (transaction != null)
            {
                transaction.IsDeleted = false;
                transaction.DeletedAt = null;
                await _context.SaveChangesAsync();
            }
        }

        // Merr nje permbledhje te transaksioneve me filter
        public async Task<TransactionSummaryDto> GetSummaryAsync(TransactionFilter? filter = null)
        {
            var query = BuildTransactionQuery(filter ?? new TransactionFilter());

            var transactions = await query.ToListAsync();

            var credits = transactions
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Sum(t => t.Amount);

            var debits = transactions
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Sum(t => t.Amount);

            return new TransactionSummaryDto
            {
                TotalTransactions = transactions.Count,
                TotalCredits = credits,
                TotalDebits = debits,
                NetBalance = credits - debits,
                AppliedFilter = filter != null ? new TransactionSummaryFilter
                {
                    CustomerFullName = filter.CustomerFullName,
                    CustomerEmail = filter.CustomerMainEmail,
                    CustomerPhone = filter.CustomerMainPhone,
                    CustomerAddress = filter.CustomerMainAddress
                } : null
            };
        }

        // Query builder me filetera 
        private IQueryable<Transaction> BuildTransactionQuery(TransactionFilter filter)
        {
            var query = _context.Transactions
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Phones.Where(p => !p.IsDeleted))
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Emails.Where(e => !e.IsDeleted))
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Addresses.Where(a => !a.IsDeleted))
                .AsQueryable();

            // Filtro soft delete 
            if (!filter.IncludeDeleted)
            {
                query = query.Where(t => !t.IsDeleted);
            }

            // Filtera bazuar ne customer 
            if (!string.IsNullOrEmpty(filter.CustomerFullName))
            {
                query = query.Where(t => t.Customer.FullName.Contains(filter.CustomerFullName));
            }

            if (!string.IsNullOrEmpty(filter.CustomerMainEmail))
            {
                query = query.Where(t => t.Customer.Emails.Any(e => e.IsMain && e.Email.Contains(filter.CustomerMainEmail)));
            }

            if (!string.IsNullOrEmpty(filter.CustomerMainPhone))
            {
                query = query.Where(t => t.Customer.Phones.Any(p => p.IsMain && p.PhoneNumber.Contains(filter.CustomerMainPhone)));
            }

            if (!string.IsNullOrEmpty(filter.CustomerMainAddress))
            {
                query = query.Where(t => t.Customer.Addresses.Any(a => a.IsMain && a.Address.Contains(filter.CustomerMainAddress)));
            }

            // Filtera shtese 
            if (filter.TransactionType.HasValue)
            {
                query = query.Where(t => t.TransactionType == filter.TransactionType.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                var toDate = filter.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.TransactionDate <= toDate);
            }

            return query;
        }
    }
}