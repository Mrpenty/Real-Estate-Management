using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.Payment;

public partial class ApplicationUserConfiguration
{
    public class PromotionPackageConfiguration : IEntityTypeConfiguration<PromotionPackage>
    {
        public void Configure(EntityTypeBuilder<PromotionPackage> builder)
        {
            builder.Property(pp => pp.Price)
                   .HasColumnType("decimal(18,2)") 
                   .HasPrecision(18, 2); 

            builder.Property(pp => pp.Name).HasMaxLength(100).IsRequired();
            builder.Property(pp => pp.Description).HasMaxLength(500);
            builder.Property(pp => pp.DurationInDays).IsRequired();
            builder.Property(pp => pp.Level).IsRequired();
            builder.Property(pp => pp.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(pp => pp.UpdatedAt);
            builder.Property(pp => pp.IsActive).HasDefaultValue(true);

            builder.HasMany(pp => pp.PropertyPromotions)
                   .WithOne(pp => pp.PromotionPackage)
                   .HasForeignKey(pp => pp.PackageId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}