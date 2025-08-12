// Sử dụng config thay vì hardcode API endpoints
let newsList = [];
let currentPage = 1;
let pageSize = 10;
let filteredNews = [];

const NewsService = {
    async getAll() {
        const res = await fetch(config.buildApiUrl(config.admin.news.list), {
            method: 'GET',
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy danh sách tin tức');
        return await res.json();
    },

    async delete(id) {
        const res = await fetch(config.buildApiUrl(config.admin.news.delete.replace('{newsId}', id)), {
            method: 'DELETE',
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể xóa tin tức');
        return true;
    },

    async publish(id) {
        const res = await fetch(config.buildApiUrl(config.admin.news.publish.replace('{newsId}', id)), {
            method: 'PUT',
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể xuất bản tin tức');
        return true;
    }
};

async function loadNewsList() {
    try {
        showLoading(true);
        const response = await NewsService.getAll();
        newsList = response || [];
        filteredNews = [...newsList];
        updateStats(newsList);
        renderNewsTable();
    } catch (err) {
        showError(err.message);
    } finally {
        hideLoading();
    }
}

function updateStats(news) {
    const total = news.length;
    const published = news.filter(n => n.isPublished).length;
    const draft = news.filter(n => !n.isPublished).length;
    const totalImages = news.reduce((sum, n) => sum + (n.imageCount || 0), 0);
    
    document.getElementById('totalNews').textContent = total;
    document.getElementById('publishedNews').textContent = published;
    document.getElementById('draftNews').textContent = draft;
    //document.getElementById('totalImages').textContent = totalImages;
    document.getElementById('displayCount').textContent = filteredNews.length;
}

function renderNewsTable() {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const pageNews = filteredNews.slice(startIndex, endIndex);

    const tbody = document.getElementById('newsTableBody');

    if (filteredNews.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center py-8">
                    <div class="empty-state">
                        <i class="fas fa-newspaper"></i>
                        <p>Không có tin tức nào</p>
                    </div>
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = pageNews.map(news => `
        <tr class="table-row">
            <td class="font-medium">${news.id}</td>
            <td>
                <div class="font-medium text-gray-900">${news.title}</div>
                <div class="text-sm text-gray-500 max-w-xs truncate">${news.summary || 'Không có tóm tắt'}</div>
            </td>
            <td class="max-w-xs truncate" title="${news.summary || 'Không có tóm tắt'}">
                ${news.summary || 'Không có tóm tắt'}
            </td>
            <td>${news.authorName || 'N/A'}</td>
            <td>
                <span class="badge ${news.isPublished ? 'badge-success' : 'badge-warning'}">
                    ${news.isPublished ? 'Đã xuất bản' : 'Bản nháp'}
                </span>
            </td>
            
            <td>${formatDate(news.createdAt)}</td>
            <td class="text-center">
                <a href="/Admin/NewsDetail/${news.id}" class="btn btn-secondary btn-sm mr-2" title="Xem chi tiết">
                    <i class="fas fa-book"></i>
                </a>
                <button onclick="publishNews(${news.id})" class="btn btn-success btn-sm mr-2" title="${news.isPublished ? 'Hủy xuất bản' : 'Xuất bản'}" ${news.isPublished ? 'disabled' : ''}>
                    <i class="fas ${news.isPublished ? 'fa-eye-slash' : 'fa-eye'}"></i>
                </button>
                <button onclick="deleteNews(${news.id})" class="btn btn-danger btn-sm" title="Xóa">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');

    updatePagination();
}

function updatePagination() {
    const totalPages = Math.ceil(filteredNews.length / pageSize);
    const startIndex = (currentPage - 1) * pageSize + 1;
    const endIndex = Math.min(currentPage * pageSize, filteredNews.length);

    document.getElementById('startIndex').textContent = startIndex;
    document.getElementById('endIndex').textContent = endIndex;
    document.getElementById('totalItems').textContent = filteredNews.length;

    const buttonsContainer = document.getElementById('paginationButtons');
    buttonsContainer.innerHTML = '';

    // Previous button
    if (currentPage > 1) {
        buttonsContainer.innerHTML += `
            <button onclick="goToPage(${currentPage - 1})" class="btn btn-secondary btn-sm">
                <i class="fas fa-chevron-left"></i>
            </button>
        `;
    }

    // Page numbers
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
        buttonsContainer.innerHTML += `
            <button onclick="goToPage(${i})" class="btn ${i === currentPage ? 'btn-primary' : 'btn-secondary'} btn-sm">
                ${i}
            </button>
        `;
    }

    // Next button
    if (currentPage < totalPages) {
        buttonsContainer.innerHTML += `
            <button onclick="goToPage(${currentPage + 1})" class="btn btn-secondary btn-sm">
                <i class="fas fa-chevron-right"></i>
            </button>
        `;
    }

    if (totalPages > 0) {
        document.getElementById('paginationBar').style.display = 'block';
    }
}

function goToPage(page) {
    currentPage = page;
    renderNewsTable();
}

function applyFilters() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const statusFilter = document.getElementById('statusFilter').value;
    const sortFilter = document.getElementById('sortFilter').value;

    filteredNews = newsList.filter(news => {
        const matchesSearch = news.title.toLowerCase().includes(searchTerm) ||
                             (news.author && news.author.toLowerCase().includes(searchTerm)) ||
                             (news.summary && news.summary.toLowerCase().includes(searchTerm));
        const matchesStatus = statusFilter === '' || 
                            (statusFilter === 'published' && news.isPublished) ||
                            (statusFilter === 'draft' && !news.isPublished);

        return matchesSearch && matchesStatus;
    });

    // Sort
    switch (sortFilter) {
        case 'newest':
            filteredNews.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
            break;
        case 'oldest':
            filteredNews.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
            break;
        case 'title':
            filteredNews.sort((a, b) => a.title.localeCompare(b.title));
            break;
    }

    currentPage = 1;
    updateStats(filteredNews);
    renderNewsTable();
}

function refreshNews() {
    loadNewsList();
}

async function deleteNews(id) {
    const result = await Swal.fire({
        title: 'Xác nhận xóa',
        text: 'Bạn có chắc chắn muốn xóa tin tức này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Có, xóa!',
        cancelButtonText: 'Hủy'
    });

    if (result.isConfirmed) {
        try {
            await NewsService.delete(id);
            showSuccess('Xóa tin tức thành công!');
            loadNewsList();
        } catch (err) {
            showError(err.message);
        }
    }
}

async function publishNews(id) {
    const news = newsList.find(n => n.id === id);
    const action = news.isPublished ? 'hủy xuất bản' : 'xuất bản';
    
    const result = await Swal.fire({
        title: `Xác nhận ${action}`,
        text: `Bạn có chắc chắn muốn ${action} tin tức này?`,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Có, ${action}!`,
        cancelButtonText: 'Hủy'
    });

    if (result.isConfirmed) {
        try {
            await NewsService.publish(id);
            showSuccess(`${action.charAt(0).toUpperCase() + action.slice(1)} tin tức thành công!`);
            loadNewsList();
        } catch (err) {
            showError(err.message);
        }
    }
}

function showLoading(show) {
    const tbody = document.getElementById('newsTableBody');
    if (show) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center py-8">
                    <div class="loading-spinner"></div>
                    <p class="mt-2 text-gray-600">Đang tải dữ liệu...</p>
                </td>
            </tr>
        `;
    }
}

function hideLoading() {
    // Loading is handled by renderNewsTable
}

function showError(message) {
    Swal.fire({
        icon: 'error',
        title: 'Lỗi',
        text: message,
        confirmButtonText: 'Đóng'
    });
}

function showSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Thành công',
        text: message,
        confirmButtonText: 'Đóng'
    });
}

function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

// Event Listeners
document.addEventListener('DOMContentLoaded', function() {
    loadNewsList();

    // Add event listeners for real-time search
    document.getElementById('searchInput').addEventListener('input', applyFilters);
    document.getElementById('statusFilter').addEventListener('change', applyFilters);
    document.getElementById('sortFilter').addEventListener('change', applyFilters);
}); 