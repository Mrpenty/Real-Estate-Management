using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ApplicationUserConfiguration
{
    public class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            builder.Property(pt => pt.Name).HasMaxLength(20);
            builder.Property(pt => pt.Description).HasMaxLength(200);
            builder.Property(pt => pt.CreatedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}