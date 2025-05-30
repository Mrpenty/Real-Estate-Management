using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class PropertyPostConfiguration : IEntityTypeConfiguration<PropertyPost>
    {
        public void Configure(EntityTypeBuilder<PropertyPost> builder)
        {
            builder.Property(pp => pp.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(pp => pp.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(pp => pp.Property)
                   .WithMany(p => p.Posts)
                   .HasForeignKey(pp => pp.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pp => pp.Landlord)
                   .WithMany()
                   .HasForeignKey(pp => pp.LandlordId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pp => pp.VerifiedByUser)
                   .WithMany()
                   .HasForeignKey(pp => pp.VerifiedBy)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}