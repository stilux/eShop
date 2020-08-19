namespace PaymentService.Models
{
    public class CartItem
    {
        public int PositionId { get; set; }
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ItemCode { get; set; }
        public float ItemAmount { get; set; }
    }
}