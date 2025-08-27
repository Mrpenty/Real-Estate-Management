// Admin Support Service for handling admin support-related API calls
class AdminSupportService {
    constructor() {
        this.baseUrl = 'http://194.233.81.64:5000/api/Support';
        this.token = localStorage.getItem('authToken') || localStorage.getItem('accessToken') || localStorage.getItem('token');
    }

    // Get auth headers
    getAuthHeaders() {
        return {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.token}`
        };
    }

    // Get all support conversations with filters
    async getAllSupportConversations(status = null, priority = null) {
        try {
            let url = `${this.baseUrl}/admin/all-conversations`;
            const params = new URLSearchParams();
            
            if (status) params.append('status', status);
            if (priority) params.append('priority', priority);
            
            if (params.toString()) {
                url += '?' + params.toString();
            }

            const response = await fetch(url, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get all support conversations: ${error.message}`);
        }
    }

    // Get support messages for a conversation
    async getSupportMessages(conversationId, skip = 0, take = 50) {
        try {
            const response = await fetch(`${this.baseUrl}/conversation/${conversationId}/messages?skip=${skip}&take=${take}`, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get support messages: ${error.message}`);
        }
    }

    // Admin reply to support
    async adminReply(conversationId, content) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/conversation/${conversationId}/reply`, {
                method: 'POST',
                headers: this.getAuthHeaders(),
                body: JSON.stringify(content)
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to send admin reply: ${error.message}`);
        }
    }

    // Update support status
    async updateSupportStatus(conversationId, status, adminId = null) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/conversation/${conversationId}/status`, {
                method: 'PUT',
                headers: this.getAuthHeaders(),
                body: JSON.stringify({
                    status: status,
                    adminId: adminId
                })
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to update support status: ${error.message}`);
        }
    }

    // Get support conversation details
    async getSupportConversation(conversationId) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/conversation/${conversationId}`, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get support conversation: ${error.message}`);
        }
    }

    // Assign conversation to admin
    async assignConversation(conversationId, adminId) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/conversation/${conversationId}/assign`, {
                method: 'PUT',
                headers: this.getAuthHeaders(),
                body: JSON.stringify({
                    adminId: adminId
                })
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to assign conversation: ${error.message}`);
        }
    }

    // Get support statistics
    async getSupportStatistics() {
        try {
            const response = await fetch(`${this.baseUrl}/admin/statistics`, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get support statistics: ${error.message}`);
        }
    }

    // Search support conversations
    async searchSupportConversations(searchTerm, skip = 0, take = 20) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/search?searchTerm=${encodeURIComponent(searchTerm)}&skip=${skip}&take=${take}`, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to search support conversations: ${error.message}`);
        }
    }

    // Export support conversations
    async exportSupportConversations(format = 'json', filters = {}) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/export?format=${format}`, {
                method: 'POST',
                headers: this.getAuthHeaders(),
                body: JSON.stringify(filters)
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            if (format === 'csv') {
                const blob = await response.blob();
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = `support_conversations_${new Date().toISOString().split('T')[0]}.csv`;
                a.click();
                window.URL.revokeObjectURL(url);
                return { success: true, message: 'Export completed' };
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to export support conversations: ${error.message}`);
        }
    }

    // Bulk update support status
    async bulkUpdateSupportStatus(conversationIds, status, adminId = null) {
        try {
            const response = await fetch(`${this.baseUrl}/admin/bulk-update-status`, {
                method: 'PUT',
                headers: this.getAuthHeaders(),
                body: JSON.stringify({
                    conversationIds: conversationIds,
                    status: status,
                    adminId: adminId
                })
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to bulk update support status: ${error.message}`);
        }
    }

    // Get admin performance metrics
    async getAdminPerformanceMetrics(adminId = null, startDate = null, endDate = null) {
        try {
            let url = `${this.baseUrl}/admin/performance-metrics`;
            const params = new URLSearchParams();
            
            if (adminId) params.append('adminId', adminId);
            if (startDate) params.append('startDate', startDate);
            if (endDate) params.append('endDate', endDate);
            
            if (params.toString()) {
                url += '?' + params.toString();
            }

            const response = await fetch(url, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get admin performance metrics: ${error.message}`);
        }
    }

    // Check if user is authenticated and is admin
    isAuthenticated() {
        return !!this.token;
    }

    // Get current user info from token
    getCurrentUser() {
        if (!this.token) return null;
        
        try {
            const payload = JSON.parse(atob(this.token.split('.')[1]));
            return {
                id: payload.id || payload.userId || payload.sub,
                name: payload.name || payload.userName,
                email: payload.email,
                role: payload.role || payload.roleName
            };
        } catch (error) {
            console.error('Error parsing token:', error);
            return null;
        }
    }

    // Check if current user is admin
    async isAdmin() {
        try {
            const user = this.getCurrentUser();
            if (!user) return false;

            // You can implement additional admin check logic here
            // For now, we'll assume the role is in the token
            return user.role === 'Admin' || user.role === 'admin';
        } catch (error) {
            console.error('Error checking admin status:', error);
            return false;
        }
    }

    // Refresh token if needed
    async refreshTokenIfNeeded() {
        // Implement token refresh logic if needed
        // This is a placeholder for future implementation
        return true;
    }

    // Utility method to format date
    formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('vi-VN', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    // Utility method to get status color
    getStatusColor(status) {
        switch (status) {
            case 'Open': return 'bg-blue-100 text-blue-800';
            case 'InProgress': return 'bg-yellow-100 text-yellow-800';
            case 'Resolved': return 'bg-green-100 text-green-800';
            case 'Closed': return 'bg-gray-100 text-gray-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    }

    // Utility method to get priority color
    getPriorityColor(priority) {
        switch (priority) {
            case 'Low': return 'bg-gray-100 text-gray-800';
            case 'Normal': return 'bg-blue-100 text-blue-800';
            case 'High': return 'bg-orange-100 text-orange-800';
            case 'Urgent': return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    }
}

// Global admin support service instance
window.adminSupportService = new AdminSupportService(); 