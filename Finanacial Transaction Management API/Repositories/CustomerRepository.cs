using Finanacial_Transaction_Management_API.Data;
using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Finanacial_Transaction_Management_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        // New Customer
        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return await _context.Customers
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == customer.Id);
        }

        // Gets all customers 
        public async Task<List<Customer>> GetAllAsync(bool includeDeleted = false)
        {
            var query = _context.Customers
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.Addresses)
                .AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(c => !c.IsDeleted);
            }

            return await query.ToListAsync();
        }

        // Get by ID
        public async Task<Customer?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var query = _context.Customers
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.Addresses)
                .AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(c => !c.IsDeleted);
            }

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        // Kontrollon ekzistencen per customer
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}