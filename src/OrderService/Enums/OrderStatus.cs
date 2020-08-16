namespace OrderService.Enums
{
    public enum OrderStatus : byte
    {
        New = 1,
        Formed = 2,
        ProductReserved = 3,
        ReadyForDelivery = 4,
        Delivered = 5,
        Cancel = 6
    }
}