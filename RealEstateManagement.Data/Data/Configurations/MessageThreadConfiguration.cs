using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity;

public partial class ApplicationUserConfiguration
{
    public class MessageThreadConfiguration : IEntityTypeConfiguration<MessageThread>
    {
        public void Configure(EntityTypeBuilder<MessageThread> builder)
        {
            builder.Property(mt => mt.ThreadId).IsRequired();
            builder.Property(mt => mt.Content).IsRequired().HasMaxLength(1000);
            builder.Property(mt => mt.IsRead).HasDefaultValue(false);
            builder.Property(mt => mt.NotificationSent).HasDefaultValue(false);
            builder.Property(mt => mt.SentAt).HasDefaultValueSql("GETDATE()");

            builder.HasIndex(mt => new { mt.ThreadId, mt.SentAt });

            builder.HasOne(mt => mt.Renter)
                   .WithMany(u => u.MessagesAsRenter)
                   .HasForeignKey(mt => mt.RenterId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(mt => mt.Landlord)
                   .WithMany(u => u.MessagesAsLandlord)
                   .HasForeignKey(mt => mt.LandlordId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(mt => mt.Sender)
                   .WithMany()
                   .HasForeignKey(mt => mt.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("CK_DifferentParticipants", "RenterId != LandlordId");
        }
    }
}