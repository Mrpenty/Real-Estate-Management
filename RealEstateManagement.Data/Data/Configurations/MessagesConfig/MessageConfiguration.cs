using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateManagement.Data.Entity.Messages;

public partial class ApplicationUserConfiguration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            // Relationships
            //builder.HasOne(m => m.Conversation)
            //    .WithMany(c => c.Messages)
            //    .HasForeignKey(m => m.ConversationId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(m => m.ConversationId);
            builder.HasIndex(m => m.SenderId);
            builder.HasIndex(m => m.SentAt);
        }
    }
}