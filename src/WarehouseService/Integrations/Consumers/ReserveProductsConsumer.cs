using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;
using WarehouseService.Models.Dtos;
using WarehouseService.Services;

namespace WarehouseService.Integrations.Consumers
{
    public class ReserveProductsConsumer : IConsumer<IReserveProducts>
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<ReserveProductsConsumer> _logger;

        public ReserveProductsConsumer(IWarehouseService warehouseService, ILogger<ReserveProductsConsumer> logger)
        {
            _warehouseService = warehouseService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IReserveProducts> context)
        {
            _logger.LogInformation($"Reserve Products called for order {context.Message.OrderId}");

            try
            {
                var items = context.Message.Items
                    .Select(i => new ReserveItemDto {Id = i.ProductId, Quantity = i.Quantity});
                
                var result = await _warehouseService.ReserveAsync(new ReserveDto { Items = items });
                await context.RespondAsync<IReserveProductsResult>(new { result.ReserveId, Success = true });
            }
            catch (Exception)
            {
                await context.RespondAsync<IReserveProductsResult>(new { Success = false });
            }
        }
    }
}