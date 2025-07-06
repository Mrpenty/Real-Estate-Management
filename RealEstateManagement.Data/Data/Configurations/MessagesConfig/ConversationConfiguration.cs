using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.Messages;

public partial class ApplicationUserConfiguration
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasKey(c => c.Id);

            // Relationships
            builder.HasOne(c => c.Renter)
                .WithMany(u => u.ConversationsAsRenter)
                .HasForeignKey(c => c.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Landlord)
                .WithMany(u => u.ConversationsAsLandlord)
                .HasForeignKey(c => c.LandlordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Property)
                .WithMany(p => p.Conversations)
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.LastMessage)
                .HasMaxLength(1000) 
                .IsRequired(false); 

            builder.Property(c => c.LastSentAt)
                .IsRequired(false); 

            builder.HasIndex(c => c.RenterId);
            builder.HasIndex(c => c.LandlordId);
            builder.HasIndex(c => c.PropertyId);
        }
    }


}