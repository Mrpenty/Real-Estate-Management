using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.DataSeed.User
{
    public static class InterestedPropertySeed
    {
        public static void SeedInterestedProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InterestedProperty>().HasData(
                new InterestedProperty
                {
                    Id = 1,
                    PropertyId = 1,
                    RenterId = 3,
                    InterestedAt = DateTime.Now.AddHours(-24),
                    Status = (InterestedStatus)1, // WaitingForRenterReply
                    RenterReplyAt = null,
                    LandlordReplyAt = null
                },
                new InterestedProperty
                {
                    Id = 2,
                    PropertyId = 2,
                    RenterId = 4,
                    InterestedAt = DateTime.Now.AddHours(-12),
                    Status = (InterestedStatus)2, // RenterWantToRent
                    RenterReplyAt = DateTime.Now.AddHours(-10),
                    LandlordReplyAt = null
                },
                new InterestedProperty
                {
                    Id = 3,
                    PropertyId = 3,
                    RenterId = 1,
                    InterestedAt = DateTime.Now.AddHours(-5),
                    Status = (InterestedStatus)3, // RenterNotRent
                    RenterReplyAt = DateTime.Now.AddHours(-3),
                    LandlordReplyAt = null
                }
            );
        }
    }
}
