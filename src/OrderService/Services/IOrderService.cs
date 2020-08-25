using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Models.Dtos;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderAsync(int orderId);
        Task<OrderDto> CreateOrderAsync(Guid idempotencyKey);
        Task ChangeOrderStatusAsync(int orderId, Enums.OrderStatus status);
        Task<string> PayOrderAsync(int orderId);
        Task<IList<OrderItemDto>> GetCartItemsAsync(int orderId);
        Task<IList<OrderItemDto>> AddToCartAsync(int orderId, AddToCartDto model);
        Task RemoveFromCartAsync(int orderId, RemoveFromCartDto model);
        Task ChangeOrderItemQuantityAsync(int orderId, int productId, ChangeOrderItemQuantityDto model);
    }
}