namespace FNO.Domain.Models
{
    public class LuaItemStack
    {
        public string Name { get; set; }

        public long Count { get; set; }

        /// <summary>
        /// Enrichment prop for item metadata
        /// </summary>
        public FactorioEntity Item { get; set; }
    }
}
