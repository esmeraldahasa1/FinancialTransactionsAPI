using Finanacial_Transaction_Management_API.Enums;
using Financial_Transaction_Management_API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.DTO
{
    public class CreateTransactionDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shuma duhet te jete me e madhe se zero.")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerFullName { get; set; } = string.Empty;

        // Te dhenat kryesore te klientit 
        [Required]
        [Phone]
        public string CustomerMainPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerMainEmail { get; set; } = string.Empty;

        [Required]
        public string CustomerMainAddress { get; set; } = string.Empty;
    }
}