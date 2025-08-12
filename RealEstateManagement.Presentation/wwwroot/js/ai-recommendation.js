/**
 * AI Real Estate Recommendation Frontend JavaScript
 * Hỗ trợ tìm kiếm bất động sản dựa trên vị trí thiết bị
 */

class AIRecommendationApp {
    constructor() {
        this.deviceId = this.generateDeviceId();
        this.currentLocation = null;
        this.isLocationEnabled = false;
        this.init();
    }

    init() {
        this.bindEvents();
        this.checkLocationPermission();
        this.loadSavedPreferences();
    }

    bindEvents() {
        // Location permission button
        const locationBtn = document.getElementById('enable-location');
        if (locationBtn) {
            locationBtn.addEventListener('click', () => this.enableLocation());
        }

        // Search form
        const searchForm = document.getElementById('search-form');
        if (searchForm) {
            searchForm.addEventListener('submit', (e) => this.handleSearch(e));
        }

        // Preference toggles
        const preferenceToggles = document.querySelectorAll('.preference-toggle');
        preferenceToggles.forEach(toggle => {
            toggle.addEventListener('change', () => this.updatePreferences());
        });

        // Radius slider
        const radiusSlider = document.getElementById('radius-slider');
        if (radiusSlider) {
            radiusSlider.addEventListener('input', (e) => {
                document.getElementById('radius-value').textContent = `${e.target.value}km`;
            });
        }
    }

    generateDeviceId() {
        let deviceId = localStorage.getItem('device_id');
        if (!deviceId) {
            deviceId = 'device_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
            localStorage.setItem('device_id', deviceId);
        }
        return deviceId;
    }

    async checkLocationPermission() {
        if ('geolocation' in navigator) {
            try {
                const permission = await navigator.permissions.query({ name: 'geolocation' });
                this.isLocationEnabled = permission.state === 'granted';
                this.updateLocationUI();
            } catch (error) {
                console.log('Permission API not supported');
            }
        }
    }

    async enableLocation() {
        if (!('geolocation' in navigator)) {
            this.showMessage('Trình duyệt của bạn không hỗ trợ định vị', 'error');
            return;
        }

        try {
            const position = await this.getCurrentPosition();
            this.currentLocation = {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            };

            // Update device location on server
            await this.updateDeviceLocation();
            
            this.isLocationEnabled = true;
            this.updateLocationUI();
            this.showMessage('Đã bật định vị thành công!', 'success');
            
            // Auto-fill search form
            this.fillSearchForm();
            
        } catch (error) {
            this.showMessage('Không thể lấy vị trí: ' + error.message, 'error');
        }
    }

    getCurrentPosition() {
        return new Promise((resolve, reject) => {
            navigator.geolocation.getCurrentPosition(resolve, reject, {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 300000 // 5 minutes
            });
        });
    }

    async updateDeviceLocation() {
        try {
            const response = await fetch('/api/devicelocation/update', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    deviceId: this.deviceId,
                    latitude: this.currentLocation.latitude,
                    longitude: this.currentLocation.longitude
                })
            });

            if (!response.ok) {
                throw new Error('Failed to update device location');
            }
        } catch (error) {
            console.error('Error updating device location:', error);
        }
    }

    updateLocationUI() {
        const locationBtn = document.getElementById('enable-location');
        const locationStatus = document.getElementById('location-status');
        
        if (this.isLocationEnabled && this.currentLocation) {
            if (locationBtn) locationBtn.style.display = 'none';
            if (locationStatus) {
                locationStatus.innerHTML = `
                    <span class="badge badge-success">✓ Đã bật định vị</span>
                    <small class="text-muted d-block">
                        Vĩ độ: ${this.currentLocation.latitude.toFixed(6)}<br>
                        Kinh độ: ${this.currentLocation.longitude.toFixed(6)}
                    </small>
                `;
            }
        } else {
            if (locationBtn) locationBtn.style.display = 'block';
            if (locationStatus) {
                locationStatus.innerHTML = `
                    <span class="badge badge-warning">⚠ Chưa bật định vị</span>
                    <small class="text-muted d-block">Bấm nút bên dưới để bật định vị</small>
                `;
            }
        }
    }

    fillSearchForm() {
        if (!this.currentLocation) return;

        // Auto-fill current location
        const latInput = document.getElementById('latitude');
        const lngInput = document.getElementById('longitude');
        
        if (latInput) latInput.value = this.currentLocation.latitude;
        if (lngInput) lngInput.value = this.currentLocation.longitude;
    }

    async handleSearch(e) {
        e.preventDefault();
        
        if (!this.currentLocation) {
            this.showMessage('Vui lòng bật định vị trước khi tìm kiếm', 'warning');
            return;
        }

        const formData = new FormData(e.target);
        const searchData = {
            latitude: this.currentLocation.latitude,
            longitude: this.currentLocation.longitude,
            radiusKm: parseFloat(formData.get('radius') || 10),
            maxResults: parseInt(formData.get('maxResults') || 20),
            propertyType: formData.get('propertyType') || null,
            maxPrice: formData.get('maxPrice') ? parseFloat(formData.get('maxPrice')) : null,
            minPrice: formData.get('minPrice') ? parseFloat(formData.get('minPrice')) : null,
            minBedrooms: formData.get('minBedrooms') ? parseInt(formData.get('minBedrooms')) : null,
            minArea: formData.get('minArea') ? parseFloat(formData.get('minArea')) : null,
            maxArea: formData.get('maxArea') ? parseFloat(formData.get('maxArea')) : null,
            userPreference: this.getSelectedPreference()
        };

        this.showLoading(true);
        
        try {
            const recommendations = await this.getRecommendations(searchData);
            this.displayRecommendations(recommendations);
        } catch (error) {
            this.showMessage('Lỗi khi tìm kiếm: ' + error.message, 'error');
        } finally {
            this.showLoading(false);
        }
    }

    getSelectedPreference() {
        const preferences = document.querySelectorAll('.preference-toggle:checked');
        if (preferences.length > 0) {
            return preferences[0].value;
        }
        return null;
    }

    async getRecommendations(searchData) {
        const response = await fetch('/api/airecommendation/location-based', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(searchData)
        });

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        return await response.json();
    }

    displayRecommendations(recommendations) {
        const resultsContainer = document.getElementById('recommendation-results');
        if (!resultsContainer) return;

        if (!recommendations.properties || recommendations.properties.length === 0) {
            resultsContainer.innerHTML = `
                <div class="alert alert-info">
                    <h5>Không tìm thấy bất động sản phù hợp</h5>
                    <p>Thử điều chỉnh tiêu chí tìm kiếm hoặc mở rộng bán kính tìm kiếm.</p>
                </div>
            `;
            return;
        }

        let html = `
            <div class="recommendation-summary mb-4">
                <h4>Kết quả tìm kiếm</h4>
                <p class="text-muted">
                    Tìm thấy <strong>${recommendations.totalResults}</strong> bất động sản 
                    trong bán kính <strong>${recommendations.searchRadius}km</strong>
                </p>
                <div class="ai-reason alert alert-info">
                    <strong>🤖 AI Analysis:</strong> ${recommendations.recommendationReason}
                </div>
            </div>
        `;

        recommendations.properties.forEach((property, index) => {
            html += this.createPropertyCard(property, index + 1);
        });

        // Add nearby amenities and transportation info
        if (recommendations.nearbyAmenities && recommendations.nearbyAmenities.length > 0) {
            html += `
                <div class="nearby-info mt-4">
                    <h5>Tiện ích gần đó</h5>
                    <div class="row">
                        <div class="col-md-6">
                            <h6>🏥 Tiện ích</h6>
                            <ul class="list-unstyled">
                                ${recommendations.nearbyAmenities.slice(0, 5).map(amenity => 
                                    `<li><i class="fas fa-check text-success"></i> ${amenity}</li>`
                                ).join('')}
                            </ul>
                        </div>
                        <div class="col-md-6">
                            <h6>🚌 Giao thông</h6>
                            <ul class="list-unstyled">
                                ${recommendations.transportationInfo.slice(0, 5).map(info => 
                                    `<li><i class="fas fa-bus text-info"></i> ${info}</li>`
                                ).join('')}
                            </ul>
                        </div>
                    </div>
                </div>
            `;
        }

        resultsContainer.innerHTML = html;
    }

    createPropertyCard(property, rank) {
        const matchScoreColor = this.getScoreColor(property.matchScore);
        
        return `
            <div class="property-card card mb-3" data-property-id="${property.id}">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-3">
                            <img src="${property.primaryImageUrl || '/image/default-property.jpg'}" 
                                 class="img-fluid rounded" alt="${property.title}">
                        </div>
                        <div class="col-md-6">
                            <div class="d-flex justify-content-between align-items-start">
                                <h5 class="card-title">#${rank} ${property.title}</h5>
                                <span class="badge badge-${matchScoreColor}">
                                    ${property.matchScore.toFixed(0)}% phù hợp
                                </span>
                            </div>
                            <p class="card-text">${property.description}</p>
                            <div class="property-details">
                                <span class="badge badge-secondary mr-2">${property.type}</span>
                                <span class="badge badge-info mr-2">${property.area}m²</span>
                                <span class="badge badge-primary mr-2">${property.bedrooms} phòng ngủ</span>
                                <span class="badge badge-success">${property.price} triệu/tháng</span>
                            </div>
                            <div class="location-info mt-2">
                                <small class="text-muted">
                                    📍 ${property.detailedAddress}, ${property.ward}, ${property.province}
                                </small>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="distance-info text-center">
                                <div class="distance-circle">
                                    <strong>${property.distanceKm.toFixed(1)}km</strong>
                                </div>
                                <small class="text-muted">
                                    🚶 ${property.walkingTimeMinutes} phút đi bộ<br>
                                    🚗 ${property.drivingTimeMinutes} phút lái xe
                                </small>
                            </div>
                            <div class="match-reason mt-2">
                                <small class="text-muted">${property.matchReason}</small>
                            </div>
                            <button class="btn btn-primary btn-sm mt-2 w-100" 
                                    onclick="viewPropertyDetails(${property.id})">
                                Xem chi tiết
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    getScoreColor(score) {
        if (score >= 90) return 'success';
        if (score >= 80) return 'info';
        if (score >= 70) return 'warning';
        return 'secondary';
    }

    showLoading(show) {
        const loadingElement = document.getElementById('loading');
        if (loadingElement) {
            loadingElement.style.display = show ? 'block' : 'none';
        }
    }

    showMessage(message, type = 'info') {
        const messageContainer = document.getElementById('message-container');
        if (!messageContainer) return;

        const alertClass = `alert-${type}`;
        const icon = this.getMessageIcon(type);
        
        messageContainer.innerHTML = `
            <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
                ${icon} ${message}
                <button type="button" class="close" data-dismiss="alert">
                    <span>&times;</span>
                </button>
            </div>
        `;

        // Auto-hide after 5 seconds
        setTimeout(() => {
            const alert = messageContainer.querySelector('.alert');
            if (alert) {
                alert.remove();
            }
        }, 5000);
    }

    getMessageIcon(type) {
        switch (type) {
            case 'success': return '✅';
            case 'error': return '❌';
            case 'warning': return '⚠️';
            default: return 'ℹ️';
        }
    }

    loadSavedPreferences() {
        const savedRadius = localStorage.getItem('search_radius');
        const savedPreference = localStorage.getItem('user_preference');
        
        if (savedRadius) {
            const radiusSlider = document.getElementById('radius-slider');
            if (radiusSlider) {
                radiusSlider.value = savedRadius;
                document.getElementById('radius-value').textContent = `${savedRadius}km`;
            }
        }
        
        if (savedPreference) {
            const preferenceToggle = document.querySelector(`input[value="${savedPreference}"]`);
            if (preferenceToggle) {
                preferenceToggle.checked = true;
            }
        }
    }

    updatePreferences() {
        const radiusSlider = document.getElementById('radius-slider');
        const preferenceToggle = document.querySelector('.preference-toggle:checked');
        
        if (radiusSlider) {
            localStorage.setItem('search_radius', radiusSlider.value);
        }
        
        if (preferenceToggle) {
            localStorage.setItem('user_preference', preferenceToggle.value);
        }
    }
}

// Global function for property details
function viewPropertyDetails(propertyId) {
    // Redirect to property detail page
    window.location.href = `/Property/Details/${propertyId}`;
}

// Initialize app when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.aiRecommendationApp = new AIRecommendationApp();
});

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = AIRecommendationApp;
} 