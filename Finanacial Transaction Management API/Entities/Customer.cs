namespace Finanacial_Transaction_Management_API.Entities
{
    public class Customer // Aktori kryesor 2
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
