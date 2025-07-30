# Real Estate Management - Notification System

## Overview
This document describes the comprehensive notification system implemented for the Real Estate Management application. The system allows administrators to send system notifications to all users, specific user groups (renters/landlords), or individual users.

## Features

### Admin Features
- **Create System Notifications**: Send notifications to different audiences
- **Update Notifications**: Modify existing notifications
- **Delete Notifications**: Remove notifications from the system
- **View All Notifications**: See all system notifications with recipient counts
- **Audience Targeting**: Send to all users, renters only, landlords only, or specific users

### User Features
- **View Personal Notifications**: See notifications sent to them
- **Mark as Read**: Mark individual notifications as read
- **Mark All as Read**: Mark all notifications as read at once
- **Unread Count**: See count of unread notifications
- **Real-time Updates**: Automatic updates of unread count

## Database Schema

### Notification Entity
```csharp
public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; } // info, warning, alert
    public DateTime CreatedAt { get; set; }
    public string Audience { get; set; } // all, renters, landlords, specific
    public ICollection<ApplicationUserNotification> UserNotifications { get; set; }
}
```

### ApplicationUserNotification Entity
```csharp
public class ApplicationUserNotification
{
    public int NotificationId { get; set; }
    public Notification Notification { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }
    public bool IsRead { get; set; }
}
```

## API Endpoints

### Admin Endpoints (Requires Admin Role)
- `POST /api/notification/create` - Create a new notification
- `POST /api/notification/send-to-all` - Send notification to all users
- `POST /api/notification/send-to-renters` - Send notification to renters only
- `POST /api/notification/send-to-landlords` - Send notification to landlords only
- `POST /api/notification/send-to-specific` - Send notification to specific users
- `PUT /api/notification/update` - Update an existing notification
- `DELETE /api/notification/delete/{id}` - Delete a notification
- `GET /api/notification/all` - Get all notifications
- `GET /api/notification/by-audience/{audience}` - Get notifications by audience
- `GET /api/notification/{id}` - Get notification by ID

### User Endpoints (Requires Authentication)
- `GET /api/usernotification/my-notifications` - Get user's notifications
- `GET /api/usernotification/unread` - Get user's unread notifications
- `GET /api/usernotification/unread-count` - Get unread notification count
- `POST /api/usernotification/mark-as-read/{id}` - Mark notification as read
- `POST /api/usernotification/mark-all-as-read` - Mark all notifications as read

## DTOs

### CreateNotificationDTO
```csharp
public class CreateNotificationDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public string Audience { get; set; }
    public List<int>? SpecificUserIds { get; set; }
}
```

### UpdateNotificationDTO
```csharp
public class UpdateNotificationDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public string Audience { get; set; }
    public List<int>? SpecificUserIds { get; set; }
}
```

### NotificationDTO
```csharp
public class NotificationDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public string Audience { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public int RecipientCount { get; set; }
}
```

### UserNotificationDTO
```csharp
public class UserNotificationDTO
{
    public int NotificationId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
```

## Usage Examples

### Creating a Notification for All Users
```javascript
const notificationData = {
    title: "System Maintenance",
    content: "The system will be down for maintenance on Sunday at 2 AM.",
    type: "warning",
    audience: "all"
};

const response = await fetch('/api/notification/send-to-all', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(notificationData)
});
```

### Creating a Notification for Specific Users
```javascript
const notificationData = {
    title: "Welcome to our platform!",
    content: "Thank you for joining our real estate platform.",
    type: "info",
    audience: "specific",
    specificUserIds: [1, 2, 3, 4]
};

const response = await fetch('/api/notification/send-to-specific', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(notificationData)
});
```

### Getting User Notifications
```javascript
const response = await fetch('/api/usernotification/my-notifications', {
    method: 'GET',
    headers: {
        'Authorization': 'Bearer ' + token
    }
});

const notifications = await response.json();
```

### Marking a Notification as Read
```javascript
const response = await fetch('/api/usernotification/mark-as-read/123', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token
    }
});
```

## Frontend Integration

### Admin Panel
- Access via `/Admin/NotificationManagement`
- Full CRUD operations for notifications
- Audience targeting options
- Real-time notification management

### User Interface
- Notification dropdown in header/navbar
- Unread count badge
- Mark as read functionality
- Responsive design

### JavaScript Services
- `NotificationService.js` - API communication
- `notifications.js` - UI interactions
- Real-time updates every 30 seconds

## Security
- Admin endpoints require admin role authentication
- User endpoints require user authentication
- Input validation on all DTOs
- SQL injection protection through Entity Framework
- XSS protection through proper encoding

## Performance Considerations
- Efficient database queries with proper indexing
- Pagination support for large notification lists
- Caching of unread counts
- Asynchronous operations for better responsiveness

## Future Enhancements
- Push notifications via SignalR
- Email notifications integration
- Notification templates
- Scheduled notifications
- Notification preferences per user
- Rich media support (images, links)
- Notification analytics and reporting

## Dependencies
- Entity Framework Core
- ASP.NET Core Identity
- jQuery (for admin panel)
- Bootstrap (for styling)
- Font Awesome (for icons)

## Installation
1. Ensure all required NuGet packages are installed
2. Run database migrations
3. Register services in `DependencyInjectionExtensions.cs`
4. Include JavaScript files in your views
5. Configure authentication and authorization

## Troubleshooting
- Check authentication tokens are valid
- Verify user roles are properly assigned
- Ensure database connections are working
- Check browser console for JavaScript errors
- Verify API endpoints are accessible 