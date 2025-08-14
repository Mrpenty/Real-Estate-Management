// Floating Support Button Management
class FloatingSupportButton {
    constructor() {
        this.button = document.querySelector('.floating-support-btn');
        this.unreadBadge = document.getElementById('supportUnreadBadge');
        this.isVisible = true;
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.checkUnreadMessages();
        this.setupAutoRefresh();
    }

    setupEventListeners() {
        // Click event
        if (this.button) {
            this.button.addEventListener('click', (e) => {
                e.preventDefault();
                this.openSimpleSupportChat();
            });
        }

        // Scroll event to hide/show button
        let lastScrollTop = 0;
        window.addEventListener('scroll', () => {
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            
            if (scrollTop > lastScrollTop && scrollTop > 100) {
                // Scrolling down, hide button
                this.hide();
            } else {
                // Scrolling up, show button
                this.show();
            }
            lastScrollTop = scrollTop;
        });

        // Keyboard navigation support
        document.addEventListener('keydown', (e) => {
            if (e.key === 'F1' && e.ctrlKey) {
                e.preventDefault();
                this.openSimpleSupportChat();
            }
        });
    }

            openSimpleSupportChat() {
            if (!this.isAuthenticated()) {
                this.showLoginPrompt();
                return;
            }
            
            // Open simple chat modal
            if (window.openSimpleSupportChat) {
                window.openSimpleSupportChat();
            } else {
                // Fallback to redirect if modal function not available
                window.location.href = '/Support/Index';
            }
        }

    isAuthenticated() {
        return window.authService && window.authService.isAuthenticated();
    }

    showLoginPrompt() {
        if (window.Swal) {
            Swal.fire({
                icon: 'info',
                title: 'Vui lòng đăng nhập',
                text: 'Bạn cần đăng nhập để có thể liên hệ hỗ trợ.',
                confirmButtonText: 'Đăng nhập',
                showCancelButton: true,
                cancelButtonText: 'Hủy',
                confirmButtonColor: '#ef4444',
                cancelButtonColor: '#6b7280'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/Auth/Login';
                }
            });
        } else {
            // Fallback if SweetAlert is not available
            if (confirm('Bạn cần đăng nhập để có thể liên hệ hỗ trợ. Nhấn OK để đăng nhập.')) {
                window.location.href = '/Auth/Login';
            }
        }
    }

    async checkUnreadMessages() {
        if (!this.isAuthenticated() || !this.unreadBadge) return;
        
        try {
            const token = localStorage.getItem('authToken') || 
                          localStorage.getItem('accessToken') || 
                          localStorage.getItem('token');
            
            if (!token) return;

            const response = await fetch('/api/Support/user-conversations', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                const conversations = await response.json();
                const totalUnread = conversations.reduce((sum, conv) => sum + (conv.unreadCount || 0), 0);
                this.updateUnreadBadge(totalUnread);
            }
        } catch (error) {
            console.error('Error checking unread support messages:', error);
        }
    }

    updateUnreadBadge(count) {
        if (!this.unreadBadge) return;
        
        if (count > 0) {
            this.unreadBadge.textContent = count > 99 ? '99+' : count;
            this.unreadBadge.classList.remove('hidden');
            
            // Add pulse animation for new messages
            this.unreadBadge.classList.add('animate-pulse');
            setTimeout(() => {
                this.unreadBadge.classList.remove('animate-pulse');
            }, 2000);
        } else {
            this.unreadBadge.classList.add('hidden');
        }
    }

    setupAutoRefresh() {
        // Check for new messages every 30 seconds
        setInterval(() => {
            if (this.isAuthenticated()) {
                this.checkUnreadMessages();
            }
        }, 30000);

        // Check when user becomes authenticated
        if (window.authService) {
            // Listen for authentication changes
            const originalUpdateNavigation = window.updateNavigation;
            if (originalUpdateNavigation) {
                window.updateNavigation = function() {
                    originalUpdateNavigation();
                    // Check unread messages after navigation update
                    setTimeout(() => {
                        if (window.floatingSupportButton) {
                            window.floatingSupportButton.checkUnreadMessages();
                        }
                    }, 100);
                };
            }
        }
    }

    show() {
        if (this.button && !this.isVisible) {
            this.button.style.transform = 'translateY(0)';
            this.button.style.opacity = '1';
            this.isVisible = true;
        }
    }

    hide() {
        if (this.button && this.isVisible) {
            this.button.style.transform = 'translateY(100px)';
            this.button.style.opacity = '0';
            this.isVisible = false;
        }
    }

    // Public method to refresh unread count
    refresh() {
        this.checkUnreadMessages();
    }
}

// Initialize floating support button when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Wait a bit for authService to be available
    setTimeout(() => {
        window.floatingSupportButton = new FloatingSupportButton();
    }, 500);
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = FloatingSupportButton;
} 