using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.Entities
{
    public class CustomerEmail
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        public bool IsMain { get; set; } // Flag for main email address

        // Soft delete
        public bool IsDeleted { get; set; } = false;

        public Customer Customer { get; set; } = null!;
    }
}