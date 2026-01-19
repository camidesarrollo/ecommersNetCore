using Ecommers.Application.DTOs.Requests.AttributeValues;

namespace Ecommers.Application.Common.Mappings
{
    public class AttributeValuesFromMapper
    {
        private static decimal? ParseDecimalIf(string tipo, string valor)
        {
            return tipo == "decimal" && decimal.TryParse(valor, out var d) ? d : null;
        }

        private static int? ParseIntIf(string tipo, string valor)
        {
            return tipo == "number" && int.TryParse(valor, out var i) ? i : null;
        }

        private static bool? ParseBoolIf(string tipo, string valor)
        {
            return tipo == "boolean" && bool.TryParse(valor, out var b) ? b : null;
        }

        public static AttributeValuesCreateRequest CrearNuevoValorAtributo(dynamic atributo, string valor)
        {
            return new AttributeValuesCreateRequest
            {
                AttributeId = atributo.Id,
                ValueString = atributo.DataType == "string" && valor.Length <= 225 ? valor : null,
                ValueText = atributo.DataType == "text" && valor.Length > 225 ? valor : null,
                ValueDecimal = ParseDecimalIf(atributo.DataType, valor),
                ValueInt = ParseIntIf(atributo.DataType, valor),
                ValueBoolean = ParseBoolIf(atributo.DataType, valor),
                ValueDate = null,
                IsActive = true
            };
        }

    }
}
