﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;

public partial class ApplicationUserConfiguration
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.Property(up => up.Location).HasMaxLength(100);
            builder.Property(up => up.PriceRangeMin).HasColumnType("decimal(18,2)");
            builder.Property(up => up.PriceRangeMax).HasColumnType("decimal(18,2)");
            builder.Property(up => up.Amenities).HasMaxLength(200);
            builder.Property(up => up.CreatedAt).HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(up => up.User)
                   .WithMany(u => u.Preferences)
                   .HasForeignKey(up => up.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(up => up.UserId);
        }
    }
}