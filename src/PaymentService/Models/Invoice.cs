using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string ExternalOrderId { get; set; }
        public int OrderId { get; set; }
        public string PaymentFormUrl { get; set; }
        public bool Paid { get; set; }
        public DateTime CreationDate { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}