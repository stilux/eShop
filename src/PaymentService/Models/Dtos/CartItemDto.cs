namespace PaymentService.Models.Dtos
{
    public class CartItemDto
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ItemCode { get; set; }
        public float ItemAmount { get; set; }
    }
}