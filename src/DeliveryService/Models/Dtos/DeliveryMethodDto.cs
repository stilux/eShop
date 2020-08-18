using System;

namespace DeliveryService.Models.Dtos
{
    public class DeliveryMethodDto
    {
        public string DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
        public float DeliveryCost { get; set; }
    }
}