// AdminPostService.js
const AdminPostService = {
    /**
     * Lấy danh sách posts cho admin
     * @param {Object} params { search, status, page, pageSize }
     * @returns {Promise}
     */
    async getPosts(params = {}) {
        // Xóa các param rỗng hoặc undefined
        Object.keys(params).forEach(key => {
            if (params[key] === "" || params[key] === undefined) {
                delete params[key];
            }
        });
        const query = new URLSearchParams(params).toString();
        const res = await fetch(`${config.buildApiUrl(config.admin.posts.list)}?${query}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            },
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy danh sách posts');
        return await res.json();
    },

    /**
     * Lấy thông tin chi tiết post
     * @param {number} postId 
     * @returns {Promise}
     */
    async getPostDetail(postId) {
        const res = await fetch(config.buildApiUrl(config.admin.posts.detail.replace('{postId}', postId)), {
            method: 'GET',
            headers: { 'Accept': 'application/json' },
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy thông tin chi tiết post');
        return await res.json();
    },

    /**
     * Cập nhật trạng thái post
     * @param {number} postId 
     * @param {string} status 
     * @param {string} rejectReason 
     * @returns {Promise}
     */
    async updatePostStatus(postId, status, rejectReason = null) {
        const body = rejectReason ? { status, rejectReason } : { status };
        const res = await fetch(config.buildApiUrl(config.admin.posts.update.replace('{postId}', postId)), {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            credentials: 'include',
            body: JSON.stringify(body)
            // Note: Backend chỉ nhận { status }, không nhận rejectReason
        });
        return res;
    },

    /**
     * Xóa post
     * @param {number} postId 
     * @returns {Promise}
     */
    async deletePost(postId) {
        const res = await fetch(config.buildApiUrl(config.admin.posts.delete.replace('{postId}', postId)), {
            method: 'DELETE',
            credentials: 'include'
        });
        return res;
    }
};
window.AdminPostService = AdminPostService; 