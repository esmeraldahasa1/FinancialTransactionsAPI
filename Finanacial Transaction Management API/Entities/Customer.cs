using System.ComponentModel.DataAnnotations;

namespace Finanacial_Transaction_Management_API.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public ICollection<CustomerPhone> Phones { get; set; } = new List<CustomerPhone>();
        public ICollection<CustomerEmail> Emails { get; set; } = new List<CustomerEmail>();
        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // Helper methods per te mare informarcionin kryesor te kontaktit
        public string? GetMainPhone() => Phones?.FirstOrDefault(p => p.IsMain && !p.IsDeleted)?.PhoneNumber;
        public string? GetMainEmail() => Emails?.FirstOrDefault(e => e.IsMain && !e.IsDeleted)?.Email;
        public string? GetMainAddress() => Addresses?.FirstOrDefault(a => a.IsMain && !a.IsDeleted)?.Address;
    }
}