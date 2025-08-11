// Configuration file for API endpoints and other settings
window.config = {
    // API Base URL
    apiBaseUrl: 'https://localhost:7031',
    
    // Chat API endpoints
    chat: {
        base: '/api/chat',
        createConversation: '/api/chat/Create-Conversation',
        listConversations: '/api/chat/List-Conversation',
        getMessages: '/api/chat/conversation',
        messageCount: '/api/chat/conversation',
        deleteMessage: '/api/chat/delete-message',
        editMessage: '/api/chat/edit-message',
        searchConversations: '/api/chat/search-conversation',
        
        // Support endpoints
        createSupportConversation: '/api/chat/create-support-conversation',
        userSupportConversations: '/api/chat/user-support-conversations',
        supportMessages: '/api/chat/support-conversation',
        sendSupportMessage: '/api/chat/support-conversation',
        adminAllSupportConversations: '/api/chat/admin/all-support-conversations',
        adminReply: '/api/chat/admin/support-conversation',
        adminUpdateStatus: '/api/chat/admin/support-conversation'
    },
    
    // User API endpoints
    user: {
        notifications: '/api/UserNotification',
        wallet: '/api/Wallet'
    },
    
    // Admin API endpoints
    admin: {
        users: {
            adminList: '/api/User/admin-list',
            detail: '/api/User/{userId}',
            update: '/api/User/admin-update/{userId}',
            delete: '/api/User/delete/{userId}'
        },
        posts: {
            list: '/api/Admin/PropertyPosts',
            detail: '/api/Admin/PropertyPosts/{postId}',
            update: '/api/Admin/PropertyPosts/{postId}/status',
            delete: '/api/Admin/PropertyPosts/{postId}',
            propertyPosts: '/api/Admin/PropertyPosts/{postId}',
            propertyPostsStatus: '/api/Admin/PropertyPosts/{postId}/status'
        },
        packages: {
            list: '/api/PromotionPackage/admin-list',
            create: '/api/PromotionPackage/create',
            update: '/api/PromotionPackage/update/{packageId}',
            delete: '/api/PromotionPackage/delete/{packageId}'
        },
        notifications: {
            list: '/api/Notification/admin-list',
            create: '/api/Notification/create',
            update: '/api/Notification/update/{notificationId}',
            delete: '/api/Notification/delete/{notificationId}'
        },
        news: {
            list: '/api/News/admin-list',
            create: '/api/News/create',
            update: '/api/News/update/{newsId}',
            delete: '/api/News/delete/{newsId}'
        },
        sliders: {
            list: '/api/Slider/admin-list',
            create: '/api/Slider/create',
            update: '/api/Slider/update/{sliderId}',
            delete: '/api/Slider/delete/{sliderId}'
        },
        amenities: {
            list: '/api/Amenity/admin-list',
            create: '/api/Amenity/create',
            update: '/api/Amenity/update/{amenityId}',
            delete: '/api/Amenity/delete/{amenityId}'
        },
        reports: {
            list: '/api/Report/admin-list',
            detail: '/api/Report/{reportId}',
            update: '/api/Report/admin-update/{reportId}'
        },
        support: {
            list: '/api/chat/admin/all-support-conversations',
            reply: '/api/chat/admin/support-conversation/{conversationId}/reply',
            updateStatus: '/api/chat/admin/support-conversation/{conversationId}/status'
        }
    },
    
    // Auth endpoints
    auth: {
        login: '/Auth/Login',
        register: '/Auth/Register'
    },
    
    // Helper function to build full API URL
    buildApiUrl: function(endpoint) {
        return this.apiBaseUrl + endpoint;
    },
    
    // Helper function to build relative URL
    buildRelativeUrl: function(endpoint) {
        return endpoint;
    }
};

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.config;
} 