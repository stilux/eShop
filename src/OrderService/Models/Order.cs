using System;
using System.Collections.Generic;

namespace OrderService.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        
        public int Id { get; set; }
        public byte OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int UserId { get; set; }
        public byte? PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool Paid => OrderStatusId == (byte) Enums.OrderStatus.Paid;
        public bool Cancelled => OrderStatusId == (byte) Enums.OrderStatus.Cancelled;
        public string DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? PlannedDeliveryDate { get; set; }

        public int? ReserveId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}