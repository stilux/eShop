using System;
using System.Collections.Generic;
using WarehouseService.Models;

namespace WarehouseService.Entity
{
    public class Reserve
    {
        public Reserve()
        {
            ReserveItems = new List<ReserveItem>();
        }
        
        public int Id { get; set; }
        public List<ReserveItem> ReserveItems { get; set; }
        public DateTime CreationDate { get; set; }
    }
}