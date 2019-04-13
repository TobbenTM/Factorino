namespace FNO.Domain.Models.Shipping
{
    public enum ShipmentState
    {
        Unknown = 0,
        Requested = 1,
        Fulfilled = 2,
        Received = 3,
        Completed = 4,
    }
}
