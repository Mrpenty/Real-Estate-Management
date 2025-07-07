const ADMIN_API_BASE_URL = "https://localhost:7031";
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
        const res = await fetch(`${ADMIN_API_BASE_URL}/api/User/admin-list?${query}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            },
            credentials: 'include'
        });
        if (!res.ok) throw new Error('Không thể lấy danh sách user');
        return await res.json();
    },
    // Có thể bổ sung các hàm duyệt, ban, đổi role, ... sau
};
window.AdminUserService = AdminUserService; 