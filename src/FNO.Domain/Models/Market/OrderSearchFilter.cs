namespace FNO.Domain.Models.Market
{
    public class OrderSearchFilter
    {
        // Filter criteria
        public OrderType OrderType { get; set; }
        public string ItemId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        // Sorting flags
        public OrderSearchSortColumn SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
