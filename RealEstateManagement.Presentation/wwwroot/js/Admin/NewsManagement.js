const API_NEW_BASE_URL = "https://localhost:7031/api/News";
const API_IMAGE_BASE_URL = "https://localhost:7031/api/NewImage";

let currentPage = 1;
let pageSize = 10;
let totalPage = 1;
let currentSearch = '';
let newsList = [];

// Load danh sách bài báo
async function loadNewsList(page = 1, search = '') {
    currentPage = page;
    currentSearch = search;
    let url = `${API_NEW_BASE_URL}/All-News`;
    try {
        const res = await fetch(url);
        let data = await res.json();
        // Lọc theo tiêu đề nếu có search
        if (search) {
            data = data.filter(n => n.title.toLowerCase().includes(search.toLowerCase()));
        }
        newsList = data;
        totalPage = Math.ceil(data.length / pageSize) || 1;
        renderNewsTable();
        renderPagination();
    } catch (err) {
        renderErrorRow('Không thể tải dữ liệu');
    }
}

function renderNewsTable() {
    const tbody = document.getElementById('newsTableBody');
    tbody.innerHTML = '';
    if (!newsList.length) {
        tbody.innerHTML = `<tr><td colspan="5" class="text-center py-8 text-gray-400">Không có dữ liệu</td></tr>`;
        return;
    }
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    newsList.slice(start, end).forEach(news => {
        tbody.innerHTML += `
            <tr>
                <td class="px-4 py-2">${news.id}</td>
                <td class="px-4 py-2">${news.title}</td>
                <td class="px-4 py-2">${news.status === 1 ? '<span class=\'text-green-600\'>Đã xuất bản</span>' : '<span class=\'text-gray-500\'>Nháp</span>'}</td>
                <td class="px-4 py-2">${formatDate(news.createdAt)}</td>
                <td class="px-4 py-2 text-center">
                    <a class="text-blue-600 hover:underline mr-2" href="/Admin/CreateOrEditNews?id=${news.id}">Sửa</a>
                    <button class="text-red-600 hover:underline mr-2" onclick="deleteNews(${news.id})">Xóa</button>
                    ${news.status === 0 ? `<button class="text-green-600 hover:underline" onclick="publishNews(${news.id})">Xuất bản</button>` : ''}
                </td>
            </tr>
        `;
    });
}

function renderErrorRow(msg) {
    const tbody = document.getElementById('newsTableBody');
    tbody.innerHTML = `<tr><td colspan="5" class="text-center py-8 text-red-400">${msg}</td></tr>`;
}

function renderPagination() {
    const container = document.getElementById('paginationContainer');
    if (totalPage <= 1) { container.innerHTML = ''; return; }
    let html = '';
    html += `<button ${currentPage === 1 ? 'disabled' : ''} onclick="loadNewsList(${currentPage - 1}, '${currentSearch}')" class="px-3 py-1 border rounded mr-2">&lt;</button>`;
    for (let i = 1; i <= totalPage; i++) {
        html += `<button onclick="loadNewsList(${i}, '${currentSearch}')" class="px-3 py-1 border rounded ${i === currentPage ? 'bg-orange-500 text-white' : ''}">${i}</button>`;
    }
    html += `<button ${currentPage === totalPage ? 'disabled' : ''} onclick="loadNewsList(${currentPage + 1}, '${currentSearch}')" class="px-3 py-1 border rounded ml-2">&gt;</button>`;
    container.innerHTML = html;
}

// Tìm kiếm
const searchBtn = document.getElementById('btnSearch');
const searchInput = document.getElementById('searchTitle');
searchBtn.onclick = () => loadNewsList(1, searchInput.value);
searchInput.addEventListener('keydown', e => { if (e.key === 'Enter') loadNewsList(1, searchInput.value); });

// Xóa bài báo
async function deleteNews(id) {
    if (!id) return;
    const confirm = await Swal.fire({
        title: 'Xác nhận xóa',
        text: 'Bạn có chắc muốn xóa bài báo này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    });
    if (!confirm.isConfirmed) return;
    try {
        const res = await fetch(`${API_NEW_BASE_URL}/${id}`, { method: 'DELETE' });
        if (!res.ok) throw new Error('Xóa thất bại');
        Swal.fire('Thành công', 'Đã xóa bài báo', 'success');
        loadNewsList(currentPage, currentSearch);
    } catch (err) {
        Swal.fire('Lỗi', err.message, 'error');
    }
}

// Xuất bản bài báo
async function publishNews(id) {
    if (!id) return;
    const confirm = await Swal.fire({
        title: 'Xuất bản bài báo?',
        text: 'Bài báo sẽ được chuyển sang trạng thái Đã xuất bản',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Xuất bản',
        cancelButtonText: 'Hủy'
    });
    if (!confirm.isConfirmed) return;
    try {
        const res = await fetch(`${API_NEW_BASE_URL}/${id}/publish`, { method: 'PUT' });
        if (!res.ok) throw new Error('Xuất bản thất bại');
        Swal.fire('Thành công', 'Bài báo đã được xuất bản', 'success');
        loadNewsList(currentPage, currentSearch);
    } catch (err) {
        Swal.fire('Lỗi', err.message, 'error');
    }
}

// Format ngày
function formatDate(dateStr) {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    return d.toLocaleDateString('vi-VN') + ' ' + d.toLocaleTimeString('vi-VN');
}

// Khởi động
window.addEventListener('DOMContentLoaded', () => {
    loadNewsList();
}); 