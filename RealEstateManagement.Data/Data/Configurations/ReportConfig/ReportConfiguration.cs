using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.ReportEntity;

namespace RealEstateManagement.Data.Data.Configurations.ReportConfig
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasOne(r => r.ReportedByUser)
                .WithMany(u => u.ReportsCreated)
                .HasForeignKey(r => r.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ResolvedByUser)
                .WithMany(u => u.ReportsResolved)
                .HasForeignKey(r => r.ResolvedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.TargetType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Reason)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
