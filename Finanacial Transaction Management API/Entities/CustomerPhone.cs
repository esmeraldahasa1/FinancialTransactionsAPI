using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.Entities
{
    public class CustomerPhone
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsMain { get; set; } // Flag for main phone number

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public Customer Customer { get; set; } = null!;
    }
}