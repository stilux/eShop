using System;

namespace Shared.Contracts.Messages
{
    public interface IDeliveryRequest : IOrderMessage
    {
        string DeliveryAddress { get; }
        string Recipient { get; }
        DateTime DeliveryDate { get; }
    }
}