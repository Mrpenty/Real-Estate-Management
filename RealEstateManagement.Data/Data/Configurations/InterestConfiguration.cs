using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class InterestConfiguration : IEntityTypeConfiguration<UserPreferenceFavoriteProperties>
    {
        public void Configure(EntityTypeBuilder<UserPreferenceFavoriteProperties> builder)
        {
            builder.HasKey(ufp => new { ufp.UserPreferenceId, ufp.PropertyId });

            builder.Property(ufp => ufp.CreatedAt).HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(ufp => ufp.UserPreference)
                   .WithMany(up => up.FavoriteProperties)
                   .HasForeignKey(ufp => ufp.UserPreferenceId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ufp => ufp.Property)
                   .WithMany(p => p.UserPreferences)
                   .HasForeignKey(ufp => ufp.PropertyId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Index for better query performance
            builder.HasIndex(ufp => new { ufp.UserPreferenceId, ufp.PropertyId });
        }
    }
}