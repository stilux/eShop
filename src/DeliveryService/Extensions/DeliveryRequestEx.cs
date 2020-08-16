using DeliveryService.Entity;
using DeliveryService.Models;

namespace DeliveryService.Extensions
{
    public static class DeliveryRequestEx
    {
        public static DeliveryRequestModel MapToDeliveryRequestModel(this DeliveryRequest request)
        {
            return new DeliveryRequestModel
            {
                Id = request.Id,
                Recipient = request.Recipient,
                CreationDate = request.CreationDate,
                DeliveryAddress = request.DeliveryAddress,
                OrderId = request.OrderId,
                PlannedDeliveryDate = request.PlannedDeliveryDate
            };
        }
    }
}