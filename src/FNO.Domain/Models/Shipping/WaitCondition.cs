namespace FNO.Domain.Models.Shipping
{
    public class WaitCondition
    {
        public WaitConditionType Type { get; set; }

        public WaitConditionCompareType CompareType { get; set; }

        /// <summary>
        /// Used when using the "time" type
        /// </summary>
        public int Ticks { get; set; }
    }
}
