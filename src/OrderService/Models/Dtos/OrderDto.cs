using System;
using System.Collections.Generic;

namespace OrderService.Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public bool Paid { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } 
    }
}