namespace FNO.Domain.Models.Shipping
{
    public class Cart
    {
        public CartType CartType { get; set; }

        public LuaItemStack[] Inventory { get; set; }
    }
}
