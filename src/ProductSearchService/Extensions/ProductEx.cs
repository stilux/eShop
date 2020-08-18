using ProductSearchService.Dtos;
using ProductSearchService.Models;

namespace ProductSearchService.Extensions
{
    public static class ProductEx
    {
        public static ProductDto MapToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                VendorCode = product.VendorCode,
                Vendor = product.Vendor,
                Price = product.Price,
                Discount = product.Discount,
                PictureUrl = product.PictureUrl,
                Weight = product.Weight,
                Dimensions = product.Dimensions
            };
        }
    }
}