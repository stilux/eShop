using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Enums;
using OrderService.Services;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Consumers
{
    public class ChangeOrderStatusConsumer : IConsumer<IChangeOrderStatus>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<ChangeOrderStatusConsumer> _logger;

        public ChangeOrderStatusConsumer(IOrderService orderService, ILogger<ChangeOrderStatusConsumer> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IChangeOrderStatus> context)
        {
            await _orderService.ChangeOrderStatusAsync(context.Message.OrderId, (OrderStatus) context.Message.OrderStatusId);
        }
    }
}