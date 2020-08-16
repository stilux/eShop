using System;

namespace WarehouseService.Exceptions
{
    public class InsufficientProductQuantityException : Exception
    {
        public InsufficientProductQuantityException(int productId, int productBalance)
        {
            ProductId = productId;
            ProductBalance = productBalance;
        }
        public int ProductId { get; }
        
        public int ProductBalance { get; }
    }
}