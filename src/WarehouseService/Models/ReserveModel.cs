using System.Collections.Generic;

namespace WarehouseService.Models
{
    public class ReserveModel
    {
        public IEnumerable<ReserveItemModel> Items { get; set; }
    }
}