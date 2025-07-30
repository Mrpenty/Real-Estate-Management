using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Business.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO createDto)
        {
            var notification = new Notification
            {
                Title = createDto.Title,
                Content = createDto.Content,
                Type = createDto.Type,
                Audience = createDto.Audience,
                CreatedAt = DateTime.UtcNow
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Create user notifications based on audience
            await CreateUserNotificationsForAudience(createdNotification, createDto);

            return MapToNotificationDTO(createdNotification);
        }

        public async Task<NotificationDTO> UpdateNotificationAsync(UpdateNotificationDTO updateDto)
        {
            var existingNotification = await _notificationRepository.GetNotificationByIdAsync(updateDto.Id);
            if (existingNotification == null)
                throw new ArgumentException("Notification not found");

            // Delete existing user notifications
            await _notificationRepository.DeleteUserNotificationsByNotificationIdAsync(updateDto.Id);

            // Update notification
            existingNotification.Title = updateDto.Title;
            existingNotification.Content = updateDto.Content;
            existingNotification.Type = updateDto.Type;
            existingNotification.Audience = updateDto.Audience;

            var updatedNotification = await _notificationRepository.UpdateNotificationAsync(existingNotification);

            // Recreate user notifications based on new audience
            var createDto = new CreateNotificationDTO
            {
                Title = updateDto.Title,
                Content = updateDto.Content,
                Type = updateDto.Type,
                Audience = updateDto.Audience,
                SpecificUserIds = updateDto.SpecificUserIds
            };

            await CreateUserNotificationsForAudience(updatedNotification, createDto);

            return MapToNotificationDTO(updatedNotification);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _notificationRepository.DeleteNotificationAsync(notificationId);
        }

        public async Task<NotificationDTO?> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            return notification != null ? MapToNotificationDTO(notification) : null;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync();
            return notifications.Select(MapToNotificationDTO);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsByAudienceAsync(string audience)
        {
            var notifications = await _notificationRepository.GetNotificationsByAudienceAsync(audience);
            return notifications.Select(MapToNotificationDTO);
        }

        public async Task<IEnumerable<UserNotificationDTO>> GetUserNotificationsAsync(int userId)
        {
            var userNotifications = await _notificationRepository.GetUserNotificationsAsync(userId);
            return userNotifications.Select(MapToUserNotificationDTO);
        }

        public async Task<IEnumerable<UserNotificationDTO>> GetUnreadUserNotificationsAsync(int userId)
        {
            var userNotifications = await _notificationRepository.GetUnreadUserNotificationsAsync(userId);
            return userNotifications.Select(MapToUserNotificationDTO);
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            return await _notificationRepository.MarkNotificationAsReadAsync(notificationId, userId);
        }

        public async Task<bool> MarkAllNotificationsAsReadAsync(int userId)
        {
            return await _notificationRepository.MarkAllNotificationsAsReadAsync(userId);
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            return await _notificationRepository.GetUnreadNotificationCountAsync(userId);
        }

        public async Task<bool> SendNotificationToAllUsersAsync(CreateNotificationDTO createDto)
        {
            createDto.Audience = "all";
            await CreateNotificationAsync(createDto);
            return true;
        }

        public async Task<bool> SendNotificationToRentersAsync(CreateNotificationDTO createDto)
        {
            createDto.Audience = "renters";
            await CreateNotificationAsync(createDto);
            return true;
        }

        public async Task<bool> SendNotificationToLandlordsAsync(CreateNotificationDTO createDto)
        {
            createDto.Audience = "landlords";
            await CreateNotificationAsync(createDto);
            return true;
        }

        public async Task<bool> SendNotificationToSpecificUsersAsync(CreateNotificationDTO createDto)
        {
            if (createDto.SpecificUserIds == null || !createDto.SpecificUserIds.Any())
                throw new ArgumentException("Specific user IDs are required for this audience type");
            // Get users from database
            var users = await _notificationRepository.GetUsersByIdsAsync(createDto.SpecificUserIds);

            // Find IDs that do not exist
            var foundIds = users.Select(u => u.Id).ToHashSet();
            var notFoundIds = createDto.SpecificUserIds.Where(id => !foundIds.Contains(id)).ToList();

            if (notFoundIds.Any())
            {
                var notFoundStr = string.Join(", ", notFoundIds);
                throw new ArgumentException(
                    $"User IDs not found: {notFoundStr}. You can search for users by phone number or email."
                );
            }
            createDto.Audience = "specific";
            await CreateNotificationAsync(createDto);
            return true;
        }

        private async Task CreateUserNotificationsForAudience(Notification notification, CreateNotificationDTO createDto)
        {
            var users = createDto.Audience.ToLower() switch
            {
                "all" => await _notificationRepository.GetUsersByAudienceAsync("all"),
                "renters" => await _notificationRepository.GetUsersByAudienceAsync("renters"),
                "landlords" => await _notificationRepository.GetUsersByAudienceAsync("landlords"),
                "specific" => await _notificationRepository.GetUsersByIdsAsync(createDto.SpecificUserIds ?? new List<int>()),
                _ => new List<ApplicationUser>()
            };

            var userNotifications = users.Select(user => new ApplicationUserNotification
            {
                NotificationId = notification.Id,
                UserId = user.Id,
                IsRead = false
            }).ToList();

            if (userNotifications.Any())
            {
                await _notificationRepository.CreateUserNotificationsAsync(userNotifications);
            }
        }

        private static NotificationDTO MapToNotificationDTO(Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Content = notification.Content,
                Type = notification.Type,
                Audience = notification.Audience,
                CreatedAt = notification.CreatedAt,
                IsRead = false, // This will be set per user
                RecipientCount = notification.UserNotifications?.Count ?? 0
            };
        }

        private static UserNotificationDTO MapToUserNotificationDTO(ApplicationUserNotification userNotification)
        {
            return new UserNotificationDTO
            {
                NotificationId = userNotification.NotificationId,
                Title = userNotification.Notification.Title,
                Content = userNotification.Notification.Content,
                Type = userNotification.Notification.Type,
                CreatedAt = userNotification.Notification.CreatedAt,
                IsRead = userNotification.IsRead
            };
        }
    }
} 