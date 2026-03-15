namespace Finanacial_Transaction_Management_API.DTO
{
    public class TransactionSummaryDto
    {
        public int TotalTransactions { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal NetBalance { get; set; }

        public TransactionSummaryFilter? AppliedFilter { get; set; }
    }

    public class TransactionSummaryFilter
    {
        public string? CustomerFullName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
    }
}