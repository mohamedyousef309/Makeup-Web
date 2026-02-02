using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class VariantAttributeValue : BaseEntity
    {
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int AttributeValueId { get; set; }
        public AttributeValue AttributeValue { get; set; }
    }
}
