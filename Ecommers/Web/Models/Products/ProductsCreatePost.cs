namespace Ecommers.Web.Models.Products
{

    public class ProductsCreatePost
    {
        public ProductDto Products { get; set; }
        public List<ProductImageDto> ProductImages { get; set; }
        public List<ProductVariantDto> ProductVariants { get; set; }
        public List<AttributeDto> Attributes { get; set; }
        public Dictionary<int, string> ProductsAttributes { get; set; }
        public int PrimaryImageIndex { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }
        public decimal BasePrice { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }

    public class ProductImageDto
    {
        public IFormFile ImageFile { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        // Diccionario para atributos de variante: MasterAttributeId -> Value
        public Dictionary<int, string> ProductVariantsAttributes { get; set; }

        public List<ProductVariantImageDto> ProductVariantImages { get; set; }
    }

    public class ProductVariantImageDto
    {
        public IFormFile ImageFile { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class AttributeDto
    {
        public int MasterAttributeId { get; set; }
    }
}
