namespace Shared.Contracts.Messages
{
    public interface IChangeOrderStatus : IOrderMessage
    {
        byte OrderStatusId { get; }
    }
}