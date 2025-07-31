using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Data.Entity;
using static ApplicationUserConfiguration;
using RealEstateManagement.Data.Data.Configurations;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity.Messages;

public class RentalDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    // DbSets for Identity
    public DbSet<UserPreference> UserPreferences { get; set; }

    // DbSets for Property Management
    public DbSet<Property> Properties { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RentalContract> RentalContracts { get; set; }
    public DbSet<PropertyAmenity> PropertyAmenities { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyPost> PropertyPosts { get; set; }


    // DbSets for Functionality
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<Conversation> Conversation { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<UserFavoriteProperty> UserFavoriteProperties { get; set; }

    public DbSet <PromotionPackage> promotionPackages { get; set; }
    public DbSet<PropertyPromotion> PropertyPromotions { get; set; }


    // DbSets for Address and Location
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<Street> Streets { get; set; }


    // DbSets for Notifications
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ApplicationUserNotification> ApplicationUserNotifications { get; set; }

    public DbSet<Slider> Sliders { get; set; }

    public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Identity
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new UserPreferenceConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());

        // Configure property entities
        modelBuilder.ApplyConfiguration(new PropertyConfiguration());
        modelBuilder.ApplyConfiguration(new AmenityConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyAmenityConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyImageConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyPostConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());

        // Configure function entities
        modelBuilder.ApplyConfiguration(new WalletConfiguration());
        modelBuilder.ApplyConfiguration(new WalletTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new InterestConfiguration());
        modelBuilder.ApplyConfiguration(new RentalContractConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyPromotionConfiguration());
        modelBuilder.ApplyConfiguration(new PromotionPackageConfiguration());

        MainDataSeed.SeedData(modelBuilder);
    }
}