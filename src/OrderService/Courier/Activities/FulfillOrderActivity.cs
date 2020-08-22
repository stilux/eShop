using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.Logging;
using OrderService.Sagas.OrderProcessingSaga;
using Shared.Contracts.Events;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Activities
{
    public class FulfillOrderActivity : Activity<OrderState, IOrderSubmittedEvent>
    {
        private readonly ILogger<FulfillOrderActivity> _logger;

        public FulfillOrderActivity(ILogger<FulfillOrderActivity> logger)
        {
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, IOrderSubmittedEvent> context,
            Behavior<OrderState, IOrderSubmittedEvent> next)
        {
            var sendEndpoint = await context.GetSendEndpoint(QueueNames.GetMessageUri(nameof(IFulfillOrder)));
            _logger.LogInformation($"Order Transaction activity for sendEndpoint {sendEndpoint} will be called");
            await sendEndpoint.Send<IFulfillOrder>(new
            {
                CorrelationId = context.Data.CorrelationId,
                OrderId = context.Data.OrderId,
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, IOrderSubmittedEvent, TException> context,
            Behavior<OrderState, IOrderSubmittedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submit-order");
        }
    }
}