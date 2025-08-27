const API_NEW_BASE_URL = "http://194.233.81.64:5000/api/News";
const API_IMAGES_BASE_URL = "http://194.233.81.64:5000/api/NewImage";

let currentNews = null;

// Lấy newsId từ ViewBag
function getNewsId() {
    const newsIdElement = document.querySelector('[data-news-id]');
    if (newsIdElement) {
        return parseInt(newsIdElement.getAttribute('data-news-id'));
    }
    
    // Fallback: lấy từ URL
    const urlParams = new URLSearchParams(window.location.search);
    const idFromUrl = urlParams.get('id');
    if (idFromUrl) {
        return parseInt(idFromUrl);
    }
    
    // Fallback: lấy từ URL path
    const pathParts = window.location.pathname.split('/');
    const idFromPath = pathParts[pathParts.length - 1];
    if (idFromPath && !isNaN(idFromPath)) {
        return parseInt(idFromPath);
    }
    
    // Fallback: lấy từ ViewBag nếu có
    if (window.ViewBag && window.ViewBag.NewsId) {
        return window.ViewBag.NewsId;
    }
    
    return null;
}

// Load chi tiết bài báo
async function loadNewsDetail() {
    const newsId = getNewsId();
    
    if (!newsId) {
        showError('Không tìm thấy ID bài báo');
        return;
    }
    
    try {
        showLoading();
        
        // Gọi API lấy thông tin bài báo
        const newsResponse = await fetch(`${API_NEW_BASE_URL}/${newsId}`);
        if (!newsResponse.ok) {
            throw new Error('Không tìm thấy bài báo');
        }
        
        const news = await newsResponse.json();
        currentNews = news;
        
        // Load ảnh
        let images = [];
        try {
            const imagesResponse = await fetch(`${API_IMAGES_BASE_URL}?newId=${newsId}`);
            if (imagesResponse.ok) {
                images = await imagesResponse.json();
            }
        } catch (err) {
            console.warn('Không thể tải ảnh:', err);
        }
        
        renderNewsDetail(news, images);
        showContent();
        
    } catch (error) {
        console.error('Error loading news:', error);
        showError(error.message);
    }
}

// Hiển thị chi tiết bài báo
function renderNewsDetail(news, images) {
    // Title
    document.getElementById('newsTitle').textContent = news.title;
    
    // Status
    const statusElement = document.getElementById('newsStatus');
    if (news.isPublished) {
        statusElement.innerHTML = '<span class="status-badge status-published"><i class="fas fa-check-circle mr-1"></i>Đã xuất bản</span>';
        document.getElementById('publishButton').style.display = 'none';
    } else {
        statusElement.innerHTML = '<span class="status-badge status-draft"><i class="fas fa-clock mr-1"></i>Nháp</span>';
        document.getElementById('publishButton').style.display = 'inline-flex';
    }
    
    // Date
    const dateText = news.publishedAt ? 
        `Xuất bản: ${formatDate(news.publishedAt)}` : 
        `Tạo: ${formatDate(news.createdAt)}`;
    document.getElementById('newsDate').innerHTML = `<i class="fas fa-calendar mr-1"></i>${dateText}`;
    
    // Author
    const authorText = news.authorName || 'Không rõ';
    document.getElementById('newsAuthor').innerHTML = `<i class="fas fa-user mr-1"></i>Tác giả: ${authorText}`;
    
    // Summary
    const summaryElement = document.getElementById('newsSummary');
    if (news.summary) {
        summaryElement.innerHTML = `<h4 class="font-semibold text-gray-800 mb-2">Tóm tắt:</h4><p class="text-gray-700">${news.summary}</p>`;
    } else {
        summaryElement.innerHTML = '<p class="text-gray-500 italic">Chưa có tóm tắt</p>';
    }
    
    // Content - cần fix URL ảnh trong content
    const contentWithFixedImages = fixImagesInContent(news.content);
    document.getElementById('newsContentBody').innerHTML = contentWithFixedImages || '<p class="text-gray-500 italic">Chưa có nội dung</p>';
    
    // Images
    if (images && images.length > 0) {
        renderImages(images);
        document.getElementById('imagesSection').style.display = 'block';
    }
    
    // Meta information
    document.getElementById('newsId').textContent = news.id;
    document.getElementById('newsSlug').textContent = news.slug || 'Chưa có';
    document.getElementById('newsCreatedAt').textContent = formatDateTime(news.createdAt);
    document.getElementById('newsUpdatedAt').textContent = news.updatedAt ? formatDateTime(news.updatedAt) : 'Chưa cập nhật';
    document.getElementById('newsPublishedAt').textContent = news.publishedAt ? formatDateTime(news.publishedAt) : 'Chưa xuất bản';
    document.getElementById('newsSource').textContent = news.source || 'Không rõ';
    
    // Edit button
    document.getElementById('editButton').href = `/Admin/CreateOrEditNews?id=${news.id}`;
}

// Hiển thị ảnh
function renderImages(images) {
    const gallery = document.getElementById('imageGallery');
    gallery.innerHTML = '';
    
    images.forEach(image => {
        const imageItem = document.createElement('div');
        imageItem.className = 'image-item';
        
        // Tạo URL đầy đủ cho ảnh
        const fullImageUrl = getFullImageUrl(image.imageUrl);
        
        imageItem.innerHTML = `
            <img src="${fullImageUrl}" alt="Ảnh bài viết" loading="lazy" onerror="handleImageError(this)">
            ${image.isPrimary ? '<div class="image-primary-badge">Ảnh chính</div>' : ''}
        `;
        
        gallery.appendChild(imageItem);
    });
}

// Helper function để tạo URL đầy đủ
function getFullImageUrl(imageUrl) {
    if (!imageUrl) return '';
    
    // Nếu đã là URL đầy đủ thì return luôn
    if (imageUrl.startsWith('http')) {
        return imageUrl;
    }
    
    // Nếu là relative path thì thêm base URL của API
    const API_BASE = "http://194.233.81.64:5000";
    return API_BASE + imageUrl;
}

// Handle lỗi khi load ảnh
function handleImageError(img) {
    img.style.display = 'none';
    const parent = img.parentElement;
    if (parent) {
        parent.innerHTML = `
            <div style="display: flex; align-items: center; justify-content: center; height: 150px; background: #f3f4f6; color: #6b7280; font-size: 14px;">
                <i class="fas fa-image mr-2"></i>
                Ảnh không tải được
            </div>
        `;
    }
}

// Fix URL ảnh trong content HTML
function fixImagesInContent(content) {
    if (!content) return content;
    
    // Tìm tất cả thẻ img và update src
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = content;
    
    const images = tempDiv.querySelectorAll('img');
    images.forEach(img => {
        const currentSrc = img.getAttribute('src');
        if (currentSrc && !currentSrc.startsWith('http')) {
            const fullUrl = getFullImageUrl(currentSrc);
            img.setAttribute('src', fullUrl);
            img.setAttribute('onerror', 'this.style.display="none"');
        }
    });
    
    return tempDiv.innerHTML;
}

// Xuất bản bài báo
async function publishNews() {
    if (!currentNews) return;
    
    const result = await Swal.fire({
        title: 'Xuất bản bài báo?',
        text: 'Bài báo sẽ được chuyển sang trạng thái "Đã xuất bản" và hiển thị công khai.',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#10b981',
        cancelButtonColor: '#6b7280',
        confirmButtonText: 'Xuất bản',
        cancelButtonText: 'Hủy',
        showLoaderOnConfirm: true,
        preConfirm: async () => {
            try {
                const response = await fetch(`${API_NEW_BASE_URL}/${currentNews.id}/publish`, {
                    method: 'PUT'
                });
                
                if (!response.ok) {
                    throw new Error('Xuất bản thất bại');
                }
                
                return true;
            } catch (error) {
                Swal.showValidationMessage(error.message);
                return false;
            }
        }
    });
    
    if (result.isConfirmed) {
        Swal.fire({
            title: 'Thành công!',
            text: 'Bài báo đã được xuất bản',
            icon: 'success',
            timer: 2000,
            showConfirmButton: false
        });
        
        // Reload trang để cập nhật trạng thái
        setTimeout(() => {
            window.location.reload();
        }, 1500);
    }
}

// Xóa bài báo
async function deleteNews() {
    if (!currentNews) return;
    
    const result = await Swal.fire({
        title: 'Xác nhận xóa',
        text: 'Bạn có chắc chắn muốn xóa bài báo này? Hành động này không thể hoàn tác.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ef4444',
        cancelButtonColor: '#6b7280',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy',
        showLoaderOnConfirm: true,
        preConfirm: async () => {
            try {
                const response = await fetch(`${API_NEW_BASE_URL}/${currentNews.id}`, {
                    method: 'DELETE'
                });
                
                if (!response.ok) {
                    throw new Error('Xóa thất bại');
                }
                
                return true;
            } catch (error) {
                Swal.showValidationMessage(error.message);
                return false;
            }
        }
    });
    
    if (result.isConfirmed) {
        Swal.fire({
            title: 'Đã xóa!',
            text: 'Bài báo đã được xóa thành công',
            icon: 'success',
            timer: 2000,
            showConfirmButton: false
        });
        
        // Chuyển về trang danh sách
        setTimeout(() => {
            window.location.href = '/Admin/NewsManagement';
        }, 1500);
    }
}

// Utility functions
function showLoading() {
    document.getElementById('loadingState').style.display = 'block';
    document.getElementById('errorState').style.display = 'none';
    document.getElementById('newsContent').style.display = 'none';
}

function showError(message) {
    document.getElementById('loadingState').style.display = 'none';
    document.getElementById('errorState').style.display = 'block';
    document.getElementById('newsContent').style.display = 'none';
    document.getElementById('errorMessage').textContent = message;
}

function showContent() {
    document.getElementById('loadingState').style.display = 'none';
    document.getElementById('errorState').style.display = 'none';
    document.getElementById('newsContent').style.display = 'block';
}

function formatDate(dateStr) {
    if (!dateStr) return '';
    const date = new Date(dateStr);
    return date.toLocaleDateString('vi-VN');
}

function formatDateTime(dateStr) {
    if (!dateStr) return '';
    const date = new Date(dateStr);
    return date.toLocaleDateString('vi-VN') + ' ' + date.toLocaleTimeString('vi-VN');
}

// Khởi động
window.addEventListener('DOMContentLoaded', () => {
    loadNewsDetail();
});

// Handle browser back/forward
window.addEventListener('popstate', () => {
    loadNewsDetail();
}); 