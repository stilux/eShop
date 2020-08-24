using System;

namespace Shared.Contracts.Events
{
    public interface IOrderEvent
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}