const API_New_BASE_URL = "https://localhost:7031/api/News";
const API_IMAGES_BASE_URL = "https://localhost:7031/api/NewImage";

const newsId = window.ViewBag && window.ViewBag.NewsId ? window.ViewBag.NewsId : null;
const form = document.getElementById('newsForm');
const imageInput = document.getElementById('newsImage');
const primaryImagePreview = document.getElementById('primaryImagePreview');
const contentEditor = document.getElementById('content');
const contentImagePreview = document.getElementById('contentImagePreview');

// Biến lưu trữ vị trí con trỏ và file được chọn
let savedRange = null;
let selectedContentFile = null;

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
            // Load nội dung và chèn ảnh từ imageUrl
            contentEditor.innerHTML = await loadContentWithImages(news.id, news.content);
            document.getElementById('authorName').value = news.authorName || '';
            document.getElementById('source').value = news.source || '';
            document.getElementById('status').value = news.status;
            
            // Hiển thị ảnh primary nếu có
            const primaryImage = news.images && news.images.find(img => img.isPrimary);
            if (primaryImage) {
                const fullImageUrl = getFullImageUrl(primaryImage.imageUrl);
                showPrimaryImagePreview(fullImageUrl);
            }
        } catch (err) {
            Swal.fire('Lỗi', err.message, 'error');
        }
    } else {
        document.getElementById('formTitle').textContent = 'Thêm bài báo';
    }
});

// Hàm tải nội dung và chèn ảnh từ imageUrl
async function loadContentWithImages(newId, content) {
    if (!content) return '';
    try {
        const res = await fetch(`${API_IMAGES_BASE_URL}?newId=${newId}`);
        if (!res.ok) throw new Error('Không tải được danh sách ảnh');
        const images = await res.json();
        let htmlContent = content;
        images.forEach(image => {
            if (!image.isPrimary) { // Chỉ chèn ảnh không phải primary vào content
                const fullImageUrl = getFullImageUrl(image.imageUrl);
                htmlContent = htmlContent.replace(`[image:${image.id}]`, `<img src="${fullImageUrl}" style="max-width: 100%; height: auto;" />`);
            }
        });
        
        // Fix tất cả ảnh trong content
        return fixImagesInContent(htmlContent);
    } catch (err) {
        console.error('Lỗi tải ảnh:', err);
        return fixImagesInContent(content);
    }
}

// Helper function để tạo URL đầy đủ
function getFullImageUrl(imageUrl) {
    if (!imageUrl) return '';
    
    // Nếu đã là URL đầy đủ thì return luôn
    if (imageUrl.startsWith('http')) {
        return imageUrl;
}

    // Nếu là relative path thì thêm base URL của API
    const API_BASE = "https://localhost:7031";
    return API_BASE + imageUrl;
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

// Preview ảnh đại diện (Primary)
function previewPrimaryImage() {
    const file = imageInput.files[0];
    if (!file) return;
    
    const reader = new FileReader();
    reader.onload = function (e) {
        showPrimaryImagePreview(e.target.result);
    };
    reader.readAsDataURL(file);
}

function showPrimaryImagePreview(imageSrc) {
    const container = document.getElementById('primaryImageContainer');
    container.innerHTML = `<img src="${imageSrc}" alt="Ảnh đại diện" />`;
    primaryImagePreview.style.display = 'block';
}

function cancelPrimaryImagePreview() {
    primaryImagePreview.style.display = 'none';
    imageInput.value = '';
}

// Preview ảnh content
function previewContentImage() {
    const fileInput = document.getElementById('contentImage');
    const file = fileInput.files[0];
    if (!file) return;

    // Lưu file được chọn
    selectedContentFile = file;
    
    // Lưu vị trí con trỏ hiện tại
    saveSelection();
    
    // Hiển thị preview
    const reader = new FileReader();
    reader.onload = function (e) {
        const container = document.getElementById('contentImageContainer');
        container.innerHTML = `<img src="${e.target.result}" alt="Preview ảnh content" />`;
        contentImagePreview.classList.add('show');
    };
    reader.readAsDataURL(file);
}

function cancelContentImagePreview() {
    contentImagePreview.classList.remove('show');
    selectedContentFile = null;
    document.getElementById('contentImage').value = '';
}

// Lưu vị trí con trỏ
function saveSelection() {
    const selection = window.getSelection();
    if (selection.rangeCount > 0) {
        savedRange = selection.getRangeAt(0).cloneRange();
}
}

// Khôi phục vị trí con trỏ
function restoreSelection() {
    if (savedRange) {
        const selection = window.getSelection();
        selection.removeAllRanges();
        selection.addRange(savedRange);
    }
}

// Chèn ảnh vào vị trí con trỏ
async function insertImageAtCursor() {
    if (!selectedContentFile) return;

    let newId = getNewsId();
    if (!newId) {
        // Nếu chưa có newId, tạo bài báo trước
        const title = document.getElementById('title').value.trim() || 'Tạm thời';
        const dto = { title, summary: '', content: '', authorName: '', source: '', status: 0 };
        try {
            const res = await fetch(API_New_BASE_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dto)
            });
            if (!res.ok) throw new Error('Tạo bài báo tạm thất bại');
            const data = await res.json();
            newId = data.id;
            document.getElementById('newsId').value = newId;
        } catch (err) {
            Swal.fire('Lỗi', 'Tạo bài báo tạm thất bại: ' + err.message, 'error');
            return;
        }
    }

    try {
        // Upload file
        const formData = new FormData();
        formData.append('file', selectedContentFile);
        const uploadRes = await fetch(`${API_IMAGES_BASE_URL}/upload?newId=${newId}`, {
            method: 'POST',
            body: formData
        });
        if (!uploadRes.ok) throw new Error('Upload ảnh thất bại: ' + uploadRes.statusText);
        const uploadData = await uploadRes.json();
        const imageUrl = uploadData.imageUrl;

        // Lưu thông tin ảnh vào API (không phải primary)
        await fetch(`${API_IMAGES_BASE_URL}?newId=${newId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ imageUrl, isPrimary: false, order: 0 })
        });

        // Khôi phục vị trí con trỏ và chèn ảnh
        restoreSelection();
        
        const img = document.createElement('img');
        img.src = imageUrl;
        img.style.maxWidth = '100%';
        img.style.height = 'auto';
        img.style.margin = '10px 0';

        const selection = window.getSelection();
        if (selection.rangeCount > 0) {
            const range = selection.getRangeAt(0);
            range.deleteContents();
            range.insertNode(img);
            range.collapse(false);
            selection.removeAllRanges();
            selection.addRange(range);
        } else {
            contentEditor.appendChild(img);
        }

        // Ẩn preview và reset
        cancelContentImagePreview();
        
        Swal.fire({
            icon: 'success',
            title: 'Thành công',
            text: 'Ảnh đã được chèn vào nội dung',
            timer: 1500,
            showConfirmButton: false
        });
    } catch (err) {
        Swal.fire('Lỗi', 'Upload ảnh thất bại: ' + err.message, 'error');
    }
}

// Lưu vị trí con trỏ khi click vào editor
contentEditor.addEventListener('mouseup', saveSelection);
contentEditor.addEventListener('keyup', saveSelection);

// Chức năng định dạng văn bản
function formatText(command) {
    document.execCommand(command, false, null);
    contentEditor.focus();
}

function changeFontSize(size) {
    if (size) {
        document.execCommand('fontSize', false, '7');
        const selection = window.getSelection();
        if (selection.rangeCount) {
            const range = selection.getRangeAt(0);
            const span = document.createElement('span');
            span.style.fontSize = size;
            try {
                range.surroundContents(span);
            } catch (e) {
                // Nếu không thể surround, insert text với style
                const textNode = document.createTextNode(selection.toString());
                span.appendChild(textNode);
                range.deleteContents();
                range.insertNode(span);
            }
        }
        contentEditor.focus();
    }
}

// Submit form
form.onsubmit = async function (e) {
    e.preventDefault();
    const id = document.getElementById('newsId').value;
    const title = document.getElementById('title').value.trim();
    const summary = document.getElementById('summary').value.trim();
    const content = contentEditor.innerHTML.trim();
    const authorName = document.getElementById('authorName').value.trim();
    const source = document.getElementById('source').value.trim();
    const status = parseInt(document.getElementById('status').value);
    const file = imageInput.files[0];

    if (!title || !content) {
        Swal.fire('Lỗi', 'Vui lòng nhập đầy đủ tiêu đề và nội dung', 'error');
        return;
    }

    let imageUrl = '';
    let newsId = id;

    // Nếu chưa có id, tạo bài báo trước
    if (!id) {
        const dto = { title, summary, content, authorName, source, status: 0 };
    try {
            const res = await fetch(API_New_BASE_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dto)
            });
            if (!res.ok) throw new Error('Tạo bài báo thất bại');
            const data = await res.json();
            newsId = data.id;
        } catch (err) {
            Swal.fire('Lỗi', err.message, 'error');
            return;
        }
    }

    // Upload ảnh đại diện nếu có
    if (file) {
        try {
            const formData = new FormData();
            formData.append('file', file);
            const uploadRes = await fetch(`${API_IMAGES_BASE_URL}/upload?newId=${newsId}`, {
                method: 'POST',
                body: formData
            });
            if (!uploadRes.ok) throw new Error('Upload ảnh đại diện thất bại');
            const uploadData = await uploadRes.json();
            imageUrl = uploadData.imageUrl;

            // Lưu ảnh đại diện với isPrimary = true
            await fetch(`${API_IMAGES_BASE_URL}?newId=${newsId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ imageUrl, isPrimary: true, order: 0 })
            });
        } catch (err) {
            Swal.fire('Lỗi', err.message, 'error');
            return;
        }
    }

    // Cập nhật bài báo
    const updateDto = { id: parseInt(newsId), title, summary, content, authorName, source, isPublished: status === 1 };
    try {
        const res = await fetch(`${API_New_BASE_URL}/${newsId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updateDto)
        });
        if (!res.ok) throw new Error('Cập nhật bài báo thất bại');

        Swal.fire('Thành công', 'Lưu bài báo thành công', 'success').then(() => {
            window.location.href = '/Admin/NewsManagement';
        });
    } catch (err) {
        Swal.fire('Lỗi', err.message, 'error');
    }
};

function getNewsId() {
    const id = document.getElementById('newsId').value;
    if (id) return id;
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}