using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Activities
{
    public class DeliveryActivity : IExecuteActivity<IDeliveryRequest>
    {
        private readonly ILogger<DeliveryActivity> _logger;
        private static readonly Uri DeliveryRequestMessageUri = QueueNames.GetMessageUri(nameof(IDeliveryRequest));

        public DeliveryActivity(ILogger<DeliveryActivity> logger)
        {
            _logger = logger;
        }
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IDeliveryRequest> context)
        {
            _logger.LogInformation($"Delivery called for order {context.Arguments.OrderId}");
            
            var sendEndpoint = await context.GetSendEndpoint(DeliveryRequestMessageUri);
            await sendEndpoint.Send<IDeliveryRequest>(context);
            return context.Completed(new { context.Arguments.OrderId });
        }
    }
}