using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Data.DataSeed;
using RealEstateManagement.Data.Data.DataSeed.AddressSeed;
using RealEstateManagement.Data.Data.DataSeed.PaymentSeed;
using RealEstateManagement.Data.Data.DataSeed.PropertySeed;
using RealEstateManagement.Data.Data.DataSeed.User;


namespace RealEstateManagement.Data.Data
{
    public class MainDataSeed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            //user and role seeding
            RoleSeed.SeedRoles(modelBuilder);
            UserSeed.SeedUsers(modelBuilder);
            UserRoleSeed.SeedUserRoles(modelBuilder);
            UserPreferenceSeed.SeedUserPreferences(modelBuilder);

            //Poperty and related entities seeding
            AmenitySeed.SeedAmenities(modelBuilder);
            PropertySeed.SeedProperties(modelBuilder);
            PropertyAmenitySeed.SeedPropertyAmenities(modelBuilder);
            PropertyImageSeed.SeedPropertyImages(modelBuilder);
            PropertyPostSeed.SeedPropertyPosts(modelBuilder);

            //Payment and transaction seeding
            WalletSeed.SeedWallets(modelBuilder);
            WalletTransactionSeed.SeedWalletTransactions(modelBuilder);
            ReviewSeed.SeedReviews(modelBuilder);
            PackageSeed.SeedPackage(modelBuilder);
            PropertyPromotionSeed.SeedPromotionProperty(modelBuilder);
            RentalContractSeed.SeedRentalContracts(modelBuilder);

            //Address and related entities seeding
            AddressSeed.SeedAddress(modelBuilder);
            ProvinceSeed.SeedProvinces(modelBuilder);
            WardSeed.SeedWards(modelBuilder);
            StreetSeed.SeedStreets(modelBuilder);


        }
    }
}
