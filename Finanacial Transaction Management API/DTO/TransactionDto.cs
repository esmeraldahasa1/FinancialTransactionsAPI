namespace Finanacial_Transaction_Management_API.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
