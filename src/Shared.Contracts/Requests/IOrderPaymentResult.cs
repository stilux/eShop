namespace Shared.Contracts.Requests
{
    public interface IOrderPaymentResult
    {
        string PaymentFormUrl { get; }
        
        bool Success { get; }
    }
}