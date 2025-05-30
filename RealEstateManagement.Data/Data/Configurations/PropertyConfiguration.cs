using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Type).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.Area).HasColumnType("decimal(10,2)");
            
            builder.Property(p => p.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.IsPromoted).HasDefaultValue(false);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.MinPrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.MaxPrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.IsVerified).HasDefaultValue(false);
            builder.Property(p => p.ViewsCount).HasDefaultValue(0);
            builder.Property(p => p.Location).HasMaxLength(50);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(p => p.Landlord)
                   .WithMany(u => u.Properties)
                   .HasForeignKey(p => p.LandlordId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}