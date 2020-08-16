namespace WarehouseService.Entity
{
    public class WarehouseItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Total { get; set; }
        public int ReservedQuantity { get; set; }
        public int Balance => Total - ReservedQuantity;
    }
}