const API_PROPERTY_BASE_URL_V1 = 'http://194.233.81.64:5000/api/Property';
const API_PROPERTY_BASE_URL_V2 = 'http://194.233.81.64:5000/api/OwnerProperty';
//
//const API_BASE = '/api';
//const API = {
//    property: `${API_BASE}/Property`,
//    ownerProperty: `${API_BASE}/OwnerProperty`,
//};
const propertyRenterService = {

    async getAllBylandlordProperty() {
        try {

            const token = localStorage.getItem('authToken');
            let response = await fetch(`${API_PROPERTY_BASE_URL_V2}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            let data = await response.json();

            console.log(data);

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get property failed');
            }

            return data;
        } catch (error) {
            console.error('Get property error:', error);
            throw error;
        }
    },

    async deleteProperty(id){
        try {
            if (!confirm("Bạn có muốn xóa ?")) return;
            const token = localStorage.getItem('authToken');
            let response = await fetch(`${API_PROPERTY_BASE_URL_V2}/DeleteProperty?id=${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            //let data = response.json();
            //console.log(data);
            alert("Xóa thành công");
            window.location.reload();
        } catch (error) {
            console.error('Get property error:', error);
            throw error;
        }
    }
};

// Export the service
window.propertyRenterService = propertyRenterService;

// Initialize navigation on page load
document.addEventListener('DOMContentLoaded', () => {
    console.log('Page loaded, initializing navigation');
}); 