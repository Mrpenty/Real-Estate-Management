using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.Configurations
{

    public class NotificationConfiguration : IEntityTypeConfiguration<ApplicationUserNotification>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ApplicationUserNotification> builder)
        {
            builder.HasKey(un => new { un.NotificationId, un.UserId });
            builder.HasOne(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(un => un.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(un => un.IsRead).HasDefaultValue(false);
        }
    }
}
