// Support Service for handling support-related API calls
class SupportService {
    constructor() {
        this.baseUrl = 'https://localhost:7031/api/Support';
        this.token = localStorage.getItem('authToken') || localStorage.getItem('accessToken') || localStorage.getItem('token');
    }

    // Get auth headers
    getAuthHeaders() {
        return {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.token}`
        };
    }

    // Create new support conversation
    async createSupportConversation(data) {
        try {
            const response = await fetch(`${this.baseUrl}/create-conversation`, {
                method: 'POST',
                headers: this.getAuthHeaders(),
                body: JSON.stringify(data)
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to create support conversation: ${error.message}`);
        }
    }

    // Get user's support conversations
    async getUserSupportConversations() {
        try {
            const response = await fetch(`${this.baseUrl}/user-conversations`, {
                headers: this.getAuthHeaders()
            });

            if (!response.ok) {
                const error = await response.text();
                throw new Error(error);
            }

            return await response.json();
        } catch (error) {
            throw new Error(`Failed to get support conversations: ${error.message}`);
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

    // Send support message
    async sendSupportMessage(conversationId, content) {
        try {
            const response = await fetch(`${this.baseUrl}/conversation/${conversationId}/send-message`, {
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
            throw new Error(`Failed to send support message: ${error.message}`);
        }
    }

    // Get all support conversations (admin only)
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

    // Check if user is authenticated
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
                email: payload.email
            };
        } catch (error) {
            console.error('Error parsing token:', error);
            return null;
        }
    }

    // Refresh token if needed
    async refreshTokenIfNeeded() {
        // Implement token refresh logic if needed
        // This is a placeholder for future implementation
        return true;
    }
}

// Global support service instance
window.supportService = new SupportService(); 