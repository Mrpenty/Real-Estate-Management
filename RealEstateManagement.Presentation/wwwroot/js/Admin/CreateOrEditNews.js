const API_New_BASE_URL = "https://localhost:7031/api/News";
const API_IMAGES_BASE_URL = "https://localhost:7031/api/NewImage";

const newsId = window.ViewBag && window.ViewBag.NewsId ? window.ViewBag.NewsId : null;
const form = document.getElementById('newsForm');
const imageInput = document.getElementById('newsImage');
const imagePreview = document.getElementById('imagePreview');



// Nếu có id, load chi tiết bài báo
window.addEventListener('DOMContentLoaded', async () => {
    const id = getNewsId();
    if (id) {
        document.getElementById('formTitle').textContent = 'Sửa bài báo';
        try {
            const res = await fetch(`${API_New_BASE_URL}/${id}`);
            if (!res.ok) throw new Error('Không tìm thấy bài báo');
            const news = await res.json();
            document.getElementById('newsId').value = news.id;
            document.getElementById('title').value = news.title;
            document.getElementById('summary').value = news.summary || '';
            document.getElementById('content').value = news.content;
            document.getElementById('authorName').value = news.authorName || '';
            document.getElementById('source').value = news.source || '';
            document.getElementById('status').value = news.status;
            if (news.images && news.images.length > 0) {
                imagePreview.innerHTML = `<img src="${news.images[0].imageUrl}" class="h-24 rounded">`;
            }
        } catch (err) {
            Swal.fire('Lỗi', err.message, 'error');
        }
    } else {
        document.getElementById('formTitle').textContent = 'Thêm bài báo';
    }
});

// Preview ảnh
imageInput.onchange = function () {
    const file = this.files[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = function (e) {
        imagePreview.innerHTML = `<img src="${e.target.result}" class="h-24 rounded">`;
    };
    reader.readAsDataURL(file);
};

// Submit form
form.onsubmit = async function (e) {
    e.preventDefault();
    const id = document.getElementById('newsId').value;
    const title = document.getElementById('title').value.trim();
    const summary = document.getElementById('summary').value.trim();
    const content = document.getElementById('content').value.trim();
    const authorName = document.getElementById('authorName').value.trim();
    const source = document.getElementById('source').value.trim();
    const status = parseInt(document.getElementById('status').value);
    const file = imageInput.files[0];
    if (!title || !content) {
        Swal.fire('Lỗi', 'Vui lòng nhập đầy đủ tiêu đề và nội dung', 'error');
        return;
    }
    let imageUrl = '';
    if (file) {
        // Upload ảnh trước
        const formData = new FormData();
        formData.append('file', file);
        const uploadRes = await fetch(`${API_IMAGES_BASE_URL}/upload?newId=${id || 0}`, {
            method: 'POST',
            body: formData
        });
        if (!uploadRes.ok) {
            Swal.fire('Lỗi', 'Upload ảnh thất bại', 'error');
            return;
        }
        const uploadData = await uploadRes.json();
        imageUrl = uploadData.imageUrl;
    }
    // Tạo hoặc cập nhật bài báo
    const dto = { id: id ? parseInt(id) : undefined, title, summary, content, authorName, source, status };
    try {
        let res;
        if (!id) {
            res = await fetch(API_New_BASE_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dto)
            });
        } else {
            res = await fetch(`${API_New_BASE_URL}/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ ...dto, id: parseInt(id) })
            });
        }
        if (!res.ok) throw new Error('Lưu bài báo thất bại');
        let newsId = id;
        if (!id) {
            const data = await res.json();
            newsId = data.id;
        }
        // Nếu có ảnh thì lưu vào DB
        if (imageUrl) {
            await fetch(`${API_IMAGES_BASE_URL}?newId=${newsId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ imageUrl, isPrimary: true, order: 0 })
            });
        }
        Swal.fire('Thành công', 'Lưu bài báo thành công', 'success').then(() => {
            window.location.href = '/Admin/NewsManagement';
        });
    } catch (err) {
        Swal.fire('Lỗi', err.message, 'error');
    }
};

function getNewsId() {
    // Ưu tiên lấy từ ViewBag nếu render server, fallback query string
    const id = document.getElementById('newsId').value;
    if (id) return id;
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
} 