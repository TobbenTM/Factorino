namespace FNO.Domain.Models.Market
{
    public enum OrderState
    {
        Created = 0,
        Active = 1,
        PartiallyFulfilled = 2,
        Fulfilled = 3,
        Cancelled = 4,
    }
}
