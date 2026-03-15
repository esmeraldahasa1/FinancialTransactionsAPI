using Finanacial_Transaction_Management_API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.DTO
{
    public class CreateTransactionDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        public int CustomerId { get; set; } 
    }
}