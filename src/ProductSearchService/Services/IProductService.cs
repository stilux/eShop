using System.Collections.Generic;
using System.Threading.Tasks;
using ProductSearchService.Dtos;
using ProductSearchService.Models;

namespace ProductSearchService.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetAsync(int id);
        Task<IEnumerable<ProductDto>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<ProductDto>> SearchAsync(string query, int take = 100);
    }
}