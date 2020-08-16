using System;
using DeliveryService.Entity;
using DeliveryService.Models;

namespace DeliveryService.Extensions
{
    public static class CreateDeliveryRequestModelEx
    {
        public static DeliveryRequest MapToDeliveryRequest(this CreateDeliveryRequestModel model)
        {
            return new DeliveryRequest
            {
                OrderId = model.OrderId,
                CreationDate = DateTime.Now,
                Recipient = model.Recipient,
                DeliveryAddress = model.DeliveryAddress,
                PlannedDeliveryDate = model.PlannedDeliveryDate
            };
        }
    }
}