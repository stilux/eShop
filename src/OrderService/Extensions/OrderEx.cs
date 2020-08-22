using System.Linq;
using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Extensions
{
    public static class OrderEx
    {
        public static OrderDto MapToOrderDto(this Order order)
        {
            var orderItems = order.OrderItems
                .Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToList();
            
            return new OrderDto
            {
                Id = order.Id,
                OrderStatus = ((Enums.OrderStatus)order.OrderStatusId).ToString(),
                PaymentMethod = order.PaymentMethodId.HasValue ? ((Enums.PaymentMethod)order.PaymentMethodId).ToString() : null,
                Paid = order.Paid,
                DeliveryMethod = order.DeliveryMethod,
                DeliveryAddress = order.DeliveryAddress,
                TotalPrice = order.TotalPrice,
                DeliveryDate = order.PlannedDeliveryDate,
                CreationDate = order.CreationDate,
                UpdateDate = order.UpdateDate,
                OrderItems = orderItems
            };
        }
    }
}