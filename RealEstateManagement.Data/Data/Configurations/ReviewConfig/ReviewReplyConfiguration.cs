using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.Configurations.ReviewConfig
{
    public class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReply>
    {
            public void Configure(EntityTypeBuilder<ReviewReply> builder)
            {
                builder.HasKey(r => r.Id);

                builder.Property(r => r.ReplyContent)
                    .IsRequired()
                    .HasMaxLength(2000);

                builder.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                builder.Property(r => r.IsFlagged)
                    .HasDefaultValue(false);

                builder.Property(r => r.IsVisible)
                    .HasDefaultValue(true);

                // Một reply chỉ thuộc về một review (1-1)
                builder.HasOne(r => r.Review)
                    .WithOne(rev => rev.Reply)
                    .HasForeignKey<ReviewReply>(r => r.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Một landlord có thể có nhiều reply (1-n)
                builder.HasOne(r => r.Landlord)
                    .WithMany()
                    .HasForeignKey(r => r.LandlordId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }

