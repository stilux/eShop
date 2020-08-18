using WarehouseService.Entity;
using WarehouseService.Models.Dtos;

namespace WarehouseService.Extensions
{
    public static class ReserveEx
    {
        public static ReservationResultDto MapToReservationResultDto(this Reserve reserve)
        {
            return new ReservationResultDto
            {
                ReserveId = reserve.Id
            };
        }
    }
}