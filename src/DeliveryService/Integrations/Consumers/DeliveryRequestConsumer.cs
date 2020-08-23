using System;
using System.Threading.Tasks;
using DeliveryService.Models.Dtos;
using DeliveryService.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace DeliveryService.Integrations.Consumers
{
    public class DeliveryRequestConsumer : IConsumer<IDeliveryRequest>
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveryRequestConsumer> _logger;

        public DeliveryRequestConsumer(IDeliveryService deliveryService, ILogger<DeliveryRequestConsumer> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IDeliveryRequest> context)
        {
            _logger.LogInformation($"Delivery Request called for order {context.Message.OrderId}");

            try
            {
                await _deliveryService.CreateDeliveryRequestAsync(new CreateDeliveryRequestDto
                {
                    OrderId = context.Message.OrderId,
                    DeliveryAddress = context.Message.DeliveryAddress,
                    DeliveryDate = context.Message.DeliveryDate,
                    Recipient = context.Message.Recipient
                });
                
                await context.RespondAsync<IDeliveryRequestResult>(new { Success = true });
            }
            catch (Exception)
            {
                await context.RespondAsync<IDeliveryRequestResult>(new { Success = false });
            }
        }
    }
}