using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.DataSeed.NewsSeed
{
    public static class NewsImageSeed
    {
        public static void SeedNewsImages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsImage>().HasData(
                new NewsImage
                {
                    Id = 1,
                    NewsId = 1,
                    ImageUrl = "/uploads/news/news1-img1.jpg"
                },
                new NewsImage
                {
                    Id = 2,
                    NewsId = 2,
                    ImageUrl = "/uploads/news/news2-img1.jpg"
                }
            );
        }
    }

}
