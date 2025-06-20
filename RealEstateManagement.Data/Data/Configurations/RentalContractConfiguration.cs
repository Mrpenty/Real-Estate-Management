using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Data.Data.Configurations
{
    public class RentalContractConfiguration : IEntityTypeConfiguration<RentalContract>
    {
        public void Configure(EntityTypeBuilder<RentalContract> builder)
        {
            builder.HasKey(rc => rc.Id);

            builder.HasOne(rc => rc.PropertyPost)
                .WithOne(pp => pp.RentalContract)
                .HasForeignKey<RentalContract>(rc => rc.PropertyPostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rc => rc.Landlord)
                .WithMany()
                .HasForeignKey(rc => rc.LandlordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Renter)
                .WithMany()
                .HasForeignKey(rc => rc.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(rc => rc.DepositAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(rc => rc.MonthlyRent)
                .HasColumnType("decimal(18,2)");

            builder.Property(rc => rc.PaymentMethod)
                .HasMaxLength(100);

            builder.Property(rc => rc.ContactInfo)
                .HasMaxLength(200);



        }
    }
}
