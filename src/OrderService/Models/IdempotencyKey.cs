using System;

namespace OrderService.Models
{
    public class IdempotencyKey
    {
        public Guid Key { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}