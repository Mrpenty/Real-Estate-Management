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
    public class NewsImageConfiguration : IEntityTypeConfiguration<NewsImage>
    {
        public void Configure(EntityTypeBuilder<NewsImage> builder)
        {
            builder.HasOne(img => img.News)
                   .WithMany(news => news.Images)
                   .HasForeignKey(img => img.NewsId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.ImageUrl).IsRequired().HasMaxLength(255);
        }
    }

}
