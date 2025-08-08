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
    setAuthToken(token) {
        const expiryMs = 3 * 24 * 60 * 60 * 1000; // 3 ngày
        const expiryDate = new Date(Date.now() + expiryMs);
        try {
            localStorage.setItem('authToken', token);
            localStorage.setItem('authTokenExpiry', expiryDate.getTime().toString());
            console.log('[setAuthToken] Token and expiry saved to localStorage');
        } catch (e) {
            console.error('[setAuthToken] Error saving token to localStorage:', e);
        }
        try {
            document.cookie = `accessToken=${token}; path=/; secure; samesite=strict; expires=${expiryDate.toUTCString()}`;
            console.log('[setAuthToken] Token saved to cookie with expiry');
        } catch (e) {
            console.error('[setAuthToken] Error saving token to cookie:', e);
        }
    },
    clearAuthToken() {
        localStorage.removeItem('authToken');
        localStorage.removeItem('authTokenExpiry');
        document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    },
    async login(phone, password) {
        try {

            const apiPhone = this.formatPhoneForAPI(phone);

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

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Login failed');
            }

            if (data.token) {
                this.setAuthToken(data.token);
                this.updateNavigation();
            } else {
                console.warn('[login] No token received from API');
            }
            return data;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    },

    async register(registerData) {
        try {

            const apiPhone = this.formatPhoneForAPI(registerData.phoneNumber);

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
            this.clearAuthToken();
            this.updateNavigation();
            return { success: true };
        }
    },

    isAuthenticated() {
        const token = localStorage.getItem('authToken');
        const expiry = localStorage.getItem('authTokenExpiry');
        //console.log('Checking authentication, token exists:', !!token);

        if (!token || !expiry) return false;
        if (Date.now() > parseInt(expiry)) {
            this.logout();
            return false;
        }

        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const isExpired = payload.exp * 1000 < Date.now();
            //console.log('Token payload:', payload);
            //console.log('Token expired (JWT exp):', isExpired);

            if (isExpired) {
                console.log('Token is expired by JWT, logging out');
                this.logout();
                return false;
            }

            const userNameValue = document.getElementById('userNameIdC');
            const phoneNumberIdC = document.getElementById('phoneNumberIdC');
            let nameCls = document.getElementById('nameCls');
            let phoneCls = document.getElementById('phoneCls');
            if (userNameValue && phoneNumberIdC) {
                userNameValue.value = payload.name;
                phoneNumberIdC.value = payload.prn;
            }
            if (nameCls && phoneCls) {
                nameCls.innerHTML = payload.name;
                phoneCls.innerHTML = payload.prn;
            }

            const userProfileLink = document.getElementById('userProfileLink');
            if (userProfileLink) {
                userProfileLink.innerHTML = '<i class="fas fa-user mr-1"></i>' + payload.name;
            }
            return true;
        } catch (e) {
            console.error('Error parsing token:', e);
            this.logout();
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
            const role = payload.role || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            return {
                id: payload.sub,
                name: name,
                email: payload.email,
                role: role
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


        const userProfileNavItem = document.getElementById('userProfileNavItem');
        const authNavItems = document.getElementById('authNavItems');

        if (userProfileNavItem) {
            console.log('Login nav item visibility:', !isAuthenticated);
            userProfileNavItem.style.display = isAuthenticated ? 'block' : 'none';
        }
        if (authNavItems) {
            console.log('Register nav item visibility:', !isAuthenticated);
            authNavItems.style.display = isAuthenticated ? 'none' : 'block';
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
                this.setAuthToken(data.token);
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
                this.setAuthToken(data.token);
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