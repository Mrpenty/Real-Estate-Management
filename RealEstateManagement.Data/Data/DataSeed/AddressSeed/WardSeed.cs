using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Data.Data.DataSeed.AddressSeed
{
    public static class WardSeed
    {
        public static void SeedWards(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ward>().HasData(
         // Phường/Xã thuộc Hồ Chí Minh City (ProvinceId = 1)
         new Ward { Id = 1, Name = "Ben Nghe", ProvinceId = 1 },
         new Ward { Id = 2, Name = "Nguyen Hue", ProvinceId = 1 },
         new Ward { Id = 3, Name = "Ward 10", ProvinceId = 1 },
         new Ward { Id = 4, Name = "Ward 4", ProvinceId = 1 },
         new Ward { Id = 5, Name = "Tan Dinh", ProvinceId = 1 },
         new Ward { Id = 6, Name = "Da Kao", ProvinceId = 1 },
         new Ward { Id = 7, Name = "Thao Dien", ProvinceId = 1 },
         new Ward { Id = 8, Name = "An Phu", ProvinceId = 1 },
         new Ward { Id = 9, Name = "Binh Thanh", ProvinceId = 1 },
         new Ward { Id = 10, Name = "Phu Nhuan", ProvinceId = 1 },

         // Phường/Xã thuộc Hanoi (ProvinceId = 2)
         new Ward { Id = 11, Name = "Hoan Kiem", ProvinceId = 2 },
         new Ward { Id = 12, Name = "Ba Dinh", ProvinceId = 2 },
         new Ward { Id = 13, Name = "Hai Ba Trung", ProvinceId = 2 },
         new Ward { Id = 14, Name = "Dong Da", ProvinceId = 2 },
         new Ward { Id = 15, Name = "Cau Giay", ProvinceId = 2 },
         new Ward { Id = 16, Name = "Tay Ho", ProvinceId = 2 },
         new Ward { Id = 17, Name = "Long Bien", ProvinceId = 2 },
         new Ward { Id = 18, Name = "Nam Tu Liem", ProvinceId = 2 }

         );
        }
    }
}