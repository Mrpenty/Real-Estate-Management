using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Rating)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.IsApproved)
            .HasDefaultValue(false);

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // Apply check constraint at the entity level
        builder.HasCheckConstraint("CK_Review_Rating_Range", "Rating BETWEEN 1 AND 5");

        builder.HasOne(r => r.Property)
               .WithMany(p => p.Reviews)
               .HasForeignKey(r => r.PropertyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Renter)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.RenterId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}