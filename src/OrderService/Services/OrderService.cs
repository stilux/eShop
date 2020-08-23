using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using OrderService.Exceptions;
using OrderService.Extensions;
using OrderService.Models;
using OrderService.Models.Dtos;
using OrderService.Providers;
using OrderStatus = OrderService.Enums.OrderStatus;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext _context;

        public OrderService(OrderContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(i => i.Id == orderId);

            return order?.MapToOrderDto();
        }

        public async Task<OrderDto> CreateOrderAsync()
        {
            var currentDate = DateTime.Now;
            var order = new Order
            {
                OrderStatusId = (byte)OrderStatus.New,
                CreationDate = currentDate,
                UpdateDate = currentDate
            };
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.MapToOrderDto();
        }

        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new OrderNotFoundException();
            
            if (order.Cancelled)
                throw new OrderAlreadyCancelledException();

            order.OrderStatusId = (byte) status;
            order.UpdateDate = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }

        public async Task<string> PayOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new OrderNotFoundException();
            
            if (order.Cancelled)
                throw new OrderAlreadyCancelledException();
            
            if (order.Paid)
                throw new OrderAlreadyPaidException();

            var faker = new Faker();
            var paymentUrl = faker.Internet.Url();
            return await Task.FromResult(paymentUrl);
        }

        public async Task<IList<OrderItemDto>> GetCartItemsAsync(int orderId)
        {
            var orderItems = await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .ToListAsync();

            return orderItems.MapToOrderItemDtoCollection();
        }

        public async Task<IList<OrderItemDto>> AddToCartAsync(int orderId, AddToCartDto model)
        {
            var addedItems = model.Items
                .Select(i => new OrderItem
                {
                    OrderId = orderId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToList();
            
            await _context.AddRangeAsync(addedItems);
            await _context.SaveChangesAsync();
            
            return addedItems.MapToOrderItemDtoCollection();
        }

        public async Task RemoveFromCartAsync(int orderId, RemoveFromCartDto model)
        {
            var orderItems = await GetOrderItems(orderId);

            if (orderItems.Count > 0)
            {
                var removedItems = orderItems
                    .Join(model.Ids, i => i.ProductId, id => id, (i, im) => i)
                    .ToList();

                if (removedItems.Count > 0)
                {
                    _context.RemoveRange(removedItems);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ChangeOrderItemQuantityAsync(int orderId, int productId, ChangeOrderItemQuantityDto model)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(i => i.OrderId == orderId && i.ProductId == productId);
            
            if (orderItem == null)
                throw new OrderItemNotFoundException();

            orderItem.Quantity = model.Quantity;
            await _context.SaveChangesAsync();
        }

        private async Task<List<OrderItem>> GetOrderItems(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .ToListAsync();
        }
    }
}