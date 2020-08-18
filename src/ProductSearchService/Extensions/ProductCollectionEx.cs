using System.Collections.Generic;
using System.Linq;
using ProductSearchService.Dtos;
using ProductSearchService.Models;

namespace ProductSearchService.Extensions
{
    public static class ProductCollectionEx
    {
        public static IEnumerable<ProductDto> MapToProductDtoCollection(this IEnumerable<Product> products)
        {
            return products
                .Select(i => i.MapToProductDto())
                .ToList();
        }
    }
}