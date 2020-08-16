using System;

namespace DeliveryService.Models
{
    public class DeliveryRequestModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Recipient { get; set; }
        public DateTime PlannedDeliveryDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}