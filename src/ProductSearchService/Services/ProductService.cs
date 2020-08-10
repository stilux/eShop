using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductSearchService.Models;
using ProductSearchService.Providers;

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
        
        public async Task<Product> GetAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        
        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> SearchAsync(string query, int take = 100)
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
            return products.Take(take);
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