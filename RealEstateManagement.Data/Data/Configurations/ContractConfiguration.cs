using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.Property(c => c.Deposit).HasColumnType("decimal(18,2)");
            builder.Property(c => c.RentAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(c => c.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(c => c.Booking)
                   .WithOne(b => b.Contract)
                   .HasForeignKey<Contract>(c => c.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}