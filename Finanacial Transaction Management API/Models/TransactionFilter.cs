using Finanacial_Transaction_Management_API.Enums;

namespace Finanacial_Transaction_Management_API.Models
{
    public class TransactionFilter
    {
        // Filters bazuar ne Customer Data
        public string? CustomerFullName { get; set; }
        public string? CustomerMainEmail { get; set; }
        public string? CustomerMainPhone { get; set; }
        public string? CustomerMainAddress { get; set; }

        // Transaction filters
        public TransactionType? TransactionType { get; set; }
        public TransactionStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Per soft / hard delete
        public bool IncludeDeleted { get; set; } = false;
    }
}