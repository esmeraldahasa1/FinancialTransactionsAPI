using Finanacial_Transaction_Management_API.DTO;
using Finanacial_Transaction_Management_API.Entities;
using Finanacial_Transaction_Management_API.Enums;
using Finanacial_Transaction_Management_API.Models;
using Finanacial_Transaction_Management_API.Repositories.Interfaces;
using System.Text;

namespace Finanacial_Transaction_Management_API.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly ICustomerRepository _customerRepository;

        public TransactionService(
            ITransactionRepository repository,
            ICustomerRepository customerRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
        }

        // Create new 
        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto)
        {
            // Verifiko qe ekzsiton customer
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found");

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                TransactionType = dto.TransactionType,
                Description = dto.Description,
                Status = dto.Status,
                CustomerId = dto.CustomerId,
                TransactionDate = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(transaction);
            return MapToDto(created);
        }

        // Get all by pagination and filters
        public async Task<TransactionListResponse> GetAllTransactionsAsync(Pagination pagination, TransactionFilter filter)
        {
            var transactions = await _repository.GetAllAsync(pagination, filter);

            return new TransactionListResponse
            {
                Pagination = new PaginationDto
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalCount = pagination.TotalCount,
                    TotalPages = pagination.TotalPages,
                    HasPrevious = pagination.HasPrevious,
                    HasNext = pagination.HasNext
                },
                Data = transactions.Select(MapToDto).ToList()
            };
        }

        // Gets by ID
        public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            return transaction != null ? MapToDto(transaction) : null;
        }

        // Updates 
        public async Task<TransactionDto?> UpdateTransactionAsync(int id, UpdateTransactionDto dto)
        {
            var transaction = await _repository.GetByIdAsync(id, true);
            if (transaction == null) return null;

            transaction.Amount = dto.Amount;
            transaction.TransactionType = dto.TransactionType;
            transaction.Description = dto.Description;
            transaction.Status = dto.Status;

            var updated = await _repository.UpdateAsync(transaction);
            return MapToDto(updated);
        }

        // Deletes a transaction me specifed pattern
        public async Task<bool> DeleteTransactionAsync(int id, string deleteType)
        {
            var transaction = await _repository.GetByIdAsync(id, true);
            if (transaction == null) return false;

            if (deleteType.ToLower() == "hard")
                await _repository.HardDeleteAsync(id);
            else
                await _repository.SoftDeleteAsync(id);

            return true;
        }

        // Restore soft deletetd transaction
        public async Task<bool> RestoreTransactionAsync(int id)
        {
            var transaction = await _repository.GetByIdAsync(id, true);
            if (transaction == null || !transaction.IsDeleted) return false;

            await _repository.RestoreAsync(id);
            return true;
        }

        // Get transaction summary with filters
        public async Task<TransactionSummaryDto> GetTransactionsSummaryAsync(TransactionFilter? filter = null)
        {
            return await _repository.GetSummaryAsync(filter ?? new TransactionFilter());
        }

        public async Task<byte[]> ExportTransactionsToCsvAsync(TransactionFilter filter)
        {
            var pagination = new Pagination { PageNumber = 1, PageSize = 10000 };
            var transactions = await _repository.GetAllAsync(pagination, filter);

            var sb = new StringBuilder();

            // Header
            sb.AppendLine("TransactionId,Amount,Type,Date,Status,Description,CustomerName,Phone,Email,Address");

            foreach (var t in transactions)
            {
                sb.AppendLine(string.Join(",",
                    t.TransactionId,
                    t.Amount,
                    t.TransactionType,
                    t.TransactionDate.ToString("yyyy-MM-dd HH:mm"),
                    t.Status,
                    $"\"{t.Description}\"",
                    $"\"{t.Customer?.FullName}\"",
                    t.Customer?.GetMainPhone(),
                    t.Customer?.GetMainEmail(),
                    $"\"{t.Customer?.GetMainAddress()}\""
                ));
            }

            // UTF-8 
            var preamble = Encoding.UTF8.GetPreamble();
            var content = Encoding.UTF8.GetBytes(sb.ToString());

            return preamble.Concat(content).ToArray();
        }

        // Maps Transaction entity to TransactionDto
        private TransactionDto MapToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description,
                Status = transaction.Status,
                CustomerId = transaction.CustomerId,
                CustomerFullName = transaction.Customer?.FullName ?? string.Empty,
                CustomerMainPhone = transaction.Customer?.GetMainPhone() ?? string.Empty,
                CustomerMainEmail = transaction.Customer?.GetMainEmail() ?? string.Empty,
                CustomerMainAddress = transaction.Customer?.GetMainAddress() ?? string.Empty
            };
        }
    }
}