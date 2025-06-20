using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Data.Data.DataSeed.AddressSeed
{
    public static class ProvinceSeed
    {
        public static void SeedProvinces(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Ho Chi Minh City" },
                new Province { Id = 2, Name = "Hanoi" }
            );
        }
    }
}