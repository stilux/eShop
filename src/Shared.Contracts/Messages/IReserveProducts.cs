using System.Collections.Generic;

namespace Shared.Contracts.Messages
{
    public interface IReserveProducts : IOrderMessage
    {
        IList<ICartItem> Items { get; }
    }
}