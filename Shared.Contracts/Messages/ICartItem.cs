namespace Shared.Contracts.Messages
{
    public interface ICartItem
    {
        int ProductId { get; set; }
        short Quantity { get; set; }
    }
}