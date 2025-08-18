const AISearchService = {
    /**
     * Tìm kiếm bất động sản với AI recommendation
     * @param {Object} searchCriteria - Tiêu chí tìm kiếm
     * @returns {Promise} - Kết quả tìm kiếm
     */
    async searchWithAI(searchCriteria) {
        try {
            const response = await fetch(config.buildApiUrl(config.ai.searchByCriteria), {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(searchCriteria)
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Lỗi tìm kiếm: ${errorText}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Lỗi khi gọi AI Search API:', error);
            throw error;
        }
    },

    /**
     * Lấy thông tin tiện ích chi tiết
     * @param {number} latitude - Vĩ độ
     * @param {number} longitude - Kinh độ
     * @param {number} radiusKm - Bán kính tìm kiếm
     * @returns {Promise} - Danh sách tiện ích
     */
    async getDetailedAmenities(latitude, longitude, radiusKm = 2) {
        try {
            const response = await fetch(`${config.buildApiUrl(config.ai.detailedAmenities)}?latitude=${latitude}&longitude=${longitude}&radiusKm=${radiusKm}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thông tin tiện ích');
            }

            return await response.json();
        } catch (error) {
            console.error('Lỗi khi lấy tiện ích:', error);
            throw error;
        }
    },

    /**
     * Lấy thông tin giao thông
     * @param {number} latitude - Vĩ độ
     * @param {number} longitude - Kinh độ
     * @returns {Promise} - Thông tin giao thông
     */
    async getTransportationInfo(latitude, longitude) {
        try {
            const response = await fetch(`${config.buildApiUrl(config.ai.transportationInfo)}?latitude=${latitude}&longitude=${longitude}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Không thể lấy thông tin giao thông');
            }

            return await response.json();
        } catch (error) {
            console.error('Lỗi khi lấy thông tin giao thông:', error);
            throw error;
        }
    },

    /**
     * Tính khoảng cách giữa hai điểm
     * @param {number} lat1 - Vĩ độ điểm 1
     * @param {number} lon1 - Kinh độ điểm 1
     * @param {number} lat2 - Vĩ độ điểm 2
     * @param {number} lon2 - Kinh độ điểm 2
     * @returns {Promise} - Khoảng cách
     */
    async calculateDistance(lat1, lon1, lat2, lon2) {
        try {
            const response = await fetch(`${config.buildApiUrl(config.ai.calculateDistance)}?latitude=${lat1}&longitude=${lon1}&lat2=${lat2}&lon2=${lon2}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Không thể tính khoảng cách');
            }

            return await response.json();
        } catch (error) {
            console.error('Lỗi khi tính khoảng cách:', error);
            throw error;
        }
    },

    /**
     * Lưu lịch sử tìm kiếm
     * @param {Object} searchData - Dữ liệu tìm kiếm
     */
    saveSearchHistory(searchData) {
        try {
            const searchHistory = JSON.parse(localStorage.getItem('aiSearchHistory') || '[]');
            searchHistory.unshift({
                ...searchData,
                timestamp: new Date().toISOString()
            });
            
            // Giữ tối đa 10 lịch sử
            if (searchHistory.length > 10) {
                searchHistory.splice(10);
            }
            
            localStorage.setItem('aiSearchHistory', JSON.stringify(searchHistory));
        } catch (error) {
            console.error('Lỗi khi lưu lịch sử tìm kiếm:', error);
        }
    },

    /**
     * Lấy lịch sử tìm kiếm
     * @returns {Array} - Danh sách lịch sử
     */
    getSearchHistory() {
        try {
            return JSON.parse(localStorage.getItem('aiSearchHistory') || '[]');
        } catch (error) {
            console.error('Lỗi khi lấy lịch sử tìm kiếm:', error);
            return [];
        }
    },

    /**
     * Xóa lịch sử tìm kiếm
     */
    clearSearchHistory() {
        try {
            localStorage.removeItem('aiSearchHistory');
        } catch (error) {
            console.error('Lỗi khi xóa lịch sử tìm kiếm:', error);
        }
    }
};

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = AISearchService;
} else {
    window.AISearchService = AISearchService;
} 