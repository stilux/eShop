﻿using System;
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
    public class ReserveProductsActivity : IActivity<IReserveProducts, IReserveProductsLog>
    {
        private readonly IRequestClient<IReserveProducts> _reserveProductsClient;
        private readonly IRequestClient<ICancelReservation> _cancelReservationClient;
        private readonly IOrderService _orderService;
        private readonly ILogger<ReserveProductsActivity> _logger;

        public ReserveProductsActivity(IRequestClient<IReserveProducts> reserveProductsClient, 
            IRequestClient<ICancelReservation> cancelReservationClient,
            IOrderService orderService, ILogger<ReserveProductsActivity> logger)
        {
            _reserveProductsClient = reserveProductsClient;
            _cancelReservationClient = cancelReservationClient;
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IReserveProducts> context)
        {
            _logger.LogInformation($"Reserve Products called for order {context.Arguments.OrderId}");

            var orderItems = await _orderService.GetCartItemsAsync(context.Arguments.OrderId);
            IList<ICartItem> items = orderItems
                .Select(i => (ICartItem) new CartItemDto { ProductId = i.ProductId, Quantity = i.Quantity })
                .ToList();

            var response = await _reserveProductsClient.GetResponse<IReserveProductsResult>(new
            {
                CorrelationId = context.Arguments.CorrelationId,
                OrderId = context.Arguments.OrderId,
                Items = items
            });
            
            if (!response.Message.Success)
                throw new InvalidOperationException();
            
            return context.Completed(new {context.Arguments.CorrelationId, context.Arguments.OrderId, response.Message.ReserveId});
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IReserveProductsLog> context)
        {
            _logger.LogInformation($"Compensate Reserve Products called for order {context.Log.OrderId}");

            var response = await _cancelReservationClient.GetResponse<ICancelReservationResult>(new
            {
                CorrelationId = context.Log.CorrelationId,
                OrderId = context.Log.OrderId,
                ReserveId = context.Log.ReserveId
            });

            if (!response.Message.Success)
                throw new InvalidOperationException();
            
            return context.Compensated();
        }
    }

    public interface IReserveProductsLog
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
        int ReserveId { get; }
    }
}