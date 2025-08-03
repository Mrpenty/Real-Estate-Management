const ADMIN_API_BASE_URL = "https://localhost:7031";

const AdminDashboardService = {
    /**
     * Lấy thống kê tổng quan dashboard
     * @returns {Promise<Object>}
     */
    async getDashboardStats() {
        try {
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/stats`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê dashboard');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching dashboard stats:', error);
            throw error;
        }
    },

    /**
     * Lấy thống kê hàng ngày
     * @param {Date} startDate - Ngày bắt đầu
     * @param {Date} endDate - Ngày kết thúc
     * @returns {Promise<Array>}
     */
    async getDailyStats(startDate, endDate) {
        try {
            const start = startDate.toISOString().split('T')[0];
            const end = endDate.toISOString().split('T')[0];
            
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/daily-stats?startDate=${start}&endDate=${end}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê hàng ngày');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching daily stats:', error);
            throw error;
        }
    },

    /**
     * Lấy thống kê hàng tháng
     * @param {number} year - Năm cần thống kê
     * @returns {Promise<Array>}
     */
    async getMonthlyStats(year) {
        try {
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/monthly-stats/${year}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê hàng tháng');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching monthly stats:', error);
            throw error;
        }
    },

    /**
     * Lấy thống kê bất động sản
     * @returns {Promise<Array>}
     */
    async getPropertyStats() {
        try {
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/property-stats`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê bất động sản');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching property stats:', error);
            throw error;
        }
    },

    /**
     * Lấy thống kê người dùng
     * @returns {Promise<Array>}
     */
    async getUserStats() {
        try {
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/user-stats`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê người dùng');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching user stats:', error);
            throw error;
        }
    },

    /**
     * Lấy thống kê doanh thu
     * @param {Date} startDate - Ngày bắt đầu
     * @param {Date} endDate - Ngày kết thúc
     * @returns {Promise<Array>}
     */
    async getRevenueStats(startDate, endDate) {
        try {
            const start = startDate.toISOString().split('T')[0];
            const end = endDate.toISOString().split('T')[0];
            
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/revenue-stats?startDate=${start}&endDate=${end}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thống kê doanh thu');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching revenue stats:', error);
            throw error;
        }
    },

    /**
     * Lấy tổng quan hệ thống
     * @returns {Promise<Object>}
     */
    async getSystemOverview() {
        try {
            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/system-overview`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (!response.ok) {
                throw new Error('Không thể lấy tổng quan hệ thống');
            }

            const result = await response.json();
            return result.data;
        } catch (error) {
            console.error('Error fetching system overview:', error);
            throw error;
        }
    },

    /**
     * Tải xuống báo cáo
     * @param {string} reportType - Loại báo cáo (daily, monthly, property, user, revenue)
     * @param {string} format - Định dạng (excel, pdf)
     * @param {Date} startDate - Ngày bắt đầu
     * @param {Date} endDate - Ngày kết thúc
     * @returns {Promise<boolean>}
     */
    async downloadReport(reportType, format, startDate, endDate) {
        try {
            const requestBody = {
                reportType: reportType,
                format: format,
                startDate: startDate.toISOString(),
                endDate: endDate.toISOString()
            };

            const response = await fetch(`${ADMIN_API_BASE_URL}/api/Admin/Dashboard/download-report`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify(requestBody)
            });

            if (!response.ok) {
                throw new Error('Không thể tải xuống báo cáo');
            }

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `report_${reportType}_${startDate.toISOString().split('T')[0]}_${endDate.toISOString().split('T')[0]}.${format}`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

            return true;
        } catch (error) {
            console.error('Error downloading report:', error);
            throw error;
        }
    },

    /**
     * Format số tiền theo định dạng Việt Nam
     * @param {number} amount - Số tiền
     * @returns {string}
     */
    formatCurrency(amount) {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(amount);
    },

    /**
     * Format số theo định dạng có dấu phẩy
     * @param {number} number - Số cần format
     * @returns {string}
     */
    formatNumber(number) {
        return new Intl.NumberFormat('vi-VN').format(number);
    },

    /**
     * Format ngày theo định dạng Việt Nam
     * @param {string|Date} date - Ngày cần format
     * @returns {string}
     */
    formatDate(date) {
        return new Intl.DateTimeFormat('vi-VN').format(new Date(date));
    }
};

// Export cho sử dụng global
window.AdminDashboardService = AdminDashboardService; 