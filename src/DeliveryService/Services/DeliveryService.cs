using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DeliveryService.Extensions;
using DeliveryService.Models;
using DeliveryService.Providers;

namespace DeliveryService.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DeliveryContext _context;
        private static readonly string[] DeliveryMethods = { "Доставка курьером", "Пункт выдачи", "Постамат"};

        public DeliveryService(DeliveryContext context)
        {
            _context = context;
        }

        public Task<ICollection<DeliveryMethodModel>> GetDeliveryMethodsAsync(string address, DateTime deliveryDate)
        {
            var deliveryMethods = new List<DeliveryMethodModel>();
            for (var i = 0; i < RandomNumberGenerator.GetInt32(1, DeliveryMethods.Length); i++)
            {
                deliveryMethods.Add(new DeliveryMethodModel
                {
                    DeliveryMethod = DeliveryMethods[i],
                    DeliveryDate = deliveryDate,
                    DeliveryCost = RandomNumberGenerator.GetInt32(100, 300)
                });
            }

            return Task.FromResult<ICollection<DeliveryMethodModel>>(deliveryMethods);
        }

        public async Task<DeliveryRequestModel> CreateDeliveryRequestAsync(CreateDeliveryRequestModel model)
        {
            var request = model.MapToDeliveryRequest();
            
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            
            return request.MapToDeliveryRequestModel();
        }
    }
}