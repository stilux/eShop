using System;

namespace Shared.Contracts.Messages
{
    public interface IOrderMessage
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}