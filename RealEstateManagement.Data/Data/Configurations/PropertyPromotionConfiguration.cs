using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ApplicationUserConfiguration
{
    public class PropertyPromotionConfiguration : IEntityTypeConfiguration<PropertyPromotion>
    {
        public void Configure(EntityTypeBuilder<PropertyPromotion> builder)
        {
            builder.HasKey(pp => pp.Id);


            builder.HasOne(pp => pp.Property)
             .WithMany(p => p.PropertyPromotions)
             .HasForeignKey(pp => pp.PropertyId)
             .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(pp => pp.PromotionPackage)
             .WithMany(p => p.PropertyPromotions)
             .HasForeignKey(pp => pp.PackageId)
             .OnDelete(DeleteBehavior.Cascade);
        
    }
    }
}
