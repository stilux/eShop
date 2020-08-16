namespace WarehouseService.Entity
{
    public class ReserveItem
    {
        public int Id { get; set; }
        public int ReserveId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
        public Reserve Reserve { get; set; }
    }
}