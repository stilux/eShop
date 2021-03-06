﻿using System;

namespace DeliveryService.Models
{
    public class DeliveryRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Recipient { get; set; }
        public DateTime DeliveryDate { get; set; }
        
        public string TrackingNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Delivered { get; set; }
    }
}