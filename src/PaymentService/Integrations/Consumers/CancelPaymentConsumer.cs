using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PaymentService.Services;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace PaymentService.Integrations.Consumers
{
    public class CancelPaymentConsumer : IConsumer<ICancelPayment>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<CancelPaymentConsumer> _logger;

        public CancelPaymentConsumer(IPaymentService paymentService, ILogger<CancelPaymentConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ICancelPayment> context)
        {
            _logger.LogInformation($"Cancel Payment called for order {context.Message.OrderId}");
            
            try
            {
                await _paymentService.CancelPaymentAsync(context.Message.OrderId);
                await context.RespondAsync<ICancelPaymentResult>(new { Success = true });
            }
            catch (Exception)
            {
                await context.RespondAsync<ICancelPaymentResult>(new { Success = false });
            }
        }
    }
}