using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryService.Models;

namespace DeliveryService.Services
{
    public interface IDeliveryService
    {
        Task<ICollection<DeliveryMethodModel>> GetDeliveryMethodsAsync(string address, DateTime deliveryDate);
        Task<DeliveryRequestModel> CreateDeliveryRequestAsync(CreateDeliveryRequestModel model);
    }
}