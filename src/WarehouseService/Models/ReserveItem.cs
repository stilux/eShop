namespace WarehouseService.Models
{
    public class ReserveItem
    {
        public int ReserveId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}