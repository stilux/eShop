using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductSearchService.Dtos;
using ProductSearchService.Extensions;
using ProductSearchService.Infrastructure;
using ProductSearchService.Models;

namespace ProductSearchService.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _context;
        
        private readonly IMemoryCache _cache;

        private readonly CacheConfig _cacheConfig;

        public ProductService(ProductContext context, IMemoryCache cache, IOptions<CacheConfig> config)
        {
            _context = context;
            _cache = cache;
            _cacheConfig = config.Value;
        }
        
        public async Task<ProductDto> GetAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product?.MapToProductDto();
        }
        
        public async Task<IEnumerable<ProductDto>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            return products.MapToProductDtoCollection();
        }
        
        public async Task<IEnumerable<ProductDto>> SearchAsync(string query, int take = 100)
        {
            var cacheEnabled = _cacheConfig.Enabled;
            IEnumerable<Product> products = null;
            
            if (cacheEnabled)
            {
                var key = $"{nameof(SearchAsync)}_{query.ToLower()}";
                products = await _cache.GetOrCreateAsync(key, entry =>
                {
                    entry.SetOptions(_cacheConfig.Options);
                    return SearchInternalAsync(query);
                });
            }
            else
            {
                products = await SearchInternalAsync(query);
            }
            
            return products.Take(take).MapToProductDtoCollection();
        }

        private async Task<IList<Product>> SearchInternalAsync(string query)
        {
            var pattern = $"%{query}%";
            return await _context.Products
                .AsNoTracking()
                .Where(p => EF.Functions.ILike(p.Name, pattern) 
                            || EF.Functions.ILike(p.Vendor, pattern)
                            || EF.Functions.ILike(p.VendorCode, pattern))
                .ToListAsync();
        }
    }
}