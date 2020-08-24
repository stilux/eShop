namespace Shared.Contracts.Requests
{
    public interface IReserveProductsResult
    {
        int ReserveId { get; }
        
        bool Success { get; }
    }
}