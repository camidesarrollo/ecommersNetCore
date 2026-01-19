using Ecommers.Application.DTOs.Requests.Products;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductFormMapper
    {
        public static  ProductsCreateRequest CrearProductoDesdeForm(IFormCollection form)
        {
            return new ProductsCreateRequest
            {
                Name = form["Products.Name"],
                Description = form["Products.Description"],
                ShortDescription = form["Products.ShortDescription"],
                CategoryId = int.TryParse(form["Products.CategoryId"], out var catId) ? catId : 0,
                BasePrice = decimal.TryParse(form["Products.BasePrice"], out var price) ? price : 0,
                Slug = form["Products.Slug"],
                IsActive = true
            };
        }

    }
}
