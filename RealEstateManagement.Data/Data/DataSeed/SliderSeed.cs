using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Data.Data.DataSeed
{
    public static class SliderSeed
    {
        public static void SeedSliders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slider>().HasData(
                new Slider
                {
                    Id = 1,
                    ImageUrl = "/image/slider1.jpg",
                    Title = "slider1",
                    Description = "slider1",
                    CreatedAt = DateTime.Now
                },
                new Slider
                {
                    Id = 2,
                    ImageUrl = "/image/slider2.jpg",
                    Title = "slider2",
                    Description = "slider2",
                    CreatedAt = DateTime.Now
                },
                new Slider
                {
                    Id = 3,
                    ImageUrl = "/image/slider3.jpg",
                    Title = "slider3",
                    Description = "slider3",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}