using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(n => n.Title)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(n => n.Slug)
                   .HasMaxLength(255);

            builder.Property(n => n.Summary)
                   .HasMaxLength(500);

            builder.Property(n => n.AuthorName)
                   .HasMaxLength(100);

            builder.Property(n => n.Source)
                   .HasMaxLength(255);

            builder.Property(n => n.IsPublished)
                   .HasDefaultValue(false);

            builder.Property(n => n.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.HasMany(n => n.Images)
                   .WithOne(img => img.News)
                   .HasForeignKey(img => img.NewsId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
