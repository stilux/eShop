using System.Collections.Generic;

namespace PaymentService.Models.Dtos
{
    public class CreateInvoiceDto
    {
        public int OrderId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}