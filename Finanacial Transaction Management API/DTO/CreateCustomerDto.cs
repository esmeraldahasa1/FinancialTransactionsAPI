using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.DTO
{
    public class CreateCustomerDto
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string MainPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string MainEmail { get; set; } = string.Empty;

        [Required]
        public string MainAddress { get; set; } = string.Empty;
    }
}