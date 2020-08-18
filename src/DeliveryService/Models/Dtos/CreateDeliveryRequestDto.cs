using System;

namespace DeliveryService.Models.Dtos
{
    public class CreateDeliveryRequestDto
    {
        public int OrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Recipient { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}