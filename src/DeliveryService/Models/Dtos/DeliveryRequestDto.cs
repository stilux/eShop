using System;

namespace DeliveryService.Models.Dtos
{
    public class DeliveryRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Recipient { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}