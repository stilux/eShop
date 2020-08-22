namespace OrderService.Enums
{
    public enum OrderStatus : byte
    {
        New = 1,
        Submitted = 2,
        ProductReserved = 3,
        Paid = 4,
        Shipped = 5,
        Delivered = 6,
        Cancelled = 7
    }
}