/**
 * Admin Dashboard UI Controller
 * Quản lý giao diện và tương tác cho Admin Dashboard
 */
const AdminDashboardUI = {
    // Chart instances
    charts: {},

    /**
     * Khởi tạo dashboard
     */
    async init() {
        try {
            await this.loadDashboardStats();
            await this.loadCharts();
            this.setupEventListeners();
            this.startAutoRefresh();
        } catch (error) {
            console.error('Error initializing dashboard:', error);
            this.showError('Không thể tải dữ liệu dashboard');
        }
    },

    /**
     * Tải thống kê dashboard
     */
    async loadDashboardStats() {
        try {
            const stats = await AdminDashboardService.getDashboardStats();
            this.renderDashboardStats(stats);
        } catch (error) {
            console.error('Error loading dashboard stats:', error);
            throw error;
        }
    },

    /**
     * Render thống kê dashboard
     */
    renderDashboardStats(stats) {
        const container = document.getElementById('dashboard-stats');
        if (!container) return;

        container.innerHTML = `
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
                <!-- Total Users -->
                <div class="bg-gradient-to-r from-blue-500 to-blue-600 rounded-lg p-6 text-white">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-blue-100 text-sm font-medium">Tổng người dùng</p>
                            <p class="text-3xl font-bold">${AdminDashboardService.formatNumber(stats.totalUsers)}</p>
                            <p class="text-blue-100 text-sm">+${stats.newUsersToday} hôm nay</p>
                        </div>
                        <div class="text-blue-100">
                            <i class="fas fa-users text-3xl"></i>
                        </div>
                    </div>
                </div>

                <!-- Total Properties -->
                <div class="bg-gradient-to-r from-green-500 to-green-600 rounded-lg p-6 text-white">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-green-100 text-sm font-medium">Tổng bất động sản</p>
                            <p class="text-3xl font-bold">${AdminDashboardService.formatNumber(stats.totalProperties)}</p>
                            <p class="text-green-100 text-sm">+${stats.newPropertiesToday} hôm nay</p>
                        </div>
                        <div class="text-green-100">
                            <i class="fas fa-home text-3xl"></i>
                        </div>
                    </div>
                </div>

                <!-- Total Posts -->
                <div class="bg-gradient-to-r from-purple-500 to-purple-600 rounded-lg p-6 text-white">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-purple-100 text-sm font-medium">Tổng bài đăng</p>
                            <p class="text-3xl font-bold">${AdminDashboardService.formatNumber(stats.totalPosts)}</p>
                            <p class="text-purple-100 text-sm">+${stats.newPostsToday} hôm nay</p>
                        </div>
                        <div class="text-purple-100">
                            <i class="fas fa-file-alt text-3xl"></i>
                        </div>
                    </div>
                </div>

                <!-- Total Revenue -->
                <div class="bg-gradient-to-r from-yellow-500 to-yellow-600 rounded-lg p-6 text-white">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-yellow-100 text-sm font-medium">Tổng doanh thu</p>
                            <p class="text-3xl font-bold">${AdminDashboardService.formatCurrency(stats.totalRevenue)}</p>
                            <p class="text-yellow-100 text-sm">+${AdminDashboardService.formatCurrency(stats.revenueToday)} hôm nay</p>
                        </div>
                        <div class="text-yellow-100">
                            <i class="fas fa-dollar-sign text-3xl"></i>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Status Cards -->
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
                <div class="bg-white rounded-lg p-6 shadow-md">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-gray-600 text-sm font-medium">Bài đăng chờ duyệt</p>
                            <p class="text-2xl font-bold text-yellow-600">${stats.pendingPosts}</p>
                        </div>
                        <div class="text-yellow-500">
                            <i class="fas fa-clock text-2xl"></i>
                        </div>
                    </div>
                </div>

                <div class="bg-white rounded-lg p-6 shadow-md">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-gray-600 text-sm font-medium">Bài đăng đã duyệt</p>
                            <p class="text-2xl font-bold text-green-600">${stats.approvedPosts}</p>
                        </div>
                        <div class="text-green-500">
                            <i class="fas fa-check-circle text-2xl"></i>
                        </div>
                    </div>
                </div>

                <div class="bg-white rounded-lg p-6 shadow-md">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-gray-600 text-sm font-medium">Bất động sản đã thuê</p>
                            <p class="text-2xl font-bold text-blue-600">${stats.rentedProperties}</p>
                        </div>
                        <div class="text-blue-500">
                            <i class="fas fa-key text-2xl"></i>
                        </div>
                    </div>
                </div>

                <div class="bg-white rounded-lg p-6 shadow-md">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-gray-600 text-sm font-medium">Bất động sản có sẵn</p>
                            <p class="text-2xl font-bold text-purple-600">${stats.availableProperties}</p>
                        </div>
                        <div class="text-purple-500">
                            <i class="fas fa-home text-2xl"></i>
                        </div>
                    </div>
                </div>
            </div>
        `;
    },

    /**
     * Tải và hiển thị charts
     */
    async loadCharts() {
        try {
            // Load daily stats for last 30 days
            const endDate = new Date();
            const startDate = new Date();
            startDate.setDate(startDate.getDate() - 30);
            const dailyStats = await AdminDashboardService.getDailyStats(startDate, endDate);

            // Load property stats
            const propertyStats = await AdminDashboardService.getPropertyStats();

            // Load monthly stats for current year
            const monthlyStats = await AdminDashboardService.getMonthlyStats(new Date().getFullYear());

            this.renderCharts(dailyStats, propertyStats, monthlyStats);
        } catch (error) {
            console.error('Error loading charts:', error);
            throw error;
        }
    },

    /**
     * Render charts
     */
    renderCharts(dailyStats, propertyStats, monthlyStats) {
        this.renderDailyChart(dailyStats);
        this.renderPropertyChart(propertyStats);
        this.renderMonthlyChart(monthlyStats);
        this.renderRevenueChart(dailyStats);
    },

    /**
     * Render daily chart
     */
    renderDailyChart(dailyStats) {
        const ctx = document.getElementById('dailyChart');
        if (!ctx) return;

        if (this.charts.daily) {
            this.charts.daily.destroy();
        }

        this.charts.daily = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dailyStats.map(stat => AdminDashboardService.formatDate(stat.date)),
                datasets: [{
                    label: 'Người dùng mới',
                    data: dailyStats.map(stat => stat.newUsers),
                    borderColor: 'rgb(59, 130, 246)',
                    backgroundColor: 'rgba(59, 130, 246, 0.1)',
                    tension: 0.4
                }, {
                    label: 'Bất động sản mới',
                    data: dailyStats.map(stat => stat.newProperties),
                    borderColor: 'rgb(34, 197, 94)',
                    backgroundColor: 'rgba(34, 197, 94, 0.1)',
                    tension: 0.4
                }, {
                    label: 'Bài đăng mới',
                    data: dailyStats.map(stat => stat.newPosts),
                    borderColor: 'rgb(168, 85, 247)',
                    backgroundColor: 'rgba(168, 85, 247, 0.1)',
                    tension: 0.4
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Thống kê hàng ngày (30 ngày gần nhất)'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    },

    /**
     * Render property chart
     */
    renderPropertyChart(propertyStats) {
        const ctx = document.getElementById('propertyChart');
        if (!ctx) return;

        if (this.charts.property) {
            this.charts.property.destroy();
        }

        this.charts.property = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: propertyStats.map(stat => stat.status),
                datasets: [{
                    data: propertyStats.map(stat => stat.count),
                    backgroundColor: [
                        '#3B82F6',
                        '#10B981',
                        '#F59E0B',
                        '#EF4444',
                        '#8B5CF6'
                    ]
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Phân bố trạng thái bất động sản'
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    },

    /**
     * Render monthly chart
     */
    renderMonthlyChart(monthlyStats) {
        const ctx = document.getElementById('monthlyChart');
        if (!ctx) return;

        if (this.charts.monthly) {
            this.charts.monthly.destroy();
        }

        const monthNames = ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12'];

        this.charts.monthly = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: monthlyStats.map(stat => monthNames[stat.month - 1]),
                datasets: [{
                    label: 'Doanh thu (triệu VND)',
                    data: monthlyStats.map(stat => stat.revenue / 1000000),
                    backgroundColor: 'rgba(59, 130, 246, 0.8)',
                    borderColor: 'rgb(59, 130, 246)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Doanh thu theo tháng'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    },

    /**
     * Render revenue chart
     */
    renderRevenueChart(dailyStats) {
        const ctx = document.getElementById('revenueChart');
        if (!ctx) return;

        if (this.charts.revenue) {
            this.charts.revenue.destroy();
        }

        this.charts.revenue = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dailyStats.map(stat => AdminDashboardService.formatDate(stat.date)),
                datasets: [{
                    label: 'Doanh thu (triệu VND)',
                    data: dailyStats.map(stat => stat.revenue / 1000000),
                    borderColor: 'rgb(245, 158, 11)',
                    backgroundColor: 'rgba(245, 158, 11, 0.1)',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Doanh thu hàng ngày'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    },

    /**
     * Setup event listeners
     */
    setupEventListeners() {
        // Date range picker
        const dateRangePicker = document.getElementById('dateRangePicker');
        if (dateRangePicker) {
            dateRangePicker.addEventListener('change', this.handleDateRangeChange.bind(this));
        }

        // Report download buttons
        const downloadButtons = document.querySelectorAll('[data-report-download]');
        downloadButtons.forEach(button => {
            button.addEventListener('click', this.handleReportDownload.bind(this));
        });

        // Refresh button
        const refreshButton = document.getElementById('refreshDashboard');
        if (refreshButton) {
            refreshButton.addEventListener('click', this.refreshDashboard.bind(this));
        }

        // Tab navigation
        const tabButtons = document.querySelectorAll('[data-tab]');
        tabButtons.forEach(button => {
            button.addEventListener('click', this.handleTabChange.bind(this));
        });
    },

    /**
     * Handle date range change
     */
    async handleDateRangeChange(event) {
        try {
            const [startDate, endDate] = event.target.value.split(' to ');
            if (startDate && endDate) {
                const start = new Date(startDate);
                const end = new Date(endDate);
                const dailyStats = await AdminDashboardService.getDailyStats(start, end);
                this.renderDailyChart(dailyStats);
                this.renderRevenueChart(dailyStats);
            }
        } catch (error) {
            console.error('Error handling date range change:', error);
            this.showError('Không thể cập nhật dữ liệu');
        }
    },

    /**
     * Handle report download
     */
    async handleReportDownload(event) {
        event.preventDefault();
        
        const button = event.currentTarget;
        const reportType = button.dataset.reportType;
        const format = button.dataset.format;
        const days = parseInt(button.dataset.days) || 30;
        
        try {
            button.disabled = true;
            button.innerHTML = '<i class="fas fa-spinner fa-spin mr-2"></i>Đang tải...';
            
            const endDate = new Date();
            const startDate = new Date();
            startDate.setDate(startDate.getDate() - days);
            
            await AdminDashboardService.downloadReport(reportType, format, startDate, endDate);
            this.showSuccess('Báo cáo đã được tải xuống thành công!');
        } catch (error) {
            console.error('Error downloading report:', error);
            this.showError('Không thể tải xuống báo cáo: ' + error.message);
        } finally {
            button.disabled = false;
            button.innerHTML = button.dataset.originalText || 'Tải xuống';
        }
    },

    /**
     * Refresh dashboard
     */
    async refreshDashboard() {
        try {
            const refreshButton = document.getElementById('refreshDashboard');
            if (refreshButton) {
                refreshButton.disabled = true;
                refreshButton.innerHTML = '<i class="fas fa-spinner fa-spin mr-2"></i>Đang tải...';
            }

            await this.loadDashboardStats();
            await this.loadCharts();
            
            this.showSuccess('Dashboard đã được cập nhật!');
        } catch (error) {
            console.error('Error refreshing dashboard:', error);
            this.showError('Không thể cập nhật dashboard');
        } finally {
            const refreshButton = document.getElementById('refreshDashboard');
            if (refreshButton) {
                refreshButton.disabled = false;
                refreshButton.innerHTML = '<i class="fas fa-sync-alt mr-2"></i>Làm mới';
            }
        }
    },

    /**
     * Handle tab change
     */
    handleTabChange(event) {
        const targetTab = event.currentTarget.dataset.tab;
        
        // Hide all tab contents
        document.querySelectorAll('[data-tab-content]').forEach(content => {
            content.classList.add('hidden');
        });
        
        // Show target tab content
        const targetContent = document.querySelector(`[data-tab-content="${targetTab}"]`);
        if (targetContent) {
            targetContent.classList.remove('hidden');
        }
        
        // Update active tab
        document.querySelectorAll('[data-tab]').forEach(button => {
            button.classList.remove('bg-blue-600', 'text-white');
            button.classList.add('bg-gray-200', 'text-gray-700');
        });
        
        event.currentTarget.classList.remove('bg-gray-200', 'text-gray-700');
        event.currentTarget.classList.add('bg-blue-600', 'text-white');
    },

    /**
     * Start auto refresh
     */
    startAutoRefresh() {
        // Refresh every 5 minutes
        setInterval(() => {
            this.loadDashboardStats();
        }, 5 * 60 * 1000);
    },

    /**
     * Show success message
     */
    showSuccess(message) {
        this.showNotification(message, 'success');
    },

    /**
     * Show error message
     */
    showError(message) {
        this.showNotification(message, 'error');
    },

    /**
     * Show notification
     */
    showNotification(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `fixed top-4 right-4 p-4 rounded-lg shadow-lg z-50 ${
            type === 'success' ? 'bg-green-500 text-white' :
            type === 'error' ? 'bg-red-500 text-white' :
            'bg-blue-500 text-white'
        }`;
        notification.innerHTML = `
            <div class="flex items-center">
                <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : 'info-circle'} mr-2"></i>
                <span>${message}</span>
            </div>
        `;
        
        document.body.appendChild(notification);
        
        setTimeout(() => {
            notification.remove();
        }, 3000);
    }
};

// Export cho sử dụng global
window.AdminDashboardUI = AdminDashboardUI; 