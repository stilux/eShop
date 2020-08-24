namespace Shared.Contracts.Events
{
    public interface IPaymentFailedEvent : IOrderEvent
    {
        string Reason { get; }
    }
}