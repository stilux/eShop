using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using OrderService.Models.Dtos;
using OrderService.Services;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace OrderService.Courier.Activities
{
    public class PaymentActivity : IActivity<IOrderPayment, IPaymentLog>
    {
        private readonly IRequestClient<IOrderPayment> _orderPaymentClient;
        private readonly IRequestClient<ICancelPayment> _cancelPaymentClient;
        private readonly IOrderService _orderService;
        private readonly ILogger<PaymentActivity> _logger;
        
        public PaymentActivity(IRequestClient<IOrderPayment> orderPaymentClient, 
            IRequestClient<ICancelPayment> cancelPaymentClient,
            IOrderService orderService, ILogger<PaymentActivity> logger)
        {
            _orderPaymentClient = orderPaymentClient;
            _cancelPaymentClient = cancelPaymentClient;
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IOrderPayment> context)
        {
            _logger.LogInformation($"Payment called for order {context.Arguments.OrderId}");
            
            var orderItems = await _orderService.GetCartItemsAsync(context.Arguments.OrderId);
            IList<ICartItem> items = orderItems
                .Select(i => (ICartItem) new CartItemDto {  ProductId = i.ProductId, Quantity = i.Quantity })
                .ToList();
            
            var response = await _orderPaymentClient.GetResponse<IOrderPaymentResult>(new
            {
                CorrelationId = context.Arguments.CorrelationId,
                OrderId = context.Arguments.OrderId,
                Items = items
            });
            
            if (!response.Message.Success)
                throw new InvalidOperationException();
            
            return context.Completed(new { context.Arguments.CorrelationId, context.Arguments.OrderId });
        }
        
        public async Task<CompensationResult> Compensate(CompensateContext<IPaymentLog> context)
        {
            _logger.LogInformation($"Payment compensated called for order {context.Log.OrderId}");
            
            var response = await _cancelPaymentClient.GetResponse<ICancelPaymentResult>(new
            {
                CorrelationId = context.Log.CorrelationId,
                OrderId = context.Log.OrderId
            });
            
            if (!response.Message.Success)
                throw new InvalidOperationException();
            
            return context.Compensated();
        }
    }

    public interface IPaymentLog
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}