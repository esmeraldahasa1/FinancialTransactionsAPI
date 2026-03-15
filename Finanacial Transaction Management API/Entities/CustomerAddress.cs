using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.Entities
{
    public class CustomerAddress
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        public bool IsMain { get; set; } // Flag for main address

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public Customer Customer { get; set; } = null!;
    }
}