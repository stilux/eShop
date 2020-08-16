using System;

namespace DeliveryService.Models
{
    public class CreateDeliveryRequestModel
    {
        public int OrderId { get; set; }
        
        public string DeliveryAddress { get; set; }
        
        public string Recipient { get; set; }
        
        public DateTime PlannedDeliveryDate { get; set; }
    }
}