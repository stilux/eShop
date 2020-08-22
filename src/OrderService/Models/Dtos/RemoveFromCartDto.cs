using System.Collections.Generic;

namespace OrderService.Models.Dtos
{
    public class RemoveFromCartDto
    {
        public IEnumerable<int> Ids { get; set; }
    }
}