// Global notification service instance
const notificationService = new NotificationService();

// Function to mark a notification as read
async function markAsRead(notificationId) {
    try {
        const response = await notificationService.markAsRead(notificationId);
        if (response.success) {
            // Update UI to show notification as read
            const notificationElement = document.querySelector(`[data-notification-id="${notificationId}"]`);
            if (notificationElement) {
                notificationElement.classList.remove('unread');
                notificationElement.classList.add('read');
                
                // Remove the "Mark as Read" button
                const markReadBtn = notificationElement.querySelector('.mark-read-btn');
                if (markReadBtn) {
                    markReadBtn.remove();
                }
                
                // Remove the "New" badge
                const newBadge = notificationElement.querySelector('.badge-primary');
                if (newBadge) {
                    newBadge.remove();
                }
            }
            
            // Update unread count
            updateUnreadCount();
        } else {
            showAlert(response.message, 'error');
        }
    } catch (error) {
        console.error('Error marking notification as read:', error);
        showAlert('Error marking notification as read', 'error');
    }
}

// Function to mark all notifications as read
async function markAllAsRead() {
    try {
        const response = await notificationService.markAllAsRead();
        if (response.success) {
            // Update all notification items to show as read
            const unreadNotifications = document.querySelectorAll('.notification-item.unread');
            unreadNotifications.forEach(notification => {
                notification.classList.remove('unread');
                notification.classList.add('read');
                
                // Remove "Mark as Read" buttons
                const markReadBtn = notification.querySelector('.mark-read-btn');
                if (markReadBtn) {
                    markReadBtn.remove();
                }
                
                // Remove "New" badges
                const newBadge = notification.querySelector('.badge-primary');
                if (newBadge) {
                    newBadge.remove();
                }
            });
            
            // Update unread count
            updateUnreadCount();
            
            showAlert('All notifications marked as read', 'success');
        } else {
            showAlert(response.message, 'error');
        }
    } catch (error) {
        console.error('Error marking all notifications as read:', error);
        showAlert('Error marking all notifications as read', 'error');
    }
}

// Function to update unread count in the UI
async function updateUnreadCount() {
    try {
        const response = await notificationService.getUnreadCount();
        if (response.success) {
            const unreadCount = response.data.unreadCount;
            
            // Update notification badge in header/navbar
            const notificationBadge = document.getElementById('notification-badge');
            if (notificationBadge) {
                if (unreadCount > 0) {
                    notificationBadge.textContent = unreadCount;
                    notificationBadge.style.display = 'inline';
                } else {
                    notificationBadge.style.display = 'none';
                }
            }
        }
    } catch (error) {
        console.error('Error updating unread count:', error);
    }
}

// Function to load and display user notifications
async function loadUserNotifications() {
    try {
        const response = await notificationService.getMyNotifications();
        if (response.success) {
            displayUserNotifications(response.data);
        } else {
            showAlert('Error loading notifications', 'error');
        }
    } catch (error) {
        console.error('Error loading user notifications:', error);
        showAlert('Error loading notifications', 'error');
    }
}

// Function to display user notifications in the UI
function displayUserNotifications(notifications) {
    const container = document.getElementById('notifications-container');
    if (!container) return;

    if (notifications.length === 0) {
        container.innerHTML = '<p class="text-muted">No notifications to display</p>';
        return;
    }

    const notificationsHtml = notifications.map(notification => `
        <div class="notification-item ${notification.isRead ? 'read' : 'unread'}" data-notification-id="${notification.notificationId}">
            <div class="notification-content">
                <div class="notification-title">
                    <strong>${notification.title}</strong>
                    ${!notification.isRead ? '<span class="badge badge-primary">New</span>' : ''}
                </div>
                <div class="notification-text">${notification.content}</div>
                <div class="notification-meta">
                    <small class="text-muted">
                        <i class="fas fa-clock"></i> ${new Date(notification.createdAt).toLocaleString()}
                    </small>
                    <span class="badge badge-${getTypeBadgeClass(notification.type)}">${notification.type}</span>
                </div>
            </div>
            ${!notification.isRead ? `
                <button class="btn btn-sm btn-outline-secondary mark-read-btn" onclick="markAsRead(${notification.notificationId})">
                    Mark as Read
                </button>
            ` : ''}
        </div>
    `).join('');

    container.innerHTML = notificationsHtml;
}

// Helper function to get badge class for notification type
function getTypeBadgeClass(type) {
    switch(type?.toLowerCase()) {
        case 'info': return 'info';
        case 'warning': return 'warning';
        case 'alert': return 'danger';
        default: return 'secondary';
    }
}

// Function to show alerts
function showAlert(message, type) {
    const alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="close" data-dismiss="alert">
                <span>&times;</span>
            </button>
        </div>
    `;
    
    // Find a suitable container for the alert
    const container = document.querySelector('.container') || document.body;
    container.insertAdjacentHTML('afterbegin', alertHtml);
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        const alert = container.querySelector('.alert');
        if (alert) {
            alert.remove();
        }
    }, 5000);
}

// Initialize notifications when page loads
document.addEventListener('DOMContentLoaded', function() {
    // Update unread count on page load
    updateUnreadCount();
    
    // Set up periodic updates for unread count (every 30 seconds)
    setInterval(updateUnreadCount, 30000);
});

// Export functions for use in other scripts
window.notificationFunctions = {
    markAsRead,
    markAllAsRead,
    loadUserNotifications,
    updateUnreadCount
}; 