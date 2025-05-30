using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.PaymentMethod).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.Status).HasMaxLength(20).IsRequired()
                .HasConversion<string>();

            builder.HasOne(p => p.Contract)
                   .WithMany(c => c.Payments)
                   .HasForeignKey(p => p.ContractId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Transaction)
                   .WithMany(t => t.Payments)
                   .HasForeignKey(p => p.TransactionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}