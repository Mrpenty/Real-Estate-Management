using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.Property(pi => pi.Url).HasMaxLength(200).IsRequired();
            builder.Property(pi => pi.IsPrimary).HasDefaultValue(false);

            builder.HasOne(pi => pi.Property)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}