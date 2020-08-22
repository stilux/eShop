using System;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Courier.Activities;
using OrderService.Enums;
using Shared.Contracts.Events;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;

namespace OrderService.Sagas.OrderProcessingSaga
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        private static readonly Uri ChangeOrderStatusMessageUri = QueueNames.GetMessageUri(nameof(IChangeOrderStatus)); 
        
        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            InstanceState(i => i.CurrentState);

            ConfigureCorrelationIds();
            
            Initially(
                When(OrderSubmitted)
                    .Then(i => i.Instance.OrderId = i.Data.OrderId)
                    .Then(i => logger.LogInformation($"Order {i.Instance.OrderId} submitted."))
                    .Activity(c => c.OfType<FulfillOrderActivity>())
                    .TransitionTo(Submitted)
            );
            
            During(Submitted,
                When(ProductsReserved)
                    .Then(i => logger.LogInformation($"Order {i.Instance.OrderId} reserved."))
                    .SendAsync(ChangeOrderStatusMessageUri, context => CreateChangeOrderStatusMessage(context, OrderStatus.ProductReserved))
                    .TransitionTo(Reserved)
            );
            
            During(Reserved,
                When(OrderPaid)
                    .Then(i => logger.LogInformation($"Order {i.Instance.OrderId} paid."))
                    .SendAsync(ChangeOrderStatusMessageUri, context => CreateChangeOrderStatusMessage(context, OrderStatus.Paid))
                    .TransitionTo(Paid)
            );
            
            DuringAny(
                When(OrderFulfillFaulted)
                    .Then(i => logger.LogInformation($"Order {i.Instance.OrderId} rejected."))
                    .TransitionTo(Rejected)
                    .Finalize()
            );

            During(Submitted,
                When(OrderFulfillCompleted)
                    .Then(i => logger.LogInformation($"Order {i.Instance.OrderId} completed."))
                    .SendAsync(ChangeOrderStatusMessageUri, context => CreateChangeOrderStatusMessage(context, OrderStatus.Shipped))
                    .TransitionTo(Completed)
                    .Finalize()
            );
        }

        private Task<IChangeOrderStatus> CreateChangeOrderStatusMessage(ConsumeEventContext<OrderState, IOrderEvent> context, OrderStatus status)
        {
            return context.Init<IChangeOrderStatus>(new
            {
                CorrelationId = context.Instance.CorrelationId, 
                OrderId = context.Instance.OrderId, 
                OrderStatusId = (byte) status
            });
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, i => i.CorrelateById(k => k.Message.CorrelationId)
                .SelectId(c => c.Message.CorrelationId));
            
            Event(() => ProductsReserved, i => i.CorrelateById(k => k.Message.CorrelationId));
            
            Event(() => OrderPaid, i => i.CorrelateById(k => k.Message.CorrelationId));
            
            Event(() => OrderFulfillFaulted, i => i.CorrelateById(k => k.Message.CorrelationId));
            
            Event(() => OrderFulfillCompleted, i => i.CorrelateById(k => k.Message.CorrelationId));
        }

        #region States

        public State Submitted { get; private set; }
        public State Reserved { get; private set; }
        public State Paid { get; private set; }
        public State Completed { get; private set; }
        public State Rejected { get; private set; }

        #endregion

        #region Events

        public Event<IOrderSubmittedEvent> OrderSubmitted { get; private set; }
        public Event<IProductsReservedEvent> ProductsReserved { get; private set; }
        public Event<IOrderPaidEvent> OrderPaid { get; private set; }
        public Event<IOrderFulfillFaultedEvent> OrderFulfillFaulted { get; private set; }
        public Event<IOrderFulfillCompletedEvent> OrderFulfillCompleted { get; private set; }

        #endregion
    }
}