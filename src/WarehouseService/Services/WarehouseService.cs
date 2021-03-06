﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseService.Entity;
using WarehouseService.Exceptions;
using WarehouseService.Extensions;
using WarehouseService.Infrastructure;
using WarehouseService.Models;
using WarehouseService.Models.Dtos;

namespace WarehouseService.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly WarehouseContext _context;

        public WarehouseService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<int?> GetProductBalanceAsync(int productId)
        {
            var item = await _context.WarehouseItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ProductId == productId);
            
            return item?.Balance;
        }

        public async Task<ReservationResultDto> ReserveAsync(ReserveDto model)
        {
            var productIds = model.Items
                .Select(i => i.Id)
                .ToList();
            
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var warehouseItems = await GetWarehouseItems(productIds);

                    var reserve = new Reserve
                    {
                        CreationDate = DateTime.Now
                    };

                    foreach (var item in model.Items)
                    {
                        var warehouseItem = warehouseItems
                            .FirstOrDefault(i => i.ProductId == item.Id);

                        if (warehouseItem == null)
                            throw new ProductNotFoundException(item.Id);

                        if (warehouseItem.Balance < item.Quantity)
                            throw new InsufficientProductQuantityException(item.Id, warehouseItem.Balance);

                        reserve.ReserveItems.Add(new ReserveItem
                        {
                            ProductId = item.Id,
                            Quantity = item.Quantity
                        });
                    }

                    await _context.AddAsync(reserve);
                
                    IncreaseProductReservedQuantity(warehouseItems, reserve.ReserveItems);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return reserve.MapToReservationResultDto();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task CancelReservationAsync(int reserveId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var reserve = await _context.Reserves.FindAsync(reserveId);
                    if (reserve == null)
                        throw new ReserveNotFoundException();

                    await DecreaseProductReservedQuantity(reserveId);
                
                    _context.Reserves.Remove(reserve);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        private async Task DecreaseProductReservedQuantity(int reserveId)
        {
            var reserveItems = await _context.ReserveItems
                .Where(i => i.ReserveId == reserveId)
                .ToListAsync();

            var productIds = reserveItems
                .Select(i => i.ProductId)
                .ToList();

            var warehouseItems = await GetWarehouseItems(productIds);
            
            foreach (var warehouseItem in warehouseItems)
            {
                var reserveItem =
                    reserveItems.FirstOrDefault(i => i.ProductId == warehouseItem.ProductId);

                if (reserveItem == null) continue;
                
                if (warehouseItem.ReservedQuantity <= reserveItem.Quantity)
                    warehouseItem.ReservedQuantity = 0;
                else
                    warehouseItem.ReservedQuantity -= reserveItem.Quantity;
            }
        }

        private static void IncreaseProductReservedQuantity(IEnumerable<WarehouseItem> warehouseItems, ICollection<ReserveItem> reserveItems)
        {
            foreach (var warehouseItem in warehouseItems)
            {
                var reserveItem =
                    reserveItems.FirstOrDefault(i => i.ProductId == warehouseItem.ProductId);

                if (reserveItem != null)
                    warehouseItem.ReservedQuantity += reserveItem.Quantity;
            }
        }

        private async Task<IList<WarehouseItem>> GetWarehouseItems(ICollection<int> productIds)
        {
            return await _context.WarehouseItems
                .Where(i => productIds.Contains(i.ProductId))
                .ToListAsync();
        }
    }
}