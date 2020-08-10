namespace ProductSearchService.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string VendorCode { get; set; }
        
        public string Vendor { get; set; }

        public float Price { get; set; }
        
        public float Discount { get; set; }
        
        public string PictureUrl { get; set; }
        
        public float Weight { get; set; }
        
        public string Dimensions { get; set; }
        
        public int WarehouseBalance { get; set; }
    }
}