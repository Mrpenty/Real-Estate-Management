using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.Reviews;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.ReviewText)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(r => r.IsFlagged)
            .HasDefaultValue(false);

        builder.Property(r => r.IsVisible)
            .HasDefaultValue(true);

        builder.HasCheckConstraint("CK_Review_Rating_Range", "Rating BETWEEN 1 AND 5");

        // Quan hệ với Property (nhiều review thuộc về 1 property)
        builder.HasOne(r => r.Property)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Quan hệ với Renter (user) - KHÔNG cần navigation ngược ở ApplicationUser
        builder.HasOne(r => r.Renter)
            .WithMany()
            .HasForeignKey(r => r.RenterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ với RentalContract (mỗi review phải gắn với 1 contract)
        builder.HasOne(r => r.Contract)
            .WithMany()
            .HasForeignKey(r => r.ContractId)
            .OnDelete(DeleteBehavior.Restrict);

        // 1-1 với ReviewReply
        builder.HasOne(r => r.Reply)
            .WithOne(rr => rr.Review)
            .HasForeignKey<ReviewReply>(rr => rr.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);


        // Unique constraint: Mỗi contract chỉ được 1 review
        builder.HasIndex(r => r.ContractId)
            .IsUnique();
    }
}