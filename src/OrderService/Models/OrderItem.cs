namespace OrderService.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}