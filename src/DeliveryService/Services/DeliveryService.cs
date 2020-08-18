using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DeliveryService.Extensions;
using DeliveryService.Infrastructure;
using DeliveryService.Models;
using DeliveryService.Models.Dtos;

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

        public Task<ICollection<DeliveryMethodDto>> GetDeliveryMethodsAsync(string address, DateTime deliveryDate)
        {
            var deliveryMethods = new List<DeliveryMethodDto>();
            for (var i = 0; i < RandomNumberGenerator.GetInt32(1, DeliveryMethods.Length); i++)
            {
                deliveryMethods.Add(new DeliveryMethodDto
                {
                    DeliveryMethod = DeliveryMethods[i],
                    DeliveryAddress = address,
                    DeliveryDate = deliveryDate,
                    DeliveryCost = RandomNumberGenerator.GetInt32(100, 300)
                });
            }

            return Task.FromResult<ICollection<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<DeliveryRequestDto> CreateDeliveryRequestAsync(CreateDeliveryRequestDto model)
        {
            var request = model.MapToDeliveryRequest();
            
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            
            return request.MapToDeliveryRequestDto();
        }
    }
}