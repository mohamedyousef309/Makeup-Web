namespace Domain_Layer.Entites
{
    public class AttributeValue:BaseEntity
    {
        public string Value { get; set; } = default!; // مثلاً: Red

        public int AttributeId { get; set; }
        public Entites.Attribute Attribute { get; set; } = default!;

        // علاقة Many-to-Many مع الـ Variants
        public ICollection<VariantAttributeValue> ProductVariants { get; set; } = new HashSet<VariantAttributeValue>();
    }
}