using System.Collections.Generic;

namespace OrderService.Models.Dtos
{
    public class AddToCartDto
    {
        public IEnumerable<CartItemDto> Items { get; set; }
    }
}