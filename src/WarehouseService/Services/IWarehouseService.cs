using System.Threading.Tasks;
using WarehouseService.Models;

namespace WarehouseService.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseItemModel> GetWarehouseItemAsync(int productId);
        Task<int> ReserveAsync(ReserveModel model);
        Task CancelReservationAsync(int reserveId);
    }
}