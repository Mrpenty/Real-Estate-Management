using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Business.Repositories.NotificationRepository
{
    public interface INotificationRepository : IRepositoryAsync<Notification>
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<IEnumerable<Notification>> GetNotificationsByAudienceAsync(string audience);
        Task<IEnumerable<ApplicationUserNotification>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<ApplicationUserNotification>> GetUnreadUserNotificationsAsync(int userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId);
        Task<bool> MarkAllNotificationsAsReadAsync(int userId);
        Task<int> GetUnreadNotificationCountAsync(int userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByAudienceAsync(string audience);
        Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(List<int> userIds);
        Task<bool> CreateUserNotificationAsync(ApplicationUserNotification userNotification);
        Task<bool> CreateUserNotificationsAsync(List<ApplicationUserNotification> userNotifications);
        Task<bool> DeleteUserNotificationsByNotificationIdAsync(int notificationId);
    }
}
