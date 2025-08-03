using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Data.Configurations.UserConfig
{
    public class InterestedPropertyConfiguration : IEntityTypeConfiguration<InterestedProperty>
    {
        public void Configure(EntityTypeBuilder<InterestedProperty> builder)
        {
            builder.HasKey(ip => ip.Id);

            // Enum Status lưu dưới dạng int
            builder.Property(ip => ip.Status)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(ip => ip.InterestedAt)
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();

            builder.Property(ip => ip.RenterConfirmed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(ip => ip.LandlordConfirmed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(ip => ip.RenterReplyAt)
                   .IsRequired(false);

            builder.Property(ip => ip.LandlordReplyAt)
                   .IsRequired(false);

            // Quan hệ với Property
            builder.HasOne(ip => ip.Property)
                   .WithMany() // hoặc .WithMany(p => p.InterestedProperties) nếu có navigation
                   .HasForeignKey(ip => ip.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với Renter (User)
            builder.HasOne(ip => ip.Renter)
                   .WithMany() // hoặc .WithMany(u => u.InterestedProperties) nếu có navigation
                   .HasForeignKey(ip => ip.RenterId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Nếu cần unique: 1 user chỉ có 1 InterestedProperty cho 1 property
            builder.HasIndex(ip => new { ip.PropertyId, ip.RenterId }).IsUnique();

            // (Optional) Index cho hiệu suất truy vấn
            builder.HasIndex(ip => ip.Status);
        }
    }
}
