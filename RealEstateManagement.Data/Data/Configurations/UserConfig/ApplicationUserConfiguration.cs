using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.User;
using System.Reflection.Emit;
using RealEstateManagement.Data.Entity.PropertyEntity;

public partial class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Role).HasMaxLength(20).IsRequired()
            .HasConversion<string>()
            .HasDefaultValue("renter");
        //  builder.Property(u => u.PhoneNumber).HasMaxLength(11).IsRequired();
        builder.Property(u => u.IsVerified).HasDefaultValue(false);
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(u => u.RefreshToken)
           .IsRequired(false)
           .HasMaxLength(500);
        builder.Property(u => u.RefreshTokenExpiryTime)
            .IsRequired(false);

        builder.Property(u => u.UserName)
        .HasMaxLength(256) 
        .IsRequired(false); 

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(256) 
            .IsRequired(false); 

        // Relationships
        builder.HasMany(u => u.Preferences)
               .WithOne(up => up.User)
               .HasForeignKey(up => up.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Properties)
               .WithOne(p => p.Landlord)
               .HasForeignKey(p => p.LandlordId)
               .OnDelete(DeleteBehavior.Cascade);



        builder.HasMany(u => u.Reviews)
               .WithOne(r => r.Renter)
               .HasForeignKey(r => r.RenterId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.ConversationsAsRenter)
               .WithOne(c => c.Renter)
               .HasForeignKey(c => c.RenterId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ConversationsAsLandlord)
               .WithOne(c => c.Landlord)
               .HasForeignKey(c => c.LandlordId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.MessagesSent)
               .WithOne(m => m.Sender)
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);


    }
}