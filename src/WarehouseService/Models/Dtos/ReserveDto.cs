using System.Collections.Generic;

namespace WarehouseService.Models.Dtos
{
    public class ReserveDto
    {
        public IEnumerable<ReserveItemDto> Items { get; set; }
    }
}