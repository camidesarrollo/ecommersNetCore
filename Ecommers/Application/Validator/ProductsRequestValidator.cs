using Ecommers.Application.DTOs.Requests.Products;
using FluentValidation;

namespace Ecommers.Application.Validator
{
    public abstract class ProductsRequestValidator<T> : AbstractValidator<T>
     where T : ProductsBaseRequest
    {
        protected ProductsRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Debe seleccionar una categoría");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
        }
    }

    public class ProductsCreateRequestValidator : ProductsRequestValidator<ProductsCreateRequest>
    {
        public ProductsCreateRequestValidator() : base()
        {
            // reglas adicionales si las hubiera
        }
    }

    public class ProductsEditRequestValidator : ProductsRequestValidator<ProductsEditRequest>
    {
        public ProductsEditRequestValidator() : base()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El id no puede ser 0");
        }
    }
}
