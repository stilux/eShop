using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Activities
{
    public class PaymentActivity : IActivity<IOrderPayment, IPaymentLog>
    {
        private readonly ILogger<PaymentActivity> _logger;
        private static readonly Uri OrderPaymentMessageUri = QueueNames.GetMessageUri(nameof(IOrderPayment));
        private static readonly Uri CancelPaymentMessageUri = QueueNames.GetMessageUri(nameof(ICancelPayment));

        public PaymentActivity(ILogger<PaymentActivity> logger)
        {
            _logger = logger;
        }
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IOrderPayment> context)
        {
            _logger.LogInformation($"Payment called for order {context.Arguments.OrderId}");
            
            var sendEndpoint = await context.GetSendEndpoint(OrderPaymentMessageUri);
            await sendEndpoint.Send<IOrderPayment>(new
            { 
                CorrelationId = context.Arguments.CorrelationId,
                OrderId = context.Arguments.OrderId,
                Items = context.Arguments.Items
            });
            return context.Completed(new { context.Arguments.CorrelationId, context.Arguments.OrderId });
        }
        
        public async Task<CompensationResult> Compensate(CompensateContext<IPaymentLog> context)
        {
            _logger.LogInformation($"Payment compensated called for order {context.Log.OrderId}");
            
            var sendEndpoint = await context.GetSendEndpoint(CancelPaymentMessageUri);
            await sendEndpoint.Send<ICancelPayment>(new
            {
                CorrelationId = context.Log.CorrelationId,
                OrderId = context.Log.OrderId               
            });
            return context.Compensated();
        }
    }

    public interface IPaymentLog
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}