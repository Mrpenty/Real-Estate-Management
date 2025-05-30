using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Role).HasMaxLength(20).IsRequired()
            .HasConversion<string>()
            .HasDefaultValue("renter");
        builder.Property(u => u.IsVerified).HasDefaultValue(false);
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasMany(u => u.Preferences)
               .WithOne(up => up.User)
               .HasForeignKey(up => up.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Properties)
               .WithOne(p => p.Landlord)
               .HasForeignKey(p => p.LandlordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Bookings)
               .WithOne(b => b.Renter)
               .HasForeignKey(b => b.RenterId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Reviews)
               .WithOne(r => r.Renter)
               .HasForeignKey(r => r.RenterId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.MessagesAsRenter)
               .WithOne(mt => mt.Renter)
               .HasForeignKey(mt => mt.RenterId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.MessagesAsLandlord)
               .WithOne(mt => mt.Landlord)
               .HasForeignKey(mt => mt.LandlordId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}