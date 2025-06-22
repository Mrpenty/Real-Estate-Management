const API_BASE_URL = 'https://localhost:7031/api/Auth';

const authService = {
    // Phone number utilities
    formatPhoneForDisplay(phone) {
        if (!phone) return '';
        return phone.startsWith('+84') ? '0' + phone.substring(3) : phone;
    },

    formatPhoneForAPI(phone) {
        if (!phone) return '';
        // Handle both 10 and 11 digit numbers starting with 0
        if (phone.startsWith('0') && (phone.length === 10 || phone.length === 11)) {
            return '+84' + phone.substring(1);
        }
        return phone;
    },

    validatePhoneNumber(phone) {
        if (!phone) return false;
        return (phone.startsWith('0') && phone.length === 11) ||
            (phone.startsWith('+84') && phone.length === 12);
    },

    // Authentication methods
    async login(phone, password) {
        try {
            console.log('Login attempt with phone:', phone);

            const apiPhone = this.formatPhoneForAPI(phone);
            console.log('Formatted phone for API:', apiPhone);

            const response = await fetch(`${API_BASE_URL}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    loginIdentifier: apiPhone,
                    password: password
                })
            });

            console.log('Login response status:', response.status);
            const data = await response.json();
            console.log('Login response data:', data);

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Login failed');
            }

            if (data.token) {
                console.log('Storing token in localStorage');
                localStorage.setItem('authToken', data.token);
                this.updateNavigation();
            }
            return data;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    },

    async register(registerData) {
        try {
            console.log('Register attempt with data:', registerData);

            const apiPhone = this.formatPhoneForAPI(registerData.phoneNumber);
            console.log('Formatted phone for API:', apiPhone);

            const response = await fetch(`${API_BASE_URL}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    name: registerData.name,
                    email: registerData.email,
                    phoneNumber: apiPhone,
                    password: registerData.password
                })
            });

            const data = await response.json();
            console.log('Register response:', data);

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Registration failed');
            }

            return data;
        } catch (error) {
            console.error('Registration error:', error);
            throw error;
        }
    },

    async verifyOTP(phone, otp) {
        try {
            console.log('Verify OTP attempt:', { phone, otp });

            const response = await fetch(`${API_BASE_URL}/verify-otp`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({ phoneNumber: phone, otp })
            });

            const data = await response.json();
            console.log('Verify OTP response:', data);

            if (!response.ok || !data.isAuthSuccessful) {
                throw new Error(data.errorMessage || 'OTP verification failed');
            }

            return data;
        } catch (error) {
            console.error('OTP verification error:', error);
            throw error;
        }
    },

    async resendOTP(phone) {
        try {
            console.log('Resend OTP attempt for phone:', phone);

            const response = await fetch(`${API_BASE_URL}/resend-otp`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({ phoneNumber: phone })
            });

            const data = await response.json();
            console.log('Resend OTP response:', data);

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Failed to resend OTP');
            }

            return data;
        } catch (error) {
            console.error('Resend OTP error:', error);
            throw error;
        }
    },

    async logout() {
        try {
            const token = localStorage.getItem('authToken');
            if (token) {
                await fetch(`${API_BASE_URL}/logout`, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
            }
        } catch (error) {
            console.error('Logout API error:', error);
        } finally {
            localStorage.removeItem('authToken');
            this.updateNavigation();
            return { success: true };
        }
    },

    isAuthenticated() {
        const token = localStorage.getItem('authToken');
        console.log('Checking authentication, token exists:', !!token);

        if (!token) return false;

        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const isExpired = payload.exp * 1000 < Date.now();
            console.log('Token payload:', payload);
            console.log('Token expired:', isExpired);

            if (isExpired) {
                console.log('Token is expired, removing from localStorage');
                localStorage.removeItem('authToken');
                this.updateNavigation();
                return false;
            }
            return true;
        } catch (e) {
            console.error('Error parsing token:', e);
            localStorage.removeItem('authToken');
            this.updateNavigation();
            return false;
        }
    },

    getAuthToken() {
        return localStorage.getItem('authToken');
    },

    getCurrentUser() {
        const token = localStorage.getItem('authToken');
        if (!token) return null;

        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            // The key for the name claim in JWT is often 'name' or a schema URL
            const name = payload.name || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
            return {
                id: payload.sub,
                name: name,
                email: payload.email
                // Add other fields from payload as needed
            };
        } catch (e) {
            console.error('Failed to parse token or get user info:', e);
            return null;
        }
    },

    updateNavigation() {
        console.log('Updating navigation');
        const isAuthenticated = this.isAuthenticated();
        console.log('Is authenticated:', isAuthenticated);

        const loginNavItem = document.getElementById('loginNavItem');
        const registerNavItem = document.getElementById('registerNavItem');
        const logoutNavItem = document.getElementById('logoutNavItem');

        if (loginNavItem) {
            console.log('Login nav item visibility:', !isAuthenticated);
            loginNavItem.style.display = isAuthenticated ? 'none' : 'block';
        }
        if (registerNavItem) {
            console.log('Register nav item visibility:', !isAuthenticated);
            registerNavItem.style.display = isAuthenticated ? 'none' : 'block';
        }
        if (logoutNavItem) {
            console.log('Logout nav item visibility:', isAuthenticated);
            logoutNavItem.style.display = isAuthenticated ? 'block' : 'none';
        }
    },

    async verifyEmail(email) {
        try {
            console.log('Verify email attempt:', email);

            const response = await fetch(`${API_BASE_URL}/verify-email`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({ email })
            });

            const data = await response.json();
            console.log('Verify email response:', data);

            if (!response.ok) {
                throw new Error(data.errorMessage || 'Email verification failed');
            }

            if (data.token) {
                localStorage.setItem('authToken', data.token);
                this.updateNavigation();
            }
            return data;
        } catch (error) {
            console.error('Email verification error:', error);
            throw error;
        }
    },

    async googleLogin(idToken) {
        try {
            console.log('Google login attempt');

            const response = await fetch(`${API_BASE_URL}/google-login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({ idToken })
            });

            const data = await response.json();
            console.log('Google login response:', data);

            if (!response.ok) {
                throw new Error(data.errorMessage || 'Google login failed');
            }

            if (data.token) {
                localStorage.setItem('authToken', data.token);
                this.updateNavigation();
            }
            return data;
        } catch (error) {
            console.error('Google login error:', error);
            throw error;
        }
    },
};

// Export the service
window.authService = authService;

// Initialize navigation on page load
document.addEventListener('DOMContentLoaded', () => {
    console.log('Page loaded, initializing navigation');
    authService.updateNavigation();
}); 