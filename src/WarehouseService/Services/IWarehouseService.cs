using System.Threading.Tasks;
using WarehouseService.Models;
using WarehouseService.Models.Dtos;

namespace WarehouseService.Services
{
    public interface IWarehouseService
    {
        Task<int?> GetProductBalanceAsync(int productId);
        Task<ReservationResultDto> ReserveAsync(ReserveDto model);
        Task CancelReservationAsync(int reserveId);
    }
}