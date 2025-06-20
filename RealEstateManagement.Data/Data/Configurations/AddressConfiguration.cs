using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

public partial class ApplicationUserConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

            builder.HasOne(a => a.Property)
               .WithOne(p => p.Address)
               .HasForeignKey<Property>(p => p.AddressId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Province)
            .WithMany(p => p.Addresses)
            .HasForeignKey(a => a.ProvinceId);

            builder.HasOne(a => a.Ward)
                .WithMany(w => w.Addresses)
                .HasForeignKey(a => a.WardId);

            builder.HasOne(a => a.Street)
                .WithMany(s => s.Addresses)
                .HasForeignKey(a => a.StreetId);

        }
    }
    
    
}
