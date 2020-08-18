using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductSearchService.Infrastructure;
using ProductSearchService.Models;

namespace ProductSearchService.Services
{
    public class ProductDbInitializerService : IProductDbInitializerService
    {
        private const int BatchSizeLimit = 1000;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProductDbInitializerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async void GenerateProducts(int count)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ProductContext>();
            
            var countInDb = await context.Products.CountAsync();
            if (countInDb > 0) return;

            var processedItemsCount = 0;

            while (processedItemsCount < count)
            {
                var remainder = count - processedItemsCount;
                var currentBatchSize = remainder >= BatchSizeLimit ? BatchSizeLimit : remainder;

                var products = new Faker<Product>()
                    .RuleFor(p => p.Vendor, (faker, product) => faker.Company.CompanyName())
                    .RuleFor(p => p.VendorCode, (faker, product) => faker.Commerce.Ean13())
                    .RuleFor(p => p.Name, (faker, product) => faker.Commerce.ProductName())
                    .RuleFor(p => p.Price, (faker, product) => MathF.Round(faker.Random.Float(100, 10000), 2))
                    .RuleFor(p => p.Discount, (faker, product) => MathF.Round(faker.Random.Float(0, 30), 2))
                    .RuleFor(p => p.Weight, (faker, product) => faker.Random.Int(10, 1000))
                    .RuleFor(p => p.PictureUrl, (faker, product) => faker.Image.PicsumUrl())
                    .RuleFor(p => p.Dimensions, (faker, product) => 
                        $"{faker.Random.Int(10, 1000)}X{faker.Random.Int(10, 1000)}X{faker.Random.Int(10, 1000)}")
                    .Generate(currentBatchSize);
                
                await context.AddRangeAsync(products);
                var insertedItemsCount = await context.SaveChangesAsync();

                processedItemsCount += insertedItemsCount;
            }
        }
    }
}