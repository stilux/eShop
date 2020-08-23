namespace Shared.Contracts.Messages
{
    public interface ICancelReservation : IOrderMessage
    {
        int ReserveId { get; }
    }
}