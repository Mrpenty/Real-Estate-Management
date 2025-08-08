// Report Management Service
const ReportManagementService = {
    baseUrl: 'https://localhost:7031',

    async getAllReports(filters = {}) {
        const params = new URLSearchParams();
        
        if (filters.targetType) params.append('targetType', filters.targetType);
        if (filters.status) params.append('status', filters.status);
        if (filters.keyword) params.append('keyword', filters.keyword);
        if (filters.page) params.append('page', filters.page);
        if (filters.pageSize) params.append('pageSize', filters.pageSize);

        const token = localStorage.getItem('authToken');
        const response = await fetch(`${this.baseUrl}/api/Report/admin/all?${params}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Không thể tải danh sách báo cáo');
        }

        return await response.json();
    },

    async getReportById(id) {
        const token = localStorage.getItem('authToken');
        const response = await fetch(`${this.baseUrl}/api/Report/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Không thể tải chi tiết báo cáo');
        }

        return await response.json();
    },

    async resolveReport(id, adminNote) {
        const token = localStorage.getItem('authToken');
        const response = await fetch(`${this.baseUrl}/api/Report/${id}/resolve`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ adminNote })
        });

        if (!response.ok) {
            throw new Error('Không thể xử lý báo cáo');
        }

        return await response.json();
    },

    async rejectReport(id) {
        const token = localStorage.getItem('authToken');
        const response = await fetch(`${this.baseUrl}/api/Report/${id}/reject`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Không thể từ chối báo cáo');
        }

        return await response.json();
    }
};

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ReportManagementService;
} 