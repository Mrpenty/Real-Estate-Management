// User SignalR Connection Manager
let userSignalRConnection = null;

// Initialize SignalR connection for user pages
function initializeUserSignalR() {
    console.log('Initializing User SignalR...');
    console.log('authService available:', typeof authService !== 'undefined');
    console.log('authService.isAuthenticated():', typeof authService !== 'undefined' ? authService.isAuthenticated() : 'N/A');
    
    // Check if user is authenticated
    const token = localStorage.getItem('authToken');
    if (!token) {
        console.log('No auth token found, skipping SignalR initialization');
        return;
    }
    
    console.log('Auth token found, creating SignalR connection...');
    
    try {
        userSignalRConnection = new signalR.HubConnectionBuilder()
            .withUrl('http://194.233.81.64:5000/chatHub', { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();

        userSignalRConnection.start()
            .then(() => {
                console.log('✅ User SignalR connected successfully');
                console.log('Connection state:', userSignalRConnection.state);
                setupUserSignalRHandlers();
            })
            .catch(err => {
                console.error('❌ User SignalR connection failed:', err);
                console.error('Error details:', err.message);
            });

        // Handle connection errors
        userSignalRConnection.onclose((error) => {
            console.log('🔴 User SignalR connection closed');
            if (error) {
                console.error('Close error:', error);
            }
        });

    } catch (error) {
        console.error('Error creating User SignalR connection:', error);
    }
}

// Setup SignalR event handlers for user pages
function setupUserSignalRHandlers() {
    if (!userSignalRConnection) {
        console.log('❌ Cannot setup handlers: no SignalR connection');
        return;
    }
    
    console.log('🔧 Setting up SignalR event handlers...');
    
    // Log all available methods
    console.log('Available SignalR methods:', Object.getOwnPropertyNames(userSignalRConnection.__proto__));

    // Listen for new notifications
    userSignalRConnection.on('ReceiveNotification', (notification) => {
        console.log('🔔 New notification received:', notification);
        
        // Update notification count
        updateNotificationCount();
        
        // Show notification toast
        showNotificationToast(notification);
    });

    // Listen for chat messages
    userSignalRConnection.on('ReceiveMessage', (message) => {
        console.log('💬 New chat message received:', message);
        
        // Update chat notification if user is on chat page
        if (window.location.pathname.includes('/Chat')) {
            updateChatNotification();
        }
    });

    // Listen for support messages
    userSignalRConnection.on('SupportMessageReceived', (data) => {
        console.log('🆘 New support message received:', data);
        
        // Update support chat notification
        updateSupportChatNotification();
        
        // Auto-refresh support chat messages if chat is open
        if (typeof loadSimpleSupportMessages === 'function' && data.conversationId) {
            console.log('🔄 Auto-refreshing support chat messages for conversation:', data.conversationId);
            loadSimpleSupportMessages(data.conversationId);
        }
        
        // Also try to refresh if we have currentSupportConversationId
        if (typeof currentSupportConversationId !== 'undefined' && currentSupportConversationId) {
            console.log('🔄 Auto-refreshing support chat messages using currentSupportConversationId:', currentSupportConversationId);
            if (typeof loadSimpleSupportMessages === 'function') {
                loadSimpleSupportMessages(currentSupportConversationId);
            }
        }
    });
}

// Update notification count in header
function updateNotificationCount() {
    const notificationCount = document.getElementById('notificationCount');
    if (notificationCount) {
        // Increment count or show dot
        const currentCount = parseInt(notificationCount.textContent) || 0;
        notificationCount.textContent = currentCount + 1;
        notificationCount.classList.remove('hidden');
    }
}

// Show notification toast
function showNotificationToast(notification) {
    if (window.Swal) {
        Swal.fire({
            icon: 'info',
            title: 'Thông báo mới',
            text: notification.message || 'Bạn có thông báo mới',
            timer: 3000,
            showConfirmButton: false,
            toast: true,
            position: 'top-end'
        });
    }
}

// Update chat notification
function updateChatNotification() {
    const chatNotificationDot = document.getElementById('chat-notification-dot');
    if (chatNotificationDot) {
        chatNotificationDot.classList.remove('hidden');
    }
}

// Update support chat notification
function updateSupportChatNotification() {
    const supportUnreadBadge = document.getElementById('supportUnreadBadge');
    if (supportUnreadBadge) {
        const currentCount = parseInt(supportUnreadBadge.textContent) || 0;
        supportUnreadBadge.textContent = currentCount + 1;
        supportUnreadBadge.classList.remove('hidden');
    }
}

// Join conversation group
function joinConversation(conversationId) {
    if (userSignalRConnection && userSignalRConnection.state === 'Connected') {
        userSignalRConnection.invoke('JoinConversation', conversationId.toString());
    }
}

// Leave conversation group
function leaveConversation(conversationId) {
    if (userSignalRConnection && userSignalRConnection.state === 'Connected') {
        userSignalRConnection.invoke('LeaveConversation', conversationId.toString());
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    console.log('UserSignalR DOM loaded');
    
    // Try to initialize immediately
    initializeUserSignalR();
    
    // If not ready, retry multiple times
    let retryCount = 0;
    const maxRetries = 10;
    
    const retryInterval = setInterval(() => {
        retryCount++;
        console.log(`UserSignalR retry attempt ${retryCount}/${maxRetries}`);
        
        if (typeof authService !== 'undefined' && authService.isAuthenticated()) {
            console.log('authService ready, initializing SignalR...');
            initializeUserSignalR();
            clearInterval(retryInterval);
        } else if (retryCount >= maxRetries) {
            console.log('Max retries reached, giving up on SignalR initialization');
            clearInterval(retryInterval);
        }
    }, 500);
});

// Export functions for use in other scripts
window.userSignalR = {
    initialize: initializeUserSignalR,
    joinConversation: joinConversation,
    leaveConversation: leaveConversation,
    connection: () => userSignalRConnection,
    isConnected: () => userSignalRConnection && userSignalRConnection.state === 'Connected',
    getState: () => userSignalRConnection ? userSignalRConnection.state : 'No Connection'
}; 