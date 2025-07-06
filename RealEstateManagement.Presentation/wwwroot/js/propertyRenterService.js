const API_PROPERTY_BASE_URL_V1 = 'https://localhost:7031/api/Property';

const propertyRenterService = {

    async getAllByRenderProperty() {
        try {

            const token = localStorage.getItem('authToken');
            let response = await fetch(`${API_PROPERTY_BASE_URL_V1}/properties-renter`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            let data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get property failed');
            }

            return data;
        } catch (error) {
            console.error('Get property error:', error);
            throw error;
        }
    },

};

// Export the service
window.propertyRenterService = propertyRenterService;

// Initialize navigation on page load
document.addEventListener('DOMContentLoaded', () => {
    console.log('Page loaded, initializing navigation');
}); 