using System;
using DeliveryService.Models;
using DeliveryService.Models.Dtos;

namespace DeliveryService.Extensions
{
    public static class CreateDeliveryRequestDtoEx
    {
        public static DeliveryRequest MapToDeliveryRequest(this CreateDeliveryRequestDto model)
        {
            return new DeliveryRequest
            {
                OrderId = model.OrderId,
                CreationDate = DateTime.Now,
                Recipient = model.Recipient,
                DeliveryAddress = model.DeliveryAddress,
                DeliveryDate = model.DeliveryDate
            };
        }
    }
}