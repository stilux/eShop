using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using OrderService.Services;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace OrderService.Courier.Activities
{
    public class DeliveryActivity : IActivity<IOrderMessage, IDeliveryLog>
    {
        private readonly IRequestClient<IDeliveryRequest> _deliveryRequestClient;
        private readonly IOrderService _orderService;
        private readonly ILogger<DeliveryActivity> _logger;
        
        public DeliveryActivity(IRequestClient<IDeliveryRequest> deliveryRequestClient, IOrderService orderService, 
            ILogger<DeliveryActivity> logger)
        {
            _deliveryRequestClient = deliveryRequestClient;
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IOrderMessage> context)
        {
            _logger.LogInformation($"Delivery called for order {context.Arguments.OrderId}");

            var order = await _orderService.GetOrderAsync(context.Arguments.OrderId);
            
            var response = await _deliveryRequestClient.GetResponse<IDeliveryRequestResult>(new
            {
                CorrelationId = context.Arguments.CorrelationId,
                OrderId = context.Arguments.OrderId,
                DeliveryAddress = order.DeliveryAddress ?? string.Empty,
                Recipient = order.Customer ?? string.Empty,
                DeliveryDate = order.DeliveryDate
            });
            
            if (!response.Message.Success)
                throw new InvalidOperationException();
            
            return context.Completed(new { context.Arguments.CorrelationId, context.Arguments.OrderId });
        }

        public Task<CompensationResult> Compensate(CompensateContext<IDeliveryLog> context)
        {
            throw new NotImplementedException();
        }
    }
    
    public interface IDeliveryLog
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}