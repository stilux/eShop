using System;

namespace Shared.Contracts.Events
{
    public interface IProductsReservedEvent : IOrderEvent
    {
        int ReserveId { get; }
    }
}