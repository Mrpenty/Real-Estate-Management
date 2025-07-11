const API_PROPERTY_BASE_URL = 'https://localhost:7031/api/Property';

const propertyService = {

    async getAllproperty() {
        try {

            const urlParams = new URLSearchParams(window.location.search);
            let type = urlParams.get('type');
            if (type == null || type == undefined) type = "room";

            let listLocationSelected = sessionStorage.getItem('selectedLocationLists');
            let provinceId = sessionStorage.getItem('provinceId');
            let provinces = [];
            let wards = [];
            let streets = [];
            if (listLocationSelected != null && listLocationSelected != undefined) {
                listLocationSelected = JSON.parse(listLocationSelected);
                if (listLocationSelected.length != 0) {
                    listLocationSelected.forEach(item => {
                        provinces.push(Number(item.province.id));
                        wards.push(item.ward.id);
                        streets.push(item.street.id);
                    });
                }
                else {
                    provinces.push(Number(provinceId));
                }

            }
            //else provinces.push(provinceId);

            const token = localStorage.getItem('authToken');
            let userId = 0;
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                userId = payload.id;
            }

            let body = sessionStorage.getItem('filterData');
            if (body != null && body != undefined) {
                body = JSON.parse(body);
            }
            else body = {};
            body['provinces'] = provinces;
            body['wards'] = wards;
            body['streets'] = streets;
            body['type'] = type;
            body['userId'] = userId;

            $('#minInputPrice').val(body.minPrice ?? 0);
            $('#maxInputPrice').val(body.maxPrice ?? 100);

            $('#minInputArea').val(body.minArea ?? 0);
            $('#maxInputArea').val(body.maxArea ?? 100);

            $('#minInput').val(body.minRoom ?? 0);
            $('#maxInput').val(body.maxRoom ?? 15);

            let response = await fetch(`${API_PROPERTY_BASE_URL}/filter-advanced`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(body)
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

    async getProperty(id) {
        try {
            console.log(id)
            const token = localStorage.getItem('authToken');
            let userId = 0;
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                userId = payload.id;
            }

            const response = await fetch(`${API_PROPERTY_BASE_URL}/${id}?userId=${userId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            const data = await response.json();

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
window.propertyService = propertyService;

// Initialize navigation on page load
document.addEventListener('DOMContentLoaded', () => {
    console.log('Page loaded, initializing navigation');
}); 