using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.Property(up => up.Location).HasMaxLength(100);
            builder.Property(up => up.PriceRangeMin).HasColumnType("decimal(18,2)");
            builder.Property(up => up.PriceRangeMax).HasColumnType("decimal(18,2)");
            builder.Property(up => up.Amenities).HasMaxLength(200);
            builder.Property(up => up.CreatedAt).HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(up => up.User)
                    .WithMany(u => u.Preferences)
                    .HasForeignKey(up => up.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(up => up.FavoriteProperties)
                   .WithOne(ufp => ufp.UserPreference)
                   .HasForeignKey(ufp => ufp.UserPreferenceId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Add index for better query performance
            builder.HasIndex(up => up.UserId);
        }
    }
}