using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Data.Entity.Notification
{
    public class ApplicationUserNotification
{
    public int NotificationId { get; set; }
    public Notification Notification { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public bool IsRead { get; set; } = false;
    }
}