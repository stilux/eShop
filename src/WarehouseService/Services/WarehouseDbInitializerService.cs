using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WarehouseService.Infrastructure;
using WarehouseService.Models;

namespace WarehouseService.Services
{
    public class WarehouseDbInitializerService : IWarehouseDbInitializerService
    {
        private const int BatchSizeLimit = 1000;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WarehouseDbInitializerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async void GenerateWarehouseItems(int count)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<WarehouseContext>();
            
            var countInDb = await context.WarehouseItems.CountAsync();
            if (countInDb > 0) return;

            var processedItemsCount = 0;

            while (processedItemsCount < count)
            {
                var remainder = count - processedItemsCount;
                var currentBatchSize = remainder >= BatchSizeLimit ? BatchSizeLimit : remainder;

                var items = new List<WarehouseItem>();
                for (var i = 0; i < currentBatchSize; i++)
                {
                    items.Add(new WarehouseItem
                    {
                        ProductId = processedItemsCount + i + 1,
                        Total = 10,
                        ReservedQuantity = 0
                    });
                }
                
                await context.AddRangeAsync(items);
                var insertedItemsCount = await context.SaveChangesAsync();

                processedItemsCount += insertedItemsCount;
            }
        }
    }
}