using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.Configurations
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<Domain_Layer.Entites.AttributeValue>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain_Layer.Entites.AttributeValue> builder)
        {
            builder.HasMany(x=>x.ProductVariants).WithOne(x=>x.AttributeValue).HasForeignKey(x=>x.AttributeValueId);

            builder.HasOne(x=>x.Attribute).WithMany(x=>x.Values).HasForeignKey(x=>x.AttributeId);

        }
    
    }
}
