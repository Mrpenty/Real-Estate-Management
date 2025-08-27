/**
 * Report Service - Xử lý các thao tác báo cáo
 */
class ReportService {
    constructor() {
        this.baseUrl = 'http://194.233.81.64:5000';
    }

    /**
     * Kiểm tra trạng thái báo cáo
     * @param {number} targetId - ID của đối tượng bị báo cáo
     * @param {string} targetType - Loại đối tượng (PropertyPost, User, Review, etc.)
     * @param {number} userId - ID của người dùng
     * @returns {Promise<Object>} Thông tin trạng thái báo cáo
     */
    async checkReportStatus(targetId, targetType, userId) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/check-status/${targetId}/${targetType}/${userId}`);
            if (response.ok) {
                return await response.json();
            } else {
                throw new Error('Failed to check report status');
            }
        } catch (error) {
            console.error('Error checking report status:', error);
            throw error;
        }
    }

    /**
     * Báo cáo bài đăng
     * @param {number} userId - ID của người báo cáo
     * @param {Object} reportData - Dữ liệu báo cáo
     * @returns {Promise<Object>} Kết quả báo cáo
     */
    async reportPropertyPost(userId, reportData) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/post/${userId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(reportData)
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to submit report');
            }
        } catch (error) {
            console.error('Error reporting property post:', error);
            throw error;
        }
    }

    /**
     * Báo cáo người dùng
     * @param {number} userId - ID của người báo cáo
     * @param {Object} reportData - Dữ liệu báo cáo
     * @returns {Promise<Object>} Kết quả báo cáo
     */
    async reportUser(userId, reportData) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/user/${userId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(reportData)
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to submit user report');
            }
        } catch (error) {
            console.error('Error reporting user:', error);
            throw error;
        }
    }

    /**
     * Hủy báo cáo bài đăng
     * @param {number} targetId - ID của bài đăng
     * @param {number} userId - ID của người báo cáo
     * @returns {Promise<Object>} Kết quả hủy báo cáo
     */
    async cancelPropertyPostReport(targetId, userId) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/post/${targetId}/${userId}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to cancel report');
            }
        } catch (error) {
            console.error('Error canceling property post report:', error);
            throw error;
        }
    }

    /**
     * Hủy báo cáo người dùng
     * @param {number} targetId - ID của người dùng bị báo cáo
     * @param {number} userId - ID của người báo cáo
     * @returns {Promise<Object>} Kết quả hủy báo cáo
     */
    async cancelUserReport(targetId, userId) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/user/${targetId}/${userId}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to cancel user report');
            }
        } catch (error) {
            console.error('Error canceling user report:', error);
            throw error;
        }
    }

    /**
     * Lấy danh sách báo cáo của người dùng
     * @param {number} userId - ID của người dùng
     * @returns {Promise<Array>} Danh sách báo cáo
     */
    async getUserReports(userId) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/myReport/${userId}`);
            if (response.ok) {
                return await response.json();
            } else {
                throw new Error('Failed to get user reports');
            }
        } catch (error) {
            console.error('Error getting user reports:', error);
            throw error;
        }
    }

    /**
     * Lấy danh sách tất cả báo cáo (Admin only)
     * @param {Object} filters - Bộ lọc
     * @returns {Promise<Object>} Danh sách báo cáo và thông tin phân trang
     */
    async getAllReports(filters = {}) {
        try {
            const queryParams = new URLSearchParams();
            
            if (filters.targetType) queryParams.append('targetType', filters.targetType);
            if (filters.status) queryParams.append('status', filters.status);
            if (filters.keyword) queryParams.append('keyword', filters.keyword);
            if (filters.page) queryParams.append('page', filters.page);
            if (filters.pageSize) queryParams.append('pageSize', filters.pageSize);

            const response = await fetch(`${this.baseUrl}/api/Report/admin/all?${queryParams.toString()}`);
            if (response.ok) {
                return await response.json();
            } else {
                throw new Error('Failed to get all reports');
            }
        } catch (error) {
            console.error('Error getting all reports:', error);
            throw error;
        }
    }

    /**
     * Xử lý báo cáo (Admin only)
     * @param {number} reportId - ID của báo cáo
     * @param {Object} resolveData - Dữ liệu xử lý
     * @returns {Promise<Object>} Kết quả xử lý
     */
    async resolveReport(reportId, resolveData) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/${reportId}/resolve`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(resolveData)
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to resolve report');
            }
        } catch (error) {
            console.error('Error resolving report:', error);
            throw error;
        }
    }

    /**
     * Từ chối báo cáo (Admin only)
     * @param {number} reportId - ID của báo cáo
     * @returns {Promise<Object>} Kết quả từ chối
     */
    async rejectReport(reportId) {
        try {
            const response = await fetch(`${this.baseUrl}/api/Report/${reportId}/reject`, {
                method: 'PUT'
            });

            if (response.ok) {
                return await response.json();
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Failed to reject report');
            }
        } catch (error) {
            console.error('Error rejecting report:', error);
            throw error;
        }
    }
}

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ReportService;
} else {
    window.ReportService = ReportService;
} 