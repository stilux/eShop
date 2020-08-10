using System.Collections.Generic;
using System.Threading.Tasks;
using ProductSearchService.Models;

namespace ProductSearchService.Services
{
    public interface IProductService
    {
        Task<Product> GetAsync(int id);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Product>> SearchAsync(string query, int take = 100);
    }
}