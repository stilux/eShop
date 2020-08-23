using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;
using WarehouseService.Services;

namespace WarehouseService.Integrations.Consumers
{
    public class CancelReservationConsumer : IConsumer<ICancelReservation>
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<CancelReservationConsumer> _logger;

        public CancelReservationConsumer(IWarehouseService warehouseService, ILogger<CancelReservationConsumer> logger)
        {
            _warehouseService = warehouseService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ICancelReservation> context)
        {
            _logger.LogInformation($"Cancel Reservation called for order {context.Message.OrderId}");

            try
            {
                await _warehouseService.CancelReservationAsync(context.Message.ReserveId);
                await context.RespondAsync<ICancelReservationResult>(new { Success = true });
            }
            catch (Exception)
            {
                await context.RespondAsync<ICancelReservationResult>(new { Success = false });
            }
        }
    }
}