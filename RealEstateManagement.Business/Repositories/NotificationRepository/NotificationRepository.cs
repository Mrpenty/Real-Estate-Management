using RealEstateManagement.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.Repositories.Repository;

namespace RealEstateManagement.Business.Repositories.NotificationRepository
{
    public class NotificationRepository : RepositoryAsync<Notification>, INotificationRepository
    {
        private readonly RentalDbContext _context;

        public NotificationRepository(RentalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .Include(n => n.UserNotifications)
                .FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.UserNotifications)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByAudienceAsync(string audience)
        {
            return await _context.Notifications
                .Include(n => n.UserNotifications)
                .Where(n => n.Audience == audience)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUserNotification>> GetUserNotificationsAsync(int userId)
        {
            return await _context.ApplicationUserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUserNotification>> GetUnreadUserNotificationsAsync(int userId)
        {
            return await _context.ApplicationUserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId && !un.IsRead)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            var userNotification = await _context.ApplicationUserNotifications
                .FirstOrDefaultAsync(un => un.NotificationId == notificationId && un.UserId == userId);

            if (userNotification == null)
                return false;

            userNotification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllNotificationsAsReadAsync(int userId)
        {
            var userNotifications = await _context.ApplicationUserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToListAsync();

            foreach (var userNotification in userNotifications)
            {
                userNotification.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            return await _context.ApplicationUserNotifications
                .CountAsync(un => un.UserId == userId && !un.IsRead);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByAudienceAsync(string audience)
        {
            return audience.ToLower() switch
            {
                "all" => await _context.Users.Where(u => u.IsActive).ToListAsync(),
                "renters" => await _context.Users.Where(u => u.Role == "renter" && u.IsActive).ToListAsync(),
                "landlords" => await _context.Users.Where(u => u.Role == "landlord" && u.IsActive).ToListAsync(),
                _ => new List<ApplicationUser>()
            };
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByIdsAsync(List<int> userIds)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.Id) && u.IsActive)
                .ToListAsync();
        }

        public async Task<bool> CreateUserNotificationAsync(ApplicationUserNotification userNotification)
        {
            await _context.ApplicationUserNotifications.AddAsync(userNotification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateUserNotificationsAsync(List<ApplicationUserNotification> userNotifications)
        {
            await _context.ApplicationUserNotifications.AddRangeAsync(userNotifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserNotificationsByNotificationIdAsync(int notificationId)
        {
            var userNotifications = await _context.ApplicationUserNotifications
                .Where(un => un.NotificationId == notificationId)
                .ToListAsync();

            _context.ApplicationUserNotifications.RemoveRange(userNotifications);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}