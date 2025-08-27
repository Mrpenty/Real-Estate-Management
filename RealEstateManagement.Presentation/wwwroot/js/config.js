window.config = {
    // API Base URL
    apiBaseUrl: 'https://localhost:7031',
    //apiBaseUrl: 'http://194.233.81.64:5000',
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
            list: '/api/PromotionPackage/GetAllPackage',
            detail: '/api/PromotionPackage/GetPackageById/{packageId}',
            create: '/api/PromotionPackage/CreatePackage',
            update: '/api/PromotionPackage/UpdatePackage/{packageId}',
            delete: '/api/PromotionPackage/DeletePackage/{packageId}'
        },
        notifications: {
            list: '/api/Notification/all',
            detail: '/api/Notification/{notificationId}',
            create: '/api/Notification/create',
            update: '/api/Notification/update',
            delete: '/api/Notification/delete/{notificationId}'
        },
        news: {
            list: '/api/News/All-News',
            published: '/api/News/published',
            detail: '/api/News/{newsId}',
            bySlug: '/api/News/slug/{slug}',
            create: '/api/News',
            update: '/api/News/{newsId}',
            publish: '/api/News/{newsId}/publish',
            delete: '/api/News/{newsId}',
            images: {
                list: '/api/NewImage?newId={newsId}',
                upload: '/api/NewImage/upload?newId={newsId}',
                create: '/api/NewImage?newId={newsId}',
                update: '/api/NewImage/{imageId}?newId={newsId}',
                delete: '/api/NewImage/{imageId}?newId={newsId}',
                deleteFile: '/api/NewImage/delete-file'
            }
        },
        sliders: {
            list: '/api/Slider',
            detail: '/api/Slider/{sliderId}',
            create: '/api/Slider',
            update: '/api/Slider/{sliderId}',
            delete: '/api/Slider/{sliderId}'
        },
        amenities: {
            list: '/api/Amenity',
            detail: '/api/Amenity/{amenityId}',
            create: '/api/Amenity',
            update: '/api/Amenity/{amenityId}',
            delete: '/api/Amenity/{amenityId}'
        },
        reports: {
            list: '/api/Report/admin/all',
            detail: '/api/Report/{reportId}',
            resolve: '/api/Report/{reportId}/resolve',
            reject: '/api/Report/{reportId}/reject'
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
    
    // AI Recommendation endpoints
    ai: {
        searchByCriteria: '/api/AIRecommendation/search-by-criteria',
        locationBased: '/api/AIRecommendation/location-based',
        nearby: '/api/AIRecommendation/nearby',
        nearbyAmenities: '/api/AIRecommendation/nearby-amenities',
        transportationInfo: '/api/AIRecommendation/transportation-info',
        calculateDistance: '/api/AIRecommendation/calculate-distance'
    },
    
    // Property endpoints
    property: {
        types: '/api/PropertyType/GetAllPropertyTypes'
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