using RealEstateManagement.Business.DTO.NotificationDTO;

namespace RealEstateManagement.Business.Services.NotificationService
{
    public interface INotificationService
    {
        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO createDto);
        Task<NotificationDTO> UpdateNotificationAsync(UpdateNotificationDTO updateDto);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<NotificationDTO?> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationDTO>> GetNotificationsByAudienceAsync(string audience);
        Task<IEnumerable<UserNotificationDTO>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<UserNotificationDTO>> GetUnreadUserNotificationsAsync(int userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId);
        Task<bool> MarkAllNotificationsAsReadAsync(int userId);
        Task<int> GetUnreadNotificationCountAsync(int userId);
        Task<bool> SendNotificationToAllUsersAsync(CreateNotificationDTO createDto);
        Task<bool> SendNotificationToRentersAsync(CreateNotificationDTO createDto);
        Task<bool> SendNotificationToLandlordsAsync(CreateNotificationDTO createDto);
        Task<bool> SendNotificationToSpecificUsersAsync(CreateNotificationDTO createDto);
        Task<bool> SendNotificationToAdminsAsync(CreateNotificationDTO createDto);
    }
} 