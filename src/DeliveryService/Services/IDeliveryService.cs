using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryService.Models;
using DeliveryService.Models.Dtos;

namespace DeliveryService.Services
{
    public interface IDeliveryService
    {
        Task<ICollection<DeliveryMethodDto>> GetDeliveryMethodsAsync(string address, DateTime deliveryDate);
        Task<DeliveryRequestDto> CreateDeliveryRequestAsync(CreateDeliveryRequestDto model);
    }
}