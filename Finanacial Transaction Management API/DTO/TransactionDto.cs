using Finanacial_Transaction_Management_API.Enums;
using Financial_Transaction_Management_API.Enums;

namespace Finanacial_Transaction_Management_API.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public TransactionStatus Status { get; set; }

        // Customer data main fields
        public string CustomerFullName { get; set; } = string.Empty;
        public string CustomerMainPhone { get; set; } = string.Empty;
        public string CustomerMainEmail { get; set; } = string.Empty;
        public string CustomerMainAddress { get; set; } = string.Empty;
    }
}