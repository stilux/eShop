using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using OrderService.Enums;
using OrderService.Services;
using Shared.Contracts.Messages;

namespace OrderService.Courier.Activities
{
    public class SubmitOrderActivity: IActivity<IOrderMessage, ISubmitOrderLog>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<SubmitOrderActivity> _logger;

        public SubmitOrderActivity(IOrderService orderService, ILogger<SubmitOrderActivity> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IOrderMessage> context)
        {
            _logger.LogInformation($"Submit Order called for order {context.Arguments.OrderId}");
            
            await _orderService.ChangeOrderStatusAsync(context.Arguments.OrderId, OrderStatus.Submitted);
            return context.Completed(new { context.Arguments.OrderId });
        }
        
        public async Task<CompensationResult> Compensate(CompensateContext<ISubmitOrderLog> context)
        {
            _logger.LogInformation($"Submit Order compensated called for order {context.Log.OrderId}");
            
            await _orderService.ChangeOrderStatusAsync(context.Log.OrderId, OrderStatus.New);
            return context.Compensated();
        }
    }

    public interface ISubmitOrderLog
    {
        int OrderId { get; }
    }
}