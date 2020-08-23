using Shared.Contracts.Messages;

namespace OrderService.Models.Dtos
{
    public class CartItemDto : ICartItem
    {
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}