using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PaymentService.Models.Dtos;
using PaymentService.Services;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace PaymentService.Integrations.Consumers
{
    public class OrderPaymentConsumer : IConsumer<IOrderPayment>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<OrderPaymentConsumer> _logger;

        public OrderPaymentConsumer(IPaymentService paymentService, ILogger<OrderPaymentConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IOrderPayment> context)
        {
            _logger.LogInformation($"Payment called for order {context.Message.OrderId}");
            
            var items = context.Message.Items
                .Select(i => new CartItemDto { ItemCode = i.ProductId, Quantity = i.Quantity})
                .ToList();

            try
            {
                var paymentFormUrl = await _paymentService.CreateInvoiceAsync(new CreateInvoiceDto
                {
                    OrderId = context.Message.OrderId,
                    CartItems = items
                });
                await context.RespondAsync<IOrderPaymentResult>(new { PaymentFormUrl = paymentFormUrl, Success = true });
            }
            catch (Exception)
            {
                await context.RespondAsync<IOrderPaymentResult>(new { Success = false });
            }
        }
    }
}