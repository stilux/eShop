﻿using System.Collections.Generic;
using System.Linq;
using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Extensions
{
    public static class OrderItemCollectionEx
    {
        public static IList<OrderItemDto> MapToOrderItemDtoCollection(this IEnumerable<OrderItem> orderItem)
        {
            return orderItem
                .Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToList();
        }
    }
}