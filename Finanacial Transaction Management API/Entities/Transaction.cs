namespace Finanacial_Transaction_Management_API.Entities
{
    public class Transaction // Aktori kryesor 1
    {
        public int TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
    }
}
