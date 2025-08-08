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
                const container = document.getElementById('primaryImageContainer');
                container.innerHTML = `
                    <div class="flex items-center gap-4">
                        <img src="${fullImageUrl}" alt="Ảnh đại diện" class="max-w-xs rounded" />
                        <div class="text-sm text-gray-600">
                            <p><strong>Ảnh hiện tại:</strong> ${primaryImage.imageUrl.split('/').pop()}</p>
                            <p class="text-orange-600 font-semibold">🌟 Ảnh đại diện hiện tại</p>
                        </div>
                    </div>
                `;
                primaryImagePreview.style.display = 'block';
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
        
        // Khởi tạo danh sách ảnh hiện tại
        currentImagesInContent.clear();
        
        images.forEach(image => {
            if (!image.isPrimary) { // Chỉ chèn ảnh không phải primary vào content
                const fullImageUrl = getFullImageUrl(image.imageUrl);
                const imgTag = `<img src="${fullImageUrl}" style="max-width: 100%; height: auto; border-radius: 4px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);" data-image-url="${image.imageUrl}" />`;
                htmlContent = htmlContent.replace(`[image:${image.id}]`, imgTag);
                currentImagesInContent.add(fullImageUrl);
            }
        });
        
        // Fix tất cả ảnh trong content
        const fixedContent = fixImagesInContent(htmlContent);
        
        // Cập nhật danh sách ảnh hiện tại sau khi fix
        setTimeout(() => {
            const images = contentEditor.querySelectorAll('img');
            currentImagesInContent.clear();
            images.forEach(img => {
                const src = img.getAttribute('src');
                const dataUrl = img.getAttribute('data-image-url');
                
                if (src && (src.includes('/uploads/') || src.startsWith('http'))) {
                    currentImagesInContent.add(src);
                }
                if (dataUrl && dataUrl.includes('/uploads/')) {
                    currentImagesInContent.add(dataUrl);
                }
            });
            console.log('Danh sách ảnh hiện tại:', Array.from(currentImagesInContent));
        }, 100);
        
        return fixedContent;
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

    // Kiểm tra kích thước file (tối đa 5MB)
    if (file.size > 5 * 1024 * 1024) {
        Swal.fire('Lỗi', 'Kích thước file quá lớn. Tối đa 5MB.', 'error');
        imageInput.value = '';
        return;
    }

    // Kiểm tra định dạng file
    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
    if (!allowedTypes.includes(file.type)) {
        Swal.fire('Lỗi', 'Chỉ chấp nhận file ảnh (JPG, PNG, GIF).', 'error');
        imageInput.value = '';
        return;
    }
    
    const reader = new FileReader();
    reader.onload = function (e) {
        showPrimaryImagePreview(e.target.result, file);
    };
    reader.readAsDataURL(file);
}

function showPrimaryImagePreview(imageSrc, file) {
    const container = document.getElementById('primaryImageContainer');
    container.innerHTML = `
        <div class="flex items-center gap-4">
            <img src="${imageSrc}" alt="Ảnh đại diện" class="max-w-xs rounded" />
            <div class="text-sm text-gray-600">
                <p><strong>Tên file:</strong> ${file.name}</p>
                <p><strong>Kích thước:</strong> ${(file.size / 1024 / 1024).toFixed(2)} MB</p>
                <p><strong>Định dạng:</strong> ${file.type}</p>
                <p class="text-orange-600 font-semibold">🌟 Ảnh này sẽ được đánh dấu làm ảnh chính</p>
            </div>
        </div>
    `;
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

    // Kiểm tra kích thước file (tối đa 5MB)
    if (file.size > 5 * 1024 * 1024) {
        Swal.fire('Lỗi', 'Kích thước file quá lớn. Tối đa 5MB.', 'error');
        return;
    }

    // Kiểm tra định dạng file
    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
    if (!allowedTypes.includes(file.type)) {
        Swal.fire('Lỗi', 'Chỉ chấp nhận file ảnh (JPG, PNG, GIF).', 'error');
        return;
    }

    // Lưu file được chọn
    selectedContentFile = file;
    
    // Lưu vị trí con trỏ hiện tại
    saveSelection();
    
    // Hiển thị preview
    const reader = new FileReader();
    reader.onload = function (e) {
        const container = document.getElementById('contentImageContainer');
        container.innerHTML = `
            <div class="flex items-center gap-4">
                <img src="${e.target.result}" alt="Preview ảnh content" class="max-w-xs rounded" />
                <div class="text-sm text-gray-600">
                    <p><strong>Tên file:</strong> ${file.name}</p>
                    <p><strong>Kích thước:</strong> ${(file.size / 1024 / 1024).toFixed(2)} MB</p>
                    <p><strong>Định dạng:</strong> ${file.type}</p>
                </div>
            </div>
        `;
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
        // Sử dụng URL đầy đủ để đảm bảo ảnh hiển thị
        img.src = getFullImageUrl(imageUrl);
        img.style.maxWidth = '100%';
        img.style.height = 'auto';
        img.style.margin = '10px 0';
        img.style.borderRadius = '4px';
        img.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)';
        img.setAttribute('data-image-url', imageUrl); // Lưu URL gốc để theo dõi

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

        // Thêm ảnh mới vào danh sách theo dõi
        currentImagesInContent.add(getFullImageUrl(imageUrl));

        // Ẩn preview và reset
        cancelContentImagePreview();
        
        // Focus lại vào editor để user có thể tiếp tục viết
        contentEditor.focus();
        
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

// Theo dõi khi user xóa ảnh trong content editor
contentEditor.addEventListener('input', function() {
    // Kiểm tra xem có ảnh nào bị xóa không
    setTimeout(checkForDeletedImages, 50);
});

// Theo dõi khi user paste hoặc delete
contentEditor.addEventListener('paste', function() {
    setTimeout(checkForDeletedImages, 100);
});

contentEditor.addEventListener('keydown', function(e) {
    if (e.key === 'Delete' || e.key === 'Backspace') {
        setTimeout(checkForDeletedImages, 50);
    }
    // Theo dõi khi user undo/redo
    if ((e.ctrlKey || e.metaKey) && (e.key === 'z' || e.key === 'y')) {
        setTimeout(checkForDeletedImages, 100);
    }
});

// Theo dõi khi user cut
contentEditor.addEventListener('cut', function() {
    setTimeout(checkForDeletedImages, 50);
});

// Biến lưu trữ danh sách ảnh hiện tại trong content
let currentImagesInContent = new Set();

// Hàm kiểm tra ảnh bị xóa
function checkForDeletedImages() {
    const images = contentEditor.querySelectorAll('img');
    const currentImageUrls = new Set();
    
    images.forEach(img => {
        const src = img.getAttribute('src');
        const dataUrl = img.getAttribute('data-image-url');
        
        // Sử dụng cả src và data-image-url để theo dõi
        if (src && (src.includes('/uploads/') || src.startsWith('http'))) {
            currentImageUrls.add(src);
        }
        if (dataUrl && dataUrl.includes('/uploads/')) {
            currentImageUrls.add(dataUrl);
        }
    });
    
    // Tìm ảnh đã bị xóa
    const deletedImages = Array.from(currentImagesInContent).filter(url => !currentImageUrls.has(url));
    
    // Xóa ảnh khỏi database nếu có
    deletedImages.forEach(imageUrl => {
        console.log('Phát hiện ảnh bị xóa:', imageUrl);
        deleteImageFromDatabase(imageUrl);
    });
    
    // Cập nhật danh sách ảnh hiện tại
    currentImagesInContent = currentImageUrls;
}

// Hàm xóa ảnh khỏi database
async function deleteImageFromDatabase(imageUrl) {
    const newsId = getNewsId();
    if (!newsId) return;
    
    try {
        // Lấy danh sách ảnh để tìm imageId
        const res = await fetch(`${API_IMAGES_BASE_URL}?newId=${newsId}`);
        if (!res.ok) return;
        
        const images = await res.json();
        // Tìm ảnh bằng cách so sánh URL (cả relative và full URL)
        const image = images.find(img => {
            const fullUrl = getFullImageUrl(img.imageUrl);
            return fullUrl === imageUrl || img.imageUrl === imageUrl;
        });
        
        if (image && !image.isPrimary) {
            // Xóa ảnh khỏi database
            const deleteRes = await fetch(`${API_IMAGES_BASE_URL}/${newsId}/${image.id}`, {
                method: 'DELETE',
                credentials: 'include'
            });
            
            if (deleteRes.ok) {
                console.log(`Đã xóa ảnh: ${imageUrl}`);
                // Xóa file vật lý nếu có thể
                try {
                    await fetch(`${API_IMAGES_BASE_URL}/delete-file`, {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ imageUrl: image.imageUrl }),
                        credentials: 'include'
                    });
                } catch (fileErr) {
                    console.log('Không thể xóa file vật lý:', fileErr);
                }
            }
        }
    } catch (err) {
        console.error('Lỗi khi xóa ảnh:', err);
    }
}

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