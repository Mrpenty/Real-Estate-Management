const AdminUserService = {
    /**
     * Lấy danh sách user cho admin (role: renter, landlord), có filter, search, paging
     * @param {Object} params { search, role, isActive, page, pageSize }
     * @returns {Promise}
     */
    async getUsers(params = {}) {
        // Xóa các param rỗng hoặc undefined
        Object.keys(params).forEach(key => {
            if (params[key] === "" || params[key] === undefined) {
                delete params[key];
            }
        });
        const query = new URLSearchParams(params).toString();
        const res = await fetch(`${config.buildApiUrl(config.admin.users.adminList)}?${query}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            },
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy danh sách user');
        return await res.json();
    },

    /**
     * Lấy thông tin chi tiết user
     * @param {number} userId 
     * @returns {Promise}
     */
    async getUserDetail(userId) {
        const res = await fetch(config.buildApiUrl(config.admin.users.detail.replace('{userId}', userId)), {
            method: 'GET',
            headers: { 'Accept': 'application/json' },
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy thông tin chi tiết user');
        return await res.json();
    },

    /**
     * Cập nhật thông tin user
     * @param {number} userId 
     * @param {Object} userData 
     * @returns {Promise}
     */
    async updateUser(userId, userData) {
        const res = await fetch(config.buildApiUrl(config.admin.users.update.replace('{userId}', userId)), {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            credentials: 'include',
            body: JSON.stringify(userData)
        });
        return res;
    },

    /**
     * Xóa user
     * @param {number} userId 
     * @returns {Promise}
     */
    async deleteUser(userId) {
        const res = await fetch(config.buildApiUrl(config.admin.users.delete.replace('{userId}', userId)), {
            method: 'DELETE',
            credentials: 'include'
        });
        return res;
    }
};
window.AdminUserService = AdminUserService; 