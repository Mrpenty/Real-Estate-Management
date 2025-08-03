using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Data.Data.DataSeed.AddressSeed
{
    public static class StreetSeed
    {
        public static void SeedStreets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Street>().HasData(
         // Streets for Ho Chi Minh City Wards
         new Street { Id = 1, Name = "Nguyen Hue", WardId = 2 },
         new Street { Id = 2, Name = "Le Van Tho", WardId = 3 },
         new Street { Id = 3, Name = "Ly Thuong Kiet", WardId = 4 },
         new Street { Id = 4, Name = "Hai Ba Trung", WardId = 5 },
         new Street { Id = 5, Name = "Pasteur", WardId = 6 },
         new Street { Id = 6, Name = "Quang Trung", WardId = 7 },
         new Street { Id = 7, Name = "An Phu", WardId = 8 },
         new Street { Id = 8, Name = "Dinh Bo Linh", WardId = 9 },
         new Street { Id = 9, Name = "Phan Dinh Phung", WardId = 10 },

         // Streets for Hanoi Wards
         new Street { Id = 10, Name = "Hang Dao", WardId = 11 },
         new Street { Id = 11, Name = "Hoang Dieu", WardId = 12 },
         new Street { Id = 12, Name = "Le Duan", WardId = 13 },
         new Street { Id = 13, Name = "Chua Boc", WardId = 14 },
         new Street { Id = 14, Name = "Nguyen Van Huyen", WardId = 15 },
         new Street { Id = 15, Name = "Thanh Nien", WardId = 16 },
         new Street { Id = 16, Name = "Nguyen Khoai", WardId = 17 },
         new Street { Id = 17, Name = "Pham Van Dong", WardId = 18 }

           );
        }
    }
}