using DeliveryService.Models;
using DeliveryService.Models.Dtos;

namespace DeliveryService.Extensions
{
    public static class DeliveryRequestEx
    {
        public static DeliveryRequestDto MapToDeliveryRequestDto(this DeliveryRequest request)
        {
            return new DeliveryRequestDto
            {
                Id = request.Id,
                Recipient = request.Recipient,
                CreationDate = request.CreationDate,
                DeliveryAddress = request.DeliveryAddress,
                OrderId = request.OrderId,
                DeliveryDate = request.DeliveryDate
            };
        }
    }
}