using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Data.Entity;
using static ApplicationUserConfiguration;
using RealEstateManagement.Data.Data.Configurations;

public class RentalDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RentalContract> RentalContracts { get; set; }

    public DbSet<PropertyAmenity> PropertyAmenities { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyPost> PropertyPosts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Conversation> Conversation { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<UserPreferenceFavoriteProperties> UserPreferenceFavoriteProperties { get; set; } // Added

    public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new UserPreferenceConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyConfiguration());
        modelBuilder.ApplyConfiguration(new AmenityConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyAmenityConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyImageConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyPostConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new InterestConfiguration());
        modelBuilder.ApplyConfiguration(new RentalContractConfiguration());


        DataSeed.SeedData(modelBuilder);
    }
}