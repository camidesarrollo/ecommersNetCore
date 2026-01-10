using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Domain.Extensions
{
    public static class AttributeValuesExtensions
    {
        public static string GetDisplayValue(this AttributeValues attributeValue)
        {
            if (attributeValue == null)
                return string.Empty;

            if (!string.IsNullOrEmpty(attributeValue.ValueString))
                return attributeValue.ValueString;

            if (!string.IsNullOrEmpty(attributeValue.ValueText))
                return attributeValue.ValueText;

            if (attributeValue.ValueDecimal.HasValue)
                return attributeValue.ValueDecimal.Value.ToString();

            if (attributeValue.ValueInt.HasValue)
                return attributeValue.ValueInt.Value.ToString();

            if (attributeValue.ValueDate.HasValue)
                return attributeValue.ValueDate.Value.ToString("dd/MM/yyyy");

            return attributeValue.ValueBoolean.ToString();
        }
    }
}
