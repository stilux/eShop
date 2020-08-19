using System.Collections.Generic;
using System.Linq;
using PaymentService.Models;
using PaymentService.Models.Dtos;

namespace PaymentService.Extensions
{
    public static class CreateInvoiceDtoEx
    {
        public static Invoice MapToInvoice(this CreateInvoiceDto model)
        {
            return new Invoice
            {
                OrderId = model.OrderId,
                CartItems = model.CartItems != null 
                    ? model.CartItems
                        .Select(i => new CartItem
                        {
                            PositionId = i.PositionId,
                            Name = i.Name,
                            ItemCode = i.ItemCode,
                            ItemAmount = i.ItemAmount,
                            Quantity = i.Quantity
                        })
                        .ToList()
                    : new List<CartItem>()
            };
        }
    }
}