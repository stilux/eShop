using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using OrderService.Courier.Activities;
using OrderService.Services;
using Shared.Contracts.Events;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Consumers
{
    public class FulfillOrderConsumer : IConsumer<IFulfillOrder>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<FulfillOrderConsumer> _logger;

        public FulfillOrderConsumer(IOrderService orderService, ILogger<FulfillOrderConsumer> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IFulfillOrder> context)
        {
            _logger.LogInformation($"Fulfilled order {context.Message.OrderId}");
            
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            var order = await _orderService.GetOrderAsync(context.Message.OrderId);
            
            builder.AddActivity("SubmitOrder", QueueNames.GetActivityUri(nameof(SubmitOrderActivity)));
            
            builder.AddActivity("ReserveProducts", QueueNames.GetActivityUri(nameof(ReserveProductsActivity)), new
            {
                order.OrderItems
            });
            
            builder.AddActivity("Payment", QueueNames.GetActivityUri(nameof(PaymentActivity)));

            builder.AddActivity("Delivery", QueueNames.GetActivityUri(nameof(DeliveryActivity)), new
            {
                order.Id,
                order.DeliveryAddress,
                order.Customer,
                order.DeliveryDate
            });

            builder.AddVariable("CorrelationId", context.Message.CorrelationId);
            builder.AddVariable("OrderId", context.Message.OrderId);
            
            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<IOrderFulfillFaultedEvent>(new { context.Message.OrderId }));

            await builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.None, x => x.Send<IOrderFulfillCompletedEvent>(new { context.Message.OrderId }));

            var routingSlip = builder.Build();
            await context.Execute(routingSlip).ConfigureAwait(false);
        }
    }
}