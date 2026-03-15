using Finanacial_Transaction_Management_API.Enums;
using Financial_Transaction_Management_API.Enums;

namespace Financial_Transaction_Management_API.Models
{
    public class TransactionFilter
    {
        // Filterat bazuar ne Customer Data
        public string? CustomerFullName { get; set; }
        public string? CustomerMainEmail { get; set; }
        public string? CustomerMainPhone { get; set; }
        public string? CustomerMainAddress { get; set; }

        // Filtera shtese per transaksionet
        public TransactionType? TransactionType { get; set; }
        public TransactionStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Per soft dhe hard delete
        public bool IncludeDeleted { get; set; } = false;
    }
}