// AdminPostService.js
const ADMIN_POST_API_BASE_URL = 'https://localhost:7031/api/Admin/PropertyPosts';

window.AdminPostService = {
    async getPosts(status = '', page = 1, pageSize = 10) {
        const res = await fetch(`${ADMIN_POST_API_BASE_URL}?status=${status || ''}&page=${page}&pageSize=${pageSize}`, {
            method: 'GET',
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy danh sách bài đăng');
        return await res.json();
    },
    async getPostDetail(id) {
        const res = await fetch(`${ADMIN_POST_API_BASE_URL}/${id}`, {
            method: 'GET',
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy chi tiết bài đăng');
        return await res.json();
    },
    async updatePostStatus(id, status, rejectReason = null) {
        const body = rejectReason ? { status, rejectReason } : { status };
        const res = await fetch(`${ADMIN_POST_API_BASE_URL}/${id}/status`, {
            method: 'PUT',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error('Không thể cập nhật trạng thái bài đăng');
        return await res.json();
    }
}; 