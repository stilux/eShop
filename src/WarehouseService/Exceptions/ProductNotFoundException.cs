using System;

namespace WarehouseService.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int productId)
        {
            ProductId = productId;
        }
        public int ProductId { get; }
    }
}