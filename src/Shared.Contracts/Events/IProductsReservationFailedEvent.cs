using System;

namespace Shared.Contracts.Events
{
    public interface IProductsReservationFailedEvent : IOrderEvent
    {
        string Reason { get; }
    }
}