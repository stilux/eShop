using System.Collections.Generic;

namespace Shared.Contracts.Messages
{
    public interface IOrderPayment : IOrderMessage
    {
        IList<ICartItem> Items { get; }
    }
}