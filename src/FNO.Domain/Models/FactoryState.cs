namespace FNO.Domain.Models
{
    public enum FactoryState
    {
        Unknown = 0,
        Creating = 1,
        Starting = 2,
        Online = 3,
        Destroying = 4,
        Destroyed = 5,
    }
}
