using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class AttributesComponentComponentVM
    {
        public List<MasterAttributesD> MasterAttributes { get; set; } = new();

        public List<AttributeValuesD> AtrributeValue { get; set; } = new();
    }
}
