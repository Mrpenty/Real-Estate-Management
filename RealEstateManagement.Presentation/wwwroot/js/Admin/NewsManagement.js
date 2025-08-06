const API_NEW_BASE_URL = "https://localhost:7031/api/News";
const API_IMAGE_BASE_URL = "https://localhost:7031/api/NewImage";

let currentPage = 1;
let pageSize = 10;
let totalPage = 1;
let totalItems = 0;
let currentSearch = '';
let newsList = [];

// Load danh sách bài báo
async function loadNewsList(page = 1, search = '') {
    currentPage = page;
    currentSearch = search;
    let url = `${API_NEW_BASE_URL}/All-News`;
    
    try {
        showLoadingState();
        
        const res = await fetch(url);
        if (!res.ok) throw new Error('Không thể tải dữ liệu');
        
        let data = await res.json();
        
        // Lọc theo tiêu đề nếu có search
        if (search) {
            data = data.filter(n => 
                n.title.toLowerCase().includes(search.toLowerCase()) ||
                (n.authorName && n.authorName.toLowerCase().includes(search.toLowerCase()))
            );
        }
        
        newsList = data;
        totalItems = data.length;
        totalPage = Math.ceil(totalItems / pageSize) || 1;
        
        updateStats(data);
        renderNewsTable();
        renderPagination();
        
    } catch (err) {
        console.error('Error loading news:', err);
        renderErrorRow('Không thể tải dữ liệu. Vui lòng thử lại sau.');
    }
}

// Cập nhật thống kê
function updateStats(data) {
    const total = data.length;
    const published = data.filter(n => n.isPublished).length;
    const draft = total - published;
    
    document.getElementById('totalNews').textContent = total;
    document.getElementById('publishedNews').textContent = published;
    document.getElementById('draftNews').textContent = draft;
}

// Hiển thị trạng thái loading
function showLoadingState() {
    const tbody = document.getElementById('newsTableBody');
    tbody.innerHTML = `
        <tr class="loading-state">
            <td colspan="6" class="text-center py-12">
                <div class="loading-spinner mx-auto"></div>
                <p>Đang tải dữ liệu...</p>
            </td>
        </tr>
    `;
    document.getElementById('paginationContainer').style.display = 'none';
}

// Render bảng tin tức
function renderNewsTable() {
    const tbody = document.getElementById('newsTableBody');
    tbody.innerHTML = '';
    
    if (!newsList.length) {
        tbody.innerHTML = `
            <tr class="empty-state">
                <td colspan="6" class="text-center py-12">
                    <div class="empty-state-icon">📰</div>
                    <h3 class="text-lg font-semibold text-gray-700 mb-2">Chưa có bài báo nào</h3>
                    <p class="text-gray-500 mb-4">Hãy tạo bài báo đầu tiên của bạn</p>
                    <a href="/Admin/CreateOrEditNews" class="action-btn btn-edit">
                        <i class="fas fa-plus mr-1"></i>
                        Tạo bài báo
                    </a>
                </td>
            </tr>
        `;
        document.getElementById('paginationContainer').style.display = 'none';
        return;
    }
    
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    const itemsToShow = newsList.slice(start, end);
    
    itemsToShow.forEach(news => {
        const row = document.createElement('tr');
        row.className = 'table-row';
        
        // Tính toán status
        const statusHtml = news.isPublished ? 
            '<span class="status-badge status-published"><i class="fas fa-check-circle"></i>Đã xuất bản</span>' :
            '<span class="status-badge status-draft"><i class="fas fa-clock"></i>Nháp</span>';
        
        // Tính toán ngày
        const dateStr = news.publishedAt ? 
            formatDate(news.publishedAt) : 
            formatDate(news.createdAt);
        
        // Tác giả
        const author = news.authorName || 'Không rõ';
        
        // Action buttons
        let actionsHtml = `
            <div class="flex flex-wrap justify-center gap-1">
                <a href="/Admin/NewsDetail?id=${news.id}" class="action-btn btn-view" title="Xem chi tiết">
                    <i class="fas fa-eye"></i>
                    
                </a>
                <a href="/Admin/CreateOrEditNews?id=${news.id}" class="action-btn btn-edit" title="Chỉnh sửa">
                    <i class="fas fa-edit"></i>
                    
                </a>
        `;
        
        if (!news.isPublished) {
            actionsHtml += `
                <button class="action-btn btn-publish" onclick="publishNews(${news.id})" title="Xuất bản">
                    <i class="fas fa-upload"></i>
                </button>
            `;
        }
        
        actionsHtml += `
                <button class="action-btn btn-delete" onclick="deleteNews(${news.id})" title="Xóa">
                    <i class="fas fa-trash"></i>
                    
                </button>
            </div>
        `;
        
        row.innerHTML = `
            <td class="px-6 py-4 text-sm font-medium text-gray-900">#${news.id}</td>
            <td class="px-6 py-4">
                <div class="max-w-xs">
                    <div class="font-medium text-gray-900 truncate" title="${news.title}">${news.title}</div>
                    ${news.summary ? `<div class="text-sm text-gray-500 truncate mt-1" title="${news.summary}">${news.summary}</div>` : ''}
                </div>
            </td>
            <td class="px-6 py-4 text-center">${statusHtml}</td>
            <td class="px-6 py-4 text-center text-sm text-gray-500">${dateStr}</td>
            <td class="px-6 py-4 text-center text-sm text-gray-500">${author}</td>
            <td class="px-6 py-4 text-center">${actionsHtml}</td>
        `;
        
        tbody.appendChild(row);
    });
    
    // Hiển thị pagination nếu cần
    if (totalPage > 1) {
        document.getElementById('paginationContainer').style.display = 'flex';
    } else {
        document.getElementById('paginationContainer').style.display = 'none';
    }
}

// Render lỗi
function renderErrorRow(msg) {
    const tbody = document.getElementById('newsTableBody');
    tbody.innerHTML = `
        <tr class="empty-state">
            <td colspan="6" class="text-center py-12">
                <div class="text-red-500 text-4xl mb-4">⚠️</div>
                <h3 class="text-lg font-semibold text-gray-700 mb-2">Có lỗi xảy ra</h3>
                <p class="text-gray-500 mb-4">${msg}</p>
                <button onclick="loadNewsList()" class="action-btn btn-edit">
                    <i class="fas fa-refresh mr-1"></i>
                    Thử lại
                </button>
            </td>
        </tr>
    `;
    document.getElementById('paginationContainer').style.display = 'none';
}

// Render phân trang
function renderPagination() {
    const container = document.getElementById('paginationButtons');
    const infoContainer = document.getElementById('paginationContainer');
    
    if (totalPage <= 1) {
        infoContainer.style.display = 'none';
        return;
    }
    
    // Cập nhật thông tin hiển thị
    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalItems);
    
    document.getElementById('showingFrom').textContent = start;
    document.getElementById('showingTo').textContent = end;
    document.getElementById('totalItems').textContent = totalItems;
    
    // Tạo buttons
    let html = '';
    
    // Previous button
    html += `
        <button 
            class="pagination-btn ${currentPage === 1 ? 'opacity-50 cursor-not-allowed' : ''}" 
            onclick="loadNewsList(${currentPage - 1}, '${currentSearch}')" 
            ${currentPage === 1 ? 'disabled' : ''}
            title="Trang trước"
        >
            <i class="fas fa-chevron-left"></i>
        </button>
    `;
    
    // Page numbers
    const maxVisiblePages = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(totalPage, startPage + maxVisiblePages - 1);
    
    if (endPage - startPage + 1 < maxVisiblePages) {
        startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }
    
    // First page if not visible
    if (startPage > 1) {
        html += `
            <button class="pagination-btn" onclick="loadNewsList(1, '${currentSearch}')">1</button>
        `;
        if (startPage > 2) {
            html += `<span class="pagination-btn" disabled>...</span>`;
        }
    }
    
    // Visible pages
    for (let i = startPage; i <= endPage; i++) {
        html += `
            <button 
                class="pagination-btn ${i === currentPage ? 'active' : ''}" 
                onclick="loadNewsList(${i}, '${currentSearch}')"
            >
                ${i}
            </button>
        `;
    }
    
    // Last page if not visible
    if (endPage < totalPage) {
        if (endPage < totalPage - 1) {
            html += `<span class="pagination-btn" disabled>...</span>`;
        }
        html += `
            <button class="pagination-btn" onclick="loadNewsList(${totalPage}, '${currentSearch}')">${totalPage}</button>
        `;
    }
    
    // Next button
    html += `
        <button 
            class="pagination-btn ${currentPage === totalPage ? 'opacity-50 cursor-not-allowed' : ''}" 
            onclick="loadNewsList(${currentPage + 1}, '${currentSearch}')" 
            ${currentPage === totalPage ? 'disabled' : ''}
            title="Trang sau"
        >
            <i class="fas fa-chevron-right"></i>
        </button>
    `;
    
    container.innerHTML = html;
    infoContainer.style.display = 'flex';
}

// Tìm kiếm
const searchBtn = document.getElementById('btnSearch');
const searchInput = document.getElementById('searchTitle');

searchBtn.onclick = () => {
    const searchTerm = searchInput.value.trim();
    loadNewsList(1, searchTerm);
};

searchInput.addEventListener('keydown', e => {
    if (e.key === 'Enter') {
        const searchTerm = searchInput.value.trim();
        loadNewsList(1, searchTerm);
    }
});

// Clear search khi input trống
searchInput.addEventListener('input', e => {
    if (e.target.value === '') {
        loadNewsList(1, '');
    }
});

// Xóa bài báo
async function deleteNews(id) {
    if (!id) return;
    
    const result = await Swal.fire({
        title: 'Xác nhận xóa',
        text: 'Bạn có chắc chắn muốn xóa bài báo này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ef4444',
        cancelButtonColor: '#6b7280',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy',
        showLoaderOnConfirm: true,
        preConfirm: async () => {
            try {
                const res = await fetch(`${API_NEW_BASE_URL}/${id}`, { method: 'DELETE' });
                if (!res.ok) throw new Error('Xóa thất bại');
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
            text: 'Bài báo đã được xóa',
            icon: 'success',
            timer: 2000,
            showConfirmButton: false
        });
        loadNewsList(currentPage, currentSearch);
    }
}

// Xuất bản bài báo
async function publishNews(id) {
    if (!id) return;
    
    const result = await Swal.fire({
        title: 'Xuất bản bài báo?',
        text: 'Bài báo sẽ được chuyển sang trạng thái "Đã xuất bản"',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#10b981',
        cancelButtonColor: '#6b7280',
        confirmButtonText: 'Xuất bản',
        cancelButtonText: 'Hủy',
        showLoaderOnConfirm: true,
        preConfirm: async () => {
            try {
                const res = await fetch(`${API_NEW_BASE_URL}/${id}/publish`, { method: 'PUT' });
                if (!res.ok) throw new Error('Xuất bản thất bại');
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
        loadNewsList(currentPage, currentSearch);
    }
}

// Format ngày
function formatDate(dateStr) {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    return d.toLocaleDateString('vi-VN');
}

// Khởi động
window.addEventListener('DOMContentLoaded', () => {
    loadNewsList();
    
    // Focus vào search input
    setTimeout(() => {
        searchInput.focus();
    }, 100);
});

// Handle page visibility change
document.addEventListener('visibilitychange', () => {
    if (!document.hidden) {
        // Reload data when user returns to tab
        loadNewsList(currentPage, currentSearch);
    }
}); 