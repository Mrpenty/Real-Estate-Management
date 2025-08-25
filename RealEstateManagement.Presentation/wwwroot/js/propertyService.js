var API_PROPERTY_BASE_URL3 = 'https://localhost:7031/api/Property';
var API_PROPERTY_BASE_URL2 = 'https://localhost:7031/api/OwnerProperty';

const propertyService = {
    async getAllproperty() {
        try {
            // Remove type filtering completely - always show all properties
            // const urlParams = new URLSearchParams(window.location.search);
            // let type = urlParams.get('type');

            let listLocationSelected = sessionStorage.getItem('selectedLocationLists');
            let provinceId = sessionStorage.getItem('provinceId');
            let provinces = [];
            let wards = [];
            let streets = [];
            if (listLocationSelected != null && listLocationSelected != undefined && listLocationSelected != '') {
                listLocationSelected = JSON.parse(listLocationSelected);
                if (listLocationSelected.length != 0) {
                    listLocationSelected.forEach(item => {
                        provinces.push(Number(item.province.id));
                        wards.push(item.ward.id);
                        streets.push(item.street.id);
                    });
                } else {
                    provinces.push(Number(provinceId));
                }
            }
            else if (provinceId) provinces.push(Number(provinceId));

            const token = localStorage.getItem('authToken');
            let userId = 0;
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                userId = payload.id;
            }

            let body = sessionStorage.getItem('filterData');
            if (body != null && body != undefined) {
                body = JSON.parse(body);
            } else {
                body = {};
            }
            var pageNo = sessionStorage.getItem('pageNo') ?? 1;

            // Build query parameters - NO TYPE parameter, show all properties
            const queryParams = new URLSearchParams({
                page: Number(pageNo),
                pageSize: 5,
                provinces: provinces.join(','),
                wards: wards.join(','),
                streets: streets.join(','),
                userId: userId,
                minPrice: body.minPrice ?? 0,
                maxPrice: body.maxPrice ?? 100,
                minArea: body.minArea ?? 0,
                maxArea: body.maxArea ?? 100,
                minRoom: body.minRoom ?? 0,
                maxRoom: body.maxRoom ?? 15
            });
            //console.log("====", queryParams);
            // Do NOT add type parameter - always show all types

            let response = await fetch(`${API_PROPERTY_BASE_URL3}/homepage-paginated?${queryParams.toString()}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    'Authorization': token ? `Bearer ${token}` : ''
                }
            });

            let data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get property failed');
            }
            //console.log(data);
            if (!data.hasNextPage) $('#simNext1').attr("disabled", true);
            if (!data.hasPreviousPage) $('#simPrev1').attr("disabled", true);
            $('#simInfoIndex1').html(data.page + " / " + data.totalPages);
            return data.data; // Trả về mảng data từ phản hồi
        } catch (error) {
            console.error('Get property error:', error);
            throw error;
        }
    },

    async getProperty(id) {
        try {
            const token = localStorage.getItem('authToken');
            let userId = 0;
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                userId = payload.id;
            }

            const response = await fetch(`${API_PROPERTY_BASE_URL3}/${id}?userId=${userId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            const data = await response.json();
            //console.log(data);
            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get property failed');
            }

            return data;
        } catch (error) {
            console.error('Get property error:', error);
            throw error;
        }
    },

    async getOwnerProperty(id) {
        try {
            const token = localStorage.getItem('authToken');
            let userId = 0;
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                userId = payload.id;
            }

            const response = await fetch(`${API_PROPERTY_BASE_URL2}/${id}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    ...(token && { 'Authorization': `Bearer ${token}` })
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

    async getNewPost() {
        try {

            const response = await fetch(`https://localhost:7031/api/News/published`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get failed');
            }

            return data;
        } catch (error) {
            console.error('Get error:', error);
            throw error;
        }
    },

    async getNewPostDetail(id) {
        try {

            const response = await fetch(`https://localhost:7031/api/News/${id}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get failed');
            }

            return data;
        } catch (error) {
            console.error('Get error:', error);
            throw error;
        }
    },


    async getLandlord(landlordId) {
        try {
            const token = localStorage.getItem('authToken');
            const response = await fetch(`https://localhost:7031/api/Property/${landlordId}/profile-with-properties`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    'Authorization': token ? `Bearer ${token}` : ''
                }
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || data.errorMessage || 'Get failed');
            }

            return data;
        } catch (error) {
            console.error('Get error:', error);
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