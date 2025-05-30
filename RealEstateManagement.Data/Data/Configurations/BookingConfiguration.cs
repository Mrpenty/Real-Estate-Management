using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(b => b.DepositStatus).HasMaxLength(20)
                .HasConversion<string>();
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Renter)
                   .WithMany(u => u.Bookings)
                   .HasForeignKey(b => b.RenterId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Property)
                   .WithMany(p => p.Bookings)
                   .HasForeignKey(b => b.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}