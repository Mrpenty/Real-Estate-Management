using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public class PropertyTypeSeed
    {
        public static void SeedPropertyTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstateManagement.Data.Entity.PropertyEntity.PropertyType>().HasData(
               new PropertyType
               {
                   Id = 1,
                   Name = "Căn hộ",
                   Description = "Một đơn vị nhà ở độc lập nằm trong một tòa nhà chung cư.",
                   CreatedAt = DateTime.Now
               },
              new PropertyType
              {
                  Id = 2,
                  Name = "Nhà riêng",
                  Description = "Một tòa nhà dân cư độc lập, không chia sẻ tường với nhà khác.",
                  CreatedAt = DateTime.Now
              },
               new PropertyType
               {
                   Id = 3,
                   Name = "Chung cư",
                   Description = "Tòa nhà hoặc khu phức hợp chứa nhiều căn hộ hoặc nhà thuộc sở hữu cá nhân.",
                   CreatedAt = DateTime.Now
               },
             new PropertyType
             {
                 Id = 4,
                 Name = "Nhà phố",
                 Description = "Nhà cao, hẹp, thường có ba tầng trở lên, nằm trong dãy nhà liền kề.",
                 CreatedAt = DateTime.Now
             },
            new PropertyType
            {
                Id = 5,
                Name = "Biệt thự",
                Description = "Một ngôi nhà lớn và sang trọng, thường nằm ở khu vực ngoại ô hoặc nông thôn.",
                CreatedAt = DateTime.Now
            },
            new PropertyType
            {
                Id = 6,
                Name = "Phòng trọ ",
                Description = "Một ngôi nhà lớn và sang trọng, thường nằm ở khu vực ngoại ô hoặc nông thôn.",
                CreatedAt = DateTime.Now
            }
        );
        }
    }
}