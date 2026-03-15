namespace Finanacial_Transaction_Management_API.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string MainPhone { get; set; } = string.Empty;
        public string MainEmail { get; set; } = string.Empty;
        public string MainAddress { get; set; } = string.Empty;
    }
}