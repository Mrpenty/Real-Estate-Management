using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.User;

public partial class ApplicationUserConfiguration
{
    public class InterestConfiguration : IEntityTypeConfiguration<UserFavoriteProperty>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteProperty> builder)
        {
            // Composite key
            builder.HasKey(ufp => new { ufp.UserId, ufp.PropertyId });

            // Default value for CreatedAt
            builder.Property(ufp => ufp.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            // Relationship to User
            builder.HasOne(ufp => ufp.User)
                   .WithMany(u => u.FavoriteProperties)
                   .HasForeignKey(ufp => ufp.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Relationship to Property
            builder.HasOne(ufp => ufp.Property)
                   .WithMany(p => p.FavoritedByUsers)
                   .HasForeignKey(ufp => ufp.PropertyId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Index for fast lookup
            builder.HasIndex(ufp => new { ufp.UserId, ufp.PropertyId });
        }
    }
}