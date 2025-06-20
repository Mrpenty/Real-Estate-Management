using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.Payment;

public partial class ApplicationUserConfiguration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(t => t.TransactionType).HasMaxLength(20).IsRequired()
                .HasConversion<string>();
            builder.Property(t => t.Description).HasMaxLength(200);
            builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(t => t.User)
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}