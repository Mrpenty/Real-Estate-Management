// NewDetail Page JavaScript
class NewDetailManager {
    constructor() {
        this.urlBase = "https://localhost:7031";
        this.currentId = null;
        this.propertyService = window.propertyService;
        this.init();
    }

    init() {
        // Get ID from data attribute or global variable
        this.currentId = this.getNewsId();
        if (this.currentId) {
            this.loadProperty();
        } else {
            this.showErrorMessage('Không tìm thấy ID tin tức');
        }
    }

    getNewsId() {
        // Try to get ID from multiple sources
        return window.currentNewsId || 
               document.querySelector('[data-news-id]')?.dataset?.newsId ||
               this.extractIdFromUrl();
    }

    extractIdFromUrl() {
        const pathParts = window.location.pathname.split('/');
        const idIndex = pathParts.indexOf('NewDetail');
        if (idIndex !== -1 && pathParts[idIndex + 1]) {
            return pathParts[idIndex + 1];
        }
        return null;
    }

    async loadProperty() {
        try {
            this.showLoadingState();
            
            const prop = await this.propertyService.getNewPostDetail(this.currentId);
            console.log('Property loaded:', prop);
            
            this.updateUI(prop);
            await this.loadSidebarNews();
            
        } catch (error) {
            console.error('Error loading property:', error);
            this.showErrorMessage('Có lỗi xảy ra khi tải thông tin tin tức');
        } finally {
            this.hideLoadingState();
        }
    }

    updateUI(prop) {
        // Update title
        this.updateElement('.titleId', prop.title, 'Không có tiêu đề');
        
        // Update author and content
        this.updateElement('.areaId', prop.authorName, 'Không có tác giả');
        this.updateElement('.contentId', prop.content, 'Không có nội dung');
        this.updateElement('.summaryId', prop.summary, 'Không có tóm tắt');
        this.updateElement('.timeAgoId', this.timeAgo(prop.createdAt), 'Không xác định');
        
        // Handle favorite buttons
        this.handleFavoriteButtons(prop.isFavorite);
    }

    updateElement(selector, value, fallback) {
        const element = $(selector);
        if (element.length) {
            element.html(value || fallback);
        }
    }

    handleFavoriteButtons(isFavorite) {
        if (!isFavorite) {
            $('#addFavoriteDetail').removeClass('d-none');
        } else {
            $('#removeFavoriteDetail').removeClass('d-none');
        }
    }

    async loadSidebarNews() {
        try {
            const newsList = await this.propertyService.getNewPost();
            this.populateSidebarNews(newsList);
        } catch (error) {
            console.error('Error loading sidebar news:', error);
            $('#listNew').html('<div class="text-gray-500 text-center py-4">Không thể tải tin mới</div>');
        }
    }

    populateSidebarNews(newsList) {
        const container = $('#listNew');
        let html = '';
        
        newsList.forEach(item => {
            if (item.id != this.currentId) {
                const imageUrl = this.getImageUrl(item.images?.[0]?.imageUrl);
                
                html += `
                    <div class="news-item" onclick="window.location.href='/Home/NewDetail/${item.id}'">
                        <img src="${imageUrl}" onerror="newDetailManager.handleImageError(this)" alt="tin" class="news-item-image" />
                        <div class="news-item-content">
                            <div class="news-item-title">
                                ${item.title || 'Không có tiêu đề'}
                            </div>
                            <div class="news-item-time">${this.timeAgo(item.publishedAt) || 'Không xác định'}</div>
                        </div>
                    </div>`;
            }
        });
        
        container.html(html);
    }

    getImageUrl(imageUrl) {
        if (!imageUrl) return '/image/default-news.jpg';
        return imageUrl.includes('http') ? imageUrl : this.urlBase + imageUrl;
    }

    handleImageError(img) {
        img.onerror = null; // Prevent infinite loop
        img.src = '/image/default-news.jpg';
        img.classList.add('image-error');
    }

    showLoadingState() {
        // Add loading skeleton to main content
        $('.main-content').addClass('loading-skeleton');
    }

    hideLoadingState() {
        $('.main-content').removeClass('loading-skeleton');
    }

    showErrorMessage(message) {
        // You can implement a toast notification here
        console.error(message);
        
        // Show error in UI
        $('.main-content').html(`
            <div class="content-section">
                <div class="text-center py-8">
                    <div class="text-red-500 text-xl mb-2">⚠️</div>
                    <div class="text-gray-600">${message}</div>
                    <button onclick="location.reload()" class="mt-4 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
                        Thử lại
                    </button>
                </div>
            </div>
        `);
    }

    // Utility function for time formatting
    timeAgo(dateString) {
        if (!dateString) return '';
        
        const date = new Date(dateString);
        const now = new Date();
        const diffInSeconds = Math.floor((now - date) / 1000);
        
        if (diffInSeconds < 60) return 'Vừa xong';
        if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)} phút trước`;
        if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)} giờ trước`;
        if (diffInSeconds < 2592000) return `${Math.floor(diffInSeconds / 86400)} ngày trước`;
        if (diffInSeconds < 31536000) return `${Math.floor(diffInSeconds / 2592000)} tháng trước`;
        
        return `${Math.floor(diffInSeconds / 31536000)} năm trước`;
    }
}

// Initialize when DOM is ready
$(document).ready(function() {
    // Initialize the manager
    window.newDetailManager = new NewDetailManager();
}); 