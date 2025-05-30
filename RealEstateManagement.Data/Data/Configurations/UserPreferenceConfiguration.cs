// Configurations/UserPreferenceConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

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
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(up => up.FavoriteProperties)
               .WithMany(p => p.UserPreferences)
               .UsingEntity<UserPreferenceFavoriteProperties>(
                   j => j
                       .HasOne(ufp => ufp.Property)
                       .WithMany()
                       .HasForeignKey(ufp => ufp.PropertyId)
                       .OnDelete(DeleteBehavior.NoAction),
                   j => j
                       .HasOne(ufp => ufp.UserPreference)
                       .WithMany()
                       .HasForeignKey(ufp => ufp.UserPreferenceId)
                       .OnDelete(DeleteBehavior.NoAction),
                   j =>
                   {
                       j.ToTable("UserPreferenceFavoriteProperties");
                       j.HasKey(ufp => new { ufp.UserPreferenceId, ufp.PropertyId });
                   });
    }
}