namespace Financial_Transaction_Management_API.Models
{
    public class Pagination
    {
        private const int MaxPageSize = 100; //maksimumi 100 rreshta per faqe
        private const int DefaultPageSize = 50; //Default 50 rreshta për faqe

        public int PageNumber { get; set; } = 1; //Numri i faqes që po kërkohet (fillon nga 1)

        private int _pageSize = DefaultPageSize;

        public int PageSize  //Madhesia e faqes - sa rreshta te kthehen per faqe Nese vlera eshte > MaxPageSize kufizohet ne MaxPageSize
                             // Nese vlera eshte < 1, kufizohet ne 1
        {
            get => _pageSize; set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        public int Skip => (PageNumber - 1) * PageSize;
        public int Take => PageSize;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}