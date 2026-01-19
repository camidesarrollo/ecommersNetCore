using Ecommers.Application.DTOs.Requests.AttributeValues;
using FluentValidation;

namespace Ecommers.Application.Validator
{
    public class AttributeValuesRequestValidatorr<T> : AbstractValidator<T>
        where T : AttributeValuesBaseRequest
    {
        protected AttributeValuesRequestValidatorr()
        {
            RuleFor(x => x.AttributeId)
                .GreaterThan(0)
                .WithMessage("La característica es obligatoria.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El orden de visualización no puede ser negativo.");

            RuleFor(x => x)
                .Must(HasAtLeastOneValue)
                .WithMessage("Debe ingresar al menos un valor para el atributo.");
        }

        private bool HasAtLeastOneValue(AttributeValuesBaseRequest valor)
        {
            return valor.ValueString != null ||
                   valor.ValueText != null ||
                   valor.ValueDecimal != null ||
                   valor.ValueInt != null ||
                   valor.ValueBoolean != null ||
                   valor.ValueDate != null;
        }
    }

    public class AttributeValuesCreateRequestValidator : AttributeValuesRequestValidatorr<AttributeValuesCreateRequest>
    {
        public AttributeValuesCreateRequestValidator() : base()
        {
            // reglas adicionales si las hubiera
        }
    }

    public class AttributeValuesEditRequestValidator : AttributeValuesRequestValidatorr<AttributeValuesEditRequest>
    {
        public AttributeValuesEditRequestValidator() : base()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El id no puede ser 0");
        }
    }
}
