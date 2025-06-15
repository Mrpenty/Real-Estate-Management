// Auth Service for handling authentication API calls
const API_BASE_URL = 'https://localhost:7031/api/Auth';

const authService = {
    // Login
    async login(loginData) {
        try {
            // Validate phone number format
            if (!loginData.phoneNumber || !/^[0-9]{11}$/.test(loginData.phoneNumber)) {
                throw new Error('Please enter a valid 11-digit phone number');
            }

            const response = await fetch(`${API_BASE_URL}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    loginIdentifier: loginData.phoneNumber,
                    password: loginData.password
                })
            });

            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
                throw new Error('Server returned an invalid response format');
            }

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Login failed');
            }

            // Store the token in localStorage
            if (data.token) {
                localStorage.setItem('authToken', data.token);
            }
            return data;
        } catch (error) {
            console.error('Login error:', error);
            if (error.name === 'SyntaxError') {
                throw new Error('Server returned an invalid response. Please try again later.');
            }
            throw error;
        }
    },

    // Register
    async register(registerData) {
        try {
            const response = await fetch(`${API_BASE_URL}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(registerData)
            });

            // Handle non-JSON responses
            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
                throw new Error('Server returned an invalid response format');
            }

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Registration failed');
            }

            return data;
        } catch (error) {
            console.error('Registration error:', error);
            if (error.name === 'SyntaxError') {
                throw new Error('Server returned an invalid response. Please try again later.');
            }
            throw error;
        }
    },

    // Logout
    async logout() {
        try {
            const token = localStorage.getItem('authToken');
            const response = await fetch(`${API_BASE_URL}/logout`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Accept': 'application/json'
                }
            });

            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
                throw new Error('Server returned an invalid response format');
            }

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Logout failed');
            }

            localStorage.removeItem('authToken');
            return data;
        } catch (error) {
            console.error('Logout error:', error);
            if (error.name === 'SyntaxError') {
                throw new Error('Server returned an invalid response. Please try again later.');
            }
            throw error;
        }
    },

    // Verify OTP
    async verifyOtp(phoneNumber, otp) {
        try {
            const response = await fetch(`${API_BASE_URL}/verify-otp`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ phoneNumber, otp })
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'OTP verification failed');
            }

            const data = await response.json();
            if (data.token) {
                localStorage.setItem('authToken', data.token);
            }
            return data;
        } catch (error) {
            console.error('OTP verification error:', error);
            throw error;
        }
    },

    // Verify Email
    async verifyEmail(email) {
        try {
            const response = await fetch(`${API_BASE_URL}/verify-email`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json' 
                },
                body: JSON.stringify({ email })
            });

            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
                throw new Error('Server returned an invalid response format or no content.');
            }

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Email verification failed');
            }

            return data;
        } catch (error) {
            console.error('Email verification error:', error);
            throw error;
        }
    },

    // Google Login
    async googleLogin(idToken) {
        try {
            const response = await fetch(`${API_BASE_URL}/google-login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ idToken })
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'Google login failed');
            }

            const data = await response.json();
            if (data.token) {
                localStorage.setItem('authToken', data.token);
            }
            return data;
        } catch (error) {
            console.error('Google login error:', error);
            throw error;
        }
    },

    // Check if user is authenticated
    isAuthenticated() {
        return !!localStorage.getItem('authToken');
    },

    // Get auth token
    getAuthToken() {
        return localStorage.getItem('authToken');
    }
};

// Export the service
window.authService = authService; 