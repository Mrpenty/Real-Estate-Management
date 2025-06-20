using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.PropertyEntity;

public partial class ApplicationUserConfiguration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Type).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.Area).HasColumnType("decimal(10,2)");
            builder.Property(p => p.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.IsPromoted).HasDefaultValue(false);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();

            builder.Property(p => p.IsVerified).HasDefaultValue(false);
            builder.Property(p => p.ViewsCount).HasDefaultValue(0);
            builder.Property(p => p.Location).HasMaxLength(50);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(p => p.Landlord)
                   .WithMany(u => u.Properties)
                   .HasForeignKey(p => p.LandlordId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Images)
                   .WithOne()
                   .HasForeignKey("PropertyId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Posts)
                   .WithOne()
                   .HasForeignKey("PropertyId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Reviews)
                   .WithOne(r => r.Property)
                   .HasForeignKey(r => r.PropertyId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Conversations)
                   .WithOne(c => c.Property)
                   .HasForeignKey(c => c.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.PropertyAmenities)
                   .WithOne()
                   .HasForeignKey("PropertyId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Address)
                .WithOne(a => a.Property)
                .HasForeignKey<Property>(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}