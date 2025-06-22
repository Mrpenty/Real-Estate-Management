const API_PROPERTY_BASE_URL = 'https://localhost:7031/api/Property';

const propertyService = {

    async getAllproperty() {
        try {
            const urlParams = new URLSearchParams(window.location.search);

            const provinceId = urlParams.get('province') ?? 0;
            const wardId = urlParams.get('ward') ?? 0;
            const streetId = urlParams.get('street') ?? 0;
            const filterCategory = urlParams.get('filterCategory');
            const filterPrice = urlParams.get('filterPrice');
            const filterArea = urlParams.get('filterArea');
            const filterAmenity = urlParams.get('filterAmenity');
            let isFilter = urlParams.get('isFilter') ?? false;
            let data = null;
            let response = null;

            isFilter = Boolean(isFilter);

            //console.log({
            //    provinceId,
            //    wardId,
            //    streetId,
            //    filterCategory,
            //    filterPrice,
            //    filterArea,
            //    filterAmenity,
            //    isFilter
            //});

            if (!isFilter) {

                response = await fetch(`${API_PROPERTY_BASE_URL}/search-advanced?province=${provinceId}&ward=${wardId}&street=${streetId}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    }
                });

                data = await response.json();
            }
            else {
                let arrPrice = filterPrice.split(',');
                let arrArea = filterArea.split(',');
                let body = {
                    scopePrice: arrPrice[0],
                    minPrice: arrPrice[1],
                    maxPrice: arrPrice[2],
                    scopeArea: arrArea[0],
                    minArea: arrArea[1],
                    maxArea: arrArea[2],
                    amenityName: filterAmenity.split(",") ?? [],
                    location: `${provinceId ?? 0},${wardId ?? 0},${streetId ?? 0}`
                }
                response = await fetch(`${API_PROPERTY_BASE_URL}/filter-advanced`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(body)
                });

                data = await response.json();
            }

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

            const response = await fetch(`${API_PROPERTY_BASE_URL}/${id}`, {
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