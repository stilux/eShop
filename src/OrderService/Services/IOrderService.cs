using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderAsync(int orderId);
        Task<OrderDto> CreateOrderAsync();
        Task CancelOrderAsync(int orderId);
        Task<string> PayOrderAsync(int orderId);
        Task<IEnumerable<OrderItemDto>> AddToCartAsync(int orderId, AddToCartDto model);
        Task RemoveFromCartAsync(int orderId, RemoveFromCartDto model);
        Task ChangeOrderItemQuantityAsync(int orderId, int productId, ChangeOrderItemQuantityDto model);
    }
}