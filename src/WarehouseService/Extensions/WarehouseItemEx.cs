using WarehouseService.Entity;
using WarehouseService.Models;

namespace WarehouseService.Extensions
{
    public static class WarehouseItemEx
    {
        public static WarehouseItemModel ToWarehouseItemModel(this WarehouseItem item)
        {
            return new WarehouseItemModel
            {
                ProductId = item.ProductId,
                Balance = item.Balance
            };
        }
    }
}