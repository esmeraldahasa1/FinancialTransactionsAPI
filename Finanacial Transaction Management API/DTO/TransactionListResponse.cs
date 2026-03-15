namespace Finanacial_Transaction_Management_API.DTO
{
    public class TransactionListResponse
    {
        public PaginationDto Pagination { get; set; } = new();
        public List<TransactionDto> Data { get; set; } = new();
    }

    public class PaginationDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}