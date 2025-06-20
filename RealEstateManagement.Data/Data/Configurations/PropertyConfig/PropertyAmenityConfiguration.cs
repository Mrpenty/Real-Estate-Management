﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.PropertyEntity;

public partial class ApplicationUserConfiguration
{
    public class PropertyAmenityConfiguration : IEntityTypeConfiguration<PropertyAmenity>
    {
        public void Configure(EntityTypeBuilder<PropertyAmenity> builder)
        {
            builder.HasKey(pa => new { pa.PropertyId, pa.AmenityId });

            builder.HasOne(pa => pa.Property)
                   .WithMany(p => p.PropertyAmenities)
                   .HasForeignKey(pa => pa.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.Amenity)
                   .WithMany(a => a.PropertyAmenities)
                   .HasForeignKey(pa => pa.AmenityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}