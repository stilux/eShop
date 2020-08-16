using System;

namespace DeliveryService.Models
{
    public class DeliveryMethodModel
    {
        public string DeliveryMethod { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        
        public float DeliveryCost { get; set; }
    }
}