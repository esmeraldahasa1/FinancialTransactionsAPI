namespace Finanacial_Transaction_Management_API.DTO
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int CustomerId { get; set; }
    }
}
