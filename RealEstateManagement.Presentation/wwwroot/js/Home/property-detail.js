// Property Detail Page JavaScript
class PropertyDetailManager {
    constructor() {
        this.urlBase = "https://localhost:7031";
        this.currentId = null;
        this.landlordId = 0;
        this.currentUserId = 0;
        this.isOwner = false;
        this.propertyService = window.propertyService;
        this.reportService = null; // Will be initialized when needed

        // Check if required services are available
        if (!this.propertyService) {
            console.error('PropertyService not available');
        }

        if (!window.ReportService) {
            console.error('ReportService class not available');
        }

        this.init();
    }

    init() {
        // Get ID from ViewBag
        this.currentId = window.currentPropertyId;
        if (this.currentId) {
            this.initializeCurrentUserId();
            this.loadProperty();
        } else {
            this.showErrorMessage('Không tìm thấy ID bất động sản');
        }
    }

    getCurrentUserIdFromToken() {
        try {
            let token = localStorage.getItem('authToken');
            if (!token) {
                token = localStorage.getItem('accessToken');
            }
            if (!token) {
                token = localStorage.getItem('token');
            }

            if (!token) {
                return 0;
            }

            try {
                const payload = JSON.parse(atob(token.split('.')[1]));
                const userId = payload.id || payload.userId || payload.sub;
                return userId || 0;
            } catch (e) {
                console.error('Error decoding token:', e);
                return 0;
            }
        } catch (error) {
            console.error('Error getting current user ID:', error);
            return 0;
        }
    }

    initializeCurrentUserId() {
        this.currentUserId = this.getCurrentUserIdFromToken();
        console.log('Initialized currentUserId:', this.currentUserId);

        this.hideReportButtonForOwner();

        if (this.currentUserId > 0) {
            this.checkReportStatus();
        } else {
            this.updateReportButtonForGuest();
        }

        // Listen for token changes
        window.addEventListener('storage', (e) => {
            if (e.key === 'authToken' || e.key === 'accessToken' || e.key === 'token') {
                this.refreshCurrentUserId();
            }
        });

        window.addEventListener('userLogin', () => this.refreshCurrentUserId());
        window.addEventListener('userLogout', () => this.refreshCurrentUserId());
    }

    refreshCurrentUserId() {
        const previousUserId = this.currentUserId;
        this.currentUserId = this.getCurrentUserIdFromToken();

        if (this.currentUserId !== previousUserId) {
            this.hideReportButtonForOwner();

            if (this.currentUserId > 0) {
                this.checkReportStatus();
            } else {
                this.updateReportButtonForGuest();
            }
        }
    }

    hideReportButtonForOwner() {
        const reportBtn = document.getElementById('reportBtn');
        if (!reportBtn) {
            console.error('Report button not found');
            return;
        }

        const currentUserIdNum = Number(this.currentUserId);
        const landlordIdNum = Number(this.landlordId);

        console.log('Checking owner status:', { currentUserId: currentUserIdNum, landlordId: landlordIdNum });

        if (currentUserIdNum > 0 && currentUserIdNum === landlordIdNum) {
            console.log('Hiding report button for owner');
            reportBtn.style.display = 'none';
        } else {
            console.log('Showing report button for non-owner');
            reportBtn.style.display = 'flex';
        }
    }

    updateReportButtonForGuest() {
        const reportBtn = document.getElementById('reportBtn');
        const reportBtnText = document.getElementById('reportBtnText');

        if (!reportBtn || !reportBtnText) {
            console.error('Report button elements not found');
            return;
        }

        console.log('Updating report button for guest');

        // Always show the report button for guests
        reportBtn.style.display = 'flex';

        reportBtnText.textContent = 'Báo xấu';
        reportBtn.onclick = () => {
            Swal.fire({
                icon: 'info',
                title: 'Vui lòng đăng nhập',
                text: 'Bạn cần đăng nhập để có thể báo cáo bài đăng này.',
                confirmButtonText: 'Đăng nhập',
                showCancelButton: true,
                cancelButtonText: 'Hủy'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/Auth/Login';
                }
            });
        };

        // Remove all color classes and let CSS handle styling
        reportBtn.className = 'action-btn report';
    }

    checkUserLogin() {
        if (this.currentUserId <= 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Vui lòng đăng nhập',
                text: 'Bạn cần đăng nhập để có thể báo cáo. Vui lòng đăng nhập và thử lại.',
                confirmButtonText: 'Đồng ý',
                showCancelButton: true,
                cancelButtonText: 'Hủy'
            });
            return false;
        }
        return true;
    }

    async loadProperty() {
        try {
            this.showLoadingState();

            const prop = await this.propertyService.getProperty(this.currentId);
            console.log('Property loaded:', prop);

            this.landlordId = prop.landlordId;
            console.log('Landlord ID set to:', this.landlordId);

            this.updateUI(prop);
            await this.loadSidebarNews();
            this.loadSimilarProperties();

            // Check owner status
            const token = localStorage.getItem('authToken');
            if (token) {
                try {
                    const payload = JSON.parse(atob(token.split('.')[1]));
                    const userId = payload.id;
                    this.isOwner = prop.landlordId === userId;
                    console.log('Owner status:', { userId, landlordId: prop.landlordId, isOwner: this.isOwner });
                } catch (e) {
                    console.error('Error parsing token:', e);
                }
            }

            // Update report button after everything is loaded
            this.hideReportButtonForOwner();

            // Check report status if user is logged in
            if (this.currentUserId > 0) {
                await this.checkReportStatus();
            } else {
                this.updateReportButtonForGuest();
            }

            // Nếu có renterId thì check quan tâm
            if (this.currentUserId) {
                this.checkInterestAndRender(prop.id, this.currentUserId, prop);
            }

        } catch (error) {
            console.error('Error loading property:', error);
            this.showErrorMessage('Có lỗi xảy ra khi tải thông tin bất động sản');
        } finally {
            this.hideLoadingState();
        }
    }

    updateUI(prop) {
        // Update title and rating
        this.fullPhoneNumber = prop.landlordPhoneNumber || 'N/A';
        this.isInterested = false;
        this.updateElement('.titleId', prop.title);
        this.renderStarsBody(prop.rating);
        this.updateElement('#avgStar', `(${prop.rating})`);
        this.updateElement('#totalCommentCount', prop.commentNo || 0);

        // Update landlord info with null check
        if (prop.landlordProfilePictureUrl) {
            const landlordImageUrl = prop.landlordProfilePictureUrl.includes('http')
                ? prop.landlordProfilePictureUrl
                : this.urlBase + prop.landlordProfilePictureUrl;
            $('.landlordUrl').prop('src', landlordImageUrl);
        } else {
            // Set default avatar if landlordProfilePictureUrl is null/undefined
            $('.landlordUrl').prop('src', '/image/default-avatar.jpg');
            console.warn('Landlord profile picture URL is null/undefined, using default avatar');
        }

        // Update primary image with null check
        if (prop.primaryImageUrl) {
            const primaryImageUrl = prop.primaryImageUrl.includes("http")
                ? prop.primaryImageUrl
                : this.urlBase + prop.primaryImageUrl;
            $('#imgPrimaryUrl').prop('src', primaryImageUrl);
        } else {
            // Set default image if primaryImageUrl is null/undefined
            $('#imgPrimaryUrl').prop('src', '/image/default-property.jpg');
            console.warn('Primary image URL is null/undefined, using default image');
        }

        // Update property details with null checks
        this.updateElement('.areaId', prop.area || 'N/A');
        this.updateElement('.addressId', this.buildAddressString(prop));
        this.updateElement('.priceId', this.formatPrice(prop.price || 0));
        this.updateElement('.timeAgoId', this.timeAgo(prop.createdAt));
        this.updateElement('.descriptionId', prop.description || 'Không có mô tả');
        this.updateElement('.wardId', prop.ward || 'N/A');
        this.updateElement('.provinceId', prop.province || 'N/A');
        this.updateElement('#propId', prop.id || 'N/A');
        this.updateElement('#createTimeId', this.formatDateTime(prop.createdAt));
        this.updateElement('.contactName', prop.landlordName || 'Không xác định');

        // Handle favorite buttons
        this.handleFavoriteButtons(prop.isFavorite);

        // Populate image thumbnails with null check
        if (prop.imageUrls && Array.isArray(prop.imageUrls)) {
            this.populateImageThumbnails(prop.imageUrls);
        } else {
            console.warn('Image URLs is null/undefined or not an array, skipping thumbnails');
            $('#listImgs').html('<div class="text-gray-500 text-center py-4">Không có hình ảnh</div>');
        }
        // Update map
        this.updateMap(prop);

        // Load comments - handled by ReviewManager
        // Wait for ReviewManager to be initialized
        setTimeout(() => {
            this.loadCommentsWithRetry();
        }, 100);
    }

    // Hàm che số
    maskPhone(phone) {
        if (phone === 'N/A') return phone;
        let half = Math.floor(phone.length / 2);
        return phone.substring(0, half) + "*".repeat(phone.length - half);
    }

    // Render số bị che
    renderMaskedPhone() {
        let masked = this.maskPhone(this.fullPhoneNumber);
        $('.contactPhone').text(masked);
        $('#phoneLink').attr('href', ''); // Không cho click gọi
        $('#zaloLink').attr('href', '#');
    }

    async checkInterestAndRender(propertyId, renterId, propertyData) {
        try {
            if (!renterId) {
                alert("Bạn cần đăng nhập để sử dụng tính năng này!");
                this.isInterested = false;
                this.renderMaskedPhone();
                $(".btn-add-interest").show().off("click").on("click", () => {
                    alert("Bạn cần đăng nhập để quan tâm!");
                });
                $(".btn-remove-interest, .btn-view-contract, .btn-agree-rent, .waiting-landlord").hide();
                $("#contract-section").hide();
                return;
            }

            // Lấy danh sách quan tâm
            let res = await fetch(`${this.urlBase}/api/Property/InterestedProperty/ByRenter/${renterId}`, { credentials: "include" });
            if (!res.ok) throw new Error("API get interest failed");
            let interestList = await res.json();

            // Tìm bản ghi interest
            let foundInterest = interestList.find(item => item.propertyId === propertyId);

            if (foundInterest) {
                this.isInterested = true;
                this.showFullPhone();
                $(".btn-add-interest").hide();
                $(".btn-remove-interest").show();
                $(".btn-view-contract").show();

                if (foundInterest.status === 2) {
                    $(".btn-agree-rent").show();
                    $(".waiting-landlord").hide();
                } else if (foundInterest.status === 3) {
                    $(".btn-agree-rent").hide();
                    $(".waiting-landlord").show().text("Đang chờ Landlord phản hồi...");
                } else {
                    $(".btn-agree-rent").hide();
                    $(".waiting-landlord").hide();
                }
            } else {
                this.isInterested = false;
                this.renderMaskedPhone();
                $(".btn-add-interest").show();
                $(".btn-remove-interest, .btn-view-contract, .btn-agree-rent, .waiting-landlord").hide();
                $("#contract-section").hide();
            }

            // Sự kiện Quan tâm (dùng propertyId)
            $(".btn-add-interest").off("click").on("click", async () => {
                try {
                    let apiUrl = `${this.urlBase}/api/Property/InterestedProperty/AddInterest?propertyId=${propertyId}`;
                    let toggleRes = await fetch(apiUrl, { method: "POST", credentials: "include" });
                    if (!toggleRes.ok) throw new Error(`API add interest failed: ${toggleRes.status}`);
                    location.reload();
                } catch (err) {
                    console.error("Error adding interest:", err);
                    alert("Có lỗi xảy ra khi thêm quan tâm!");
                }
            });

            // Sự kiện Hủy quan tâm (dùng interestId)
            $(".btn-remove-interest").off("click").on("click", async () => {
                try {
                    if (!foundInterest) return;
                    let apiUrl = `${this.urlBase}/api/Property/InterestedProperty/RemoveInterest?propertyId=${propertyId}`;
                    let toggleRes = await fetch(apiUrl, { method: "DELETE", credentials: "include" });
                    if (!toggleRes.ok) throw new Error(`API remove interest failed: ${toggleRes.status}`);
                    location.reload();
                } catch (err) {
                    console.error("Error removing interest:", err);
                    alert("Có lỗi xảy ra khi hủy quan tâm!");
                }
            });

            // Xem hợp đồng
            $(".btn-view-contract").off("click").on("click", () => {
                const rc = {
                    DepositAmount: propertyData.contractDeposit,
                    MonthlyRent: propertyData.contractMonthlyRent,
                    ContractDurationMonths: propertyData.contractDurationMonths,
                    StartDate: propertyData.contractStartDate,
                    EndDate: propertyData.contractEndDate,
                    Status: propertyData.contractStatus,
                    PaymentMethod: propertyData.contractPaymentMethod,
                    ContactInfo: propertyData.contractContactInfo
                };
                if (!rc.DepositAmount && !rc.MonthlyRent) {
                    alert("Không có dữ liệu hợp đồng!");
                    return;
                }
                $("#ContractDeposit").text(rc.DepositAmount || "N/A");
                $("#ContractMonthlyRent").text(rc.MonthlyRent || "N/A");
                $("#ContractDurationMonths").text(rc.ContractDurationMonths || "N/A");
                $("#ContractStartDate").text(rc.StartDate || "N/A");
                $("#ContractEndDate").text(rc.EndDate || "N/A");
                $("#ContractStatus").text(rc.Status || "N/A");
                $("#ContractPaymentMethod").text(rc.PaymentMethod || "N/A");
                $("#ContractContactInfo").text(rc.ContactInfo || "N/A");

                $("#contract-section").slideToggle();
            });

            // Tôi muốn thuê
            $(".btn-agree-rent").off("click").on("click", async () => {
                if (!foundInterest) return;
                if (!confirm("Bạn chắc chắn muốn thuê chứ?")) return;
                try {
                    let interestId = foundInterest.id;
                    let apiUrl = `${this.urlBase}/api/Property/InterestedProperty/${interestId}/confirm?isRenter=true&confirmed=true`;
                    let res = await fetch(apiUrl, { method: "POST", credentials: "include" });
                    if (!res.ok) throw new Error(await res.text());
                    let data = await res.json();
                    alert(data.message || "Xác nhận thành công!");
                    location.reload();
                } catch (err) {
                    console.error("Error confirming rent:", err);
                    alert("Có lỗi xảy ra khi xác nhận thuê: " + err.message);
                }
            });

        } catch (err) {
            console.error("Error checking interest:", err);
            this.isInterested = false;
            this.renderMaskedPhone();
            $(".btn-add-interest").show();
            $(".btn-remove-interest, .btn-view-contract, .btn-agree-rent, .waiting-landlord").hide();
        }
    }

    showFullPhone() {
        $('.contactPhone').text(this.fullPhoneNumber);
        $('#phoneLink').attr('href', 'tel:' + this.fullPhoneNumber);
        $('#zaloLink').attr('href', 'https://zalo.me/' + this.fullPhoneNumber);
    }

    updateElement(selector, value) {
        const element = $(selector);
        if (element.length && value !== undefined) {
            element.html(value);
        }
    }

    buildAddressString(prop) {
        const parts = [
            prop.detailedAddress || '',
            prop.street || '',
            prop.ward || '',
            prop.province || ''
        ].filter(part => part && part.trim() !== '');

        if (parts.length === 0) {
            return 'Địa chỉ không xác định';
        }

        return parts.join(', ');
    }

    formatPrice(price) {
        if (!price || isNaN(price)) {
            return 'Liên hệ';
        }

        if (typeof formatVietnameseNumber === 'function') {
            return formatVietnameseNumber(price) + " đ";
        }
        return price.toLocaleString('vi-VN') + " đ";
    }

    formatDateTime(dateString) {
        if (typeof formatVietnameseDateTime === 'function') {
            return formatVietnameseDateTime(dateString);
        }
        return new Date(dateString).toLocaleDateString('vi-VN');
    }

    handleFavoriteButtons(isFavorite) {
        console.log('Handling favorite buttons, isFavorite:', isFavorite);

        // Hide both buttons first
        $('#addFavoriteDetail').addClass('d-none');
        $('#removeFavoriteDetail').addClass('d-none');

        // Show the appropriate button based on favorite status
        if (isFavorite) {
            console.log('Property is favorited, showing remove button');
            $('#removeFavoriteDetail').removeClass('d-none');
        } else {
            console.log('Property is not favorited, showing add button');
            $('#addFavoriteDetail').removeClass('d-none');
        }
    }

    // Handle adding to favorites
    async addToFavorites() {
        try {
            console.log('Adding property to favorites:', this.currentId);

            // Call the global function
            if (typeof addToFavourite === 'function') {
                await addToFavourite(this.currentId);

                // Update UI to show remove button
                this.handleFavoriteButtons(true);

                // Show success message
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Đã thêm vào danh sách yêu thích',
                    confirmButtonText: 'Đồng ý'
                });
            } else {
                console.error('addToFavourite function not available');
            }
        } catch (error) {
            console.error('Error adding to favorites:', error);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Không thể thêm vào danh sách yêu thích',
                confirmButtonText: 'Đồng ý'
            });
        }
    }

    // Handle removing from favorites
    async removeFromFavorites() {
        try {
            console.log('Removing property from favorites:', this.currentId);

            // Call the global function
            if (typeof removeToFavorite === 'function') {
                await removeToFavorite(this.currentId);

                // Update UI to show add button
                this.handleFavoriteButtons(false);

                // Show success message
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Đã xóa khỏi danh sách yêu thích',
                    confirmButtonText: 'Đồng ý'
                });
            } else {
                console.error('removeToFavorite function not available');
            }
        } catch (error) {
            console.error('Error removing from favorites:', error);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Không thể xóa khỏi danh sách yêu thích',
                confirmButtonText: 'Đồng ý'
            });
        }
    }

    populateImageThumbnails(imageUrls) {
        const container = $('#listImgs');
        container.empty();

        if (imageUrls && imageUrls.length > 0) {
            let html = '';

            imageUrls.forEach((url, index) => {
                const src = url.includes("http") ? url : this.urlBase + url;
                const isActive = index === 0 ? 'active' : '';

                html += `<div onclick="propertyDetailManager.selectImage(this)" class="thumbnail-item ${isActive}">
                            <img src="${src}" onerror="propertyDetailManager.handleImageError(this)" class="thumbnail-image" alt="Thumbnail ${index + 1}" />
                        </div>`;
            });

            container.html(html);
        } else {
            container.html('<div class="text-gray-500 text-center py-4">Không có hình ảnh</div>');
        }
    }

    updateMap(prop) {
        const mapFrame = document.getElementById('mapFrame');
        if (!mapFrame) return;

        if (prop.location) {
            const coordinates = prop.location.split(',');
            const lat = coordinates[0];
            const lng = coordinates[1];
            mapFrame.src = `https://www.google.com/maps?q=${lat},${lng}&output=embed`;
        } else {
            const addressInfo = this.buildAddressString(prop);
            const encodedLocation = encodeURIComponent(addressInfo);
            mapFrame.src = `https://www.google.com/maps?q=${encodedLocation}&output=embed`;
        }
    }

    async loadSidebarNews() {
        try {
            const newsList = await this.propertyService.getNewPost();
            this.populateSidebarNews(newsList);
        } catch (error) {
            console.error('Error loading sidebar news:', error);
            $('#listNew').html('<div class="text-gray-500 text-center py-4">Không thể tải tin mới</div>');
        }
    }

    populateSidebarNews(newsList) {
        const container = $('#listNew');
        let html = '';

        newsList.forEach(item => {
            const imageUrl = item.images && item.images.length > 0 && item.images[0] && item.images[0].imageUrl
                ? (item.images[0].imageUrl.includes('http') ? item.images[0].imageUrl : this.urlBase + item.images[0].imageUrl)
                : '/image/default-news.jpg';

            html += `<div class="news-item" onclick="window.location.href='/Home/NewDetail/${item.id}'">
                        <img src="${imageUrl}" onerror="propertyDetailManager.handleImageError(this)" alt="tin" class="news-item-image" />
                        <div class="news-item-content">
                            <div class="news-item-title">
                                ${item.title || 'Không có tiêu đề'}
                            </div>
                            <div class="news-item-time">${this.timeAgo(item.publishedAt)}</div>
                        </div>
                    </div>`;
        });

        container.html(html);
    }

    async loadSimilarProperties() {
        try {
            await this.loadSimilar(1);
        } catch (error) {
            console.error('Error loading similar properties:', error);
        }
    }

    selectImage(element) {
        // Remove active class from all thumbnails
        document.querySelectorAll('.thumbnail-item').forEach(el => {
            el.classList.remove('active');
        });

        // Add active class to clicked thumbnail
        element.classList.add('active');

        // Update primary image
        const img = element.querySelector('img');
        document.getElementById('imgPrimaryUrl').src = img.src;
    }

    handleImageError(img) {
        img.onerror = null; // Prevent infinite loop
        img.src = '/image/default-property.jpg';
        img.classList.add('image-error');
    }

    showLoadingState() {
        $('.main-content').addClass('loading-skeleton');
    }

    hideLoadingState() {
        $('.main-content').removeClass('loading-skeleton');
    }

    showErrorMessage(message) {
        console.error(message);

        $('.main-content').html(`
            <div class="content-section">
                <div class="text-center py-8">
                    <div class="text-red-500 text-xl mb-2">⚠️</div>
                    <div class="text-gray-600">${message}</div>
                    <button onclick="location.reload()" class="mt-4 px-4 py-2 bg-orange-500 text-white rounded hover:bg-orange-600">
                        Thử lại
                    </button>
                </div>
            </div>
        `);
    }

    // Utility function for time formatting
    timeAgo(dateString) {
        if (!dateString) return '';

        const date = new Date(dateString);
        const now = new Date();
        const diffInSeconds = Math.floor((now - date) / 1000);

        if (diffInSeconds < 60) return 'Vừa xong';
        if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)} phút trước`;
        if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)} giờ trước`;
        if (diffInSeconds < 2592000) return `${Math.floor(diffInSeconds / 86400)} ngày trước`;
        if (diffInSeconds < 31536000) return `${Math.floor(diffInSeconds / 2592000)} tháng trước`;

        return `${Math.floor(diffInSeconds / 31536000)} năm trước`;
    }

    // Similar properties functionality
    async loadSimilar(page = 1) {
        try {
            const res = await fetch(`${this.urlBase}/api/Property/${this.currentId}/similar?page=${page}&pageSize=3`, {
                headers: { 'Accept': 'application/json' }
            });

            if (!res.ok) throw new Error('Similar API failed');

            const data = await res.json();
            const items = data.items || [];

            const wrap = document.getElementById('similarList');
            const empty = document.getElementById('similarEmpty');
            const pager = document.getElementById('similarPager');

            if (!items.length) {
                wrap.innerHTML = '';
                empty.classList.remove('hidden');
                pager.classList.add('hidden');
                return;
            }

            empty.classList.add('hidden');
            wrap.innerHTML = items.map(item => this.renderSimilarCard(item)).join('');

            const more = document.getElementById('seeMoreSimilar');
            if (more && items[0]?.type) {
                more.href = `/Home/Search?type=${encodeURIComponent(items[0].type)}`;
                more.classList.remove('hidden');
            }

            // Update pager info
            const totalItems = data.totalItems || 0;
            const totalPages = Math.max(1, Math.ceil(totalItems / 3));

            document.getElementById('simInfo').textContent = `Trang ${page}/${totalPages} • ${totalItems} tin`;

            const prev = document.getElementById('simPrev');
            const next = document.getElementById('simNext');
            prev.disabled = (page <= 1);
            next.disabled = (page >= totalPages);

            pager.classList.remove('hidden');

            // Bind event listeners
            if (!prev._bound) {
                prev.addEventListener('click', () => this.loadSimilar(page - 1));
                prev._bound = true;
            }
            if (!next._bound) {
                next.addEventListener('click', () => this.loadSimilar(page + 1));
                next._bound = true;
            }
        } catch (e) {
            console.error('Error loading similar properties:', e);
            document.getElementById('similarEmpty').classList.remove('hidden');
            document.getElementById('similarPager').classList.add('hidden');
        }
    }

    renderSimilarCard(property) {
        const img = property.primaryImageUrl
            ? (property.primaryImageUrl.startsWith('http') ? property.primaryImageUrl : `${this.urlBase}${property.primaryImageUrl}`)
            : '/image/default-property.jpg';
        const address = [property.street, property.ward, property.province].filter(Boolean).join(', ');
        const priceText = this.formatPrice(property.price);

        return `
          <div class="similar-card" onclick="window.location.href='/Home/Detail/${property.id}'">
            <div class="similar-image">
              <img src="${img}" onerror="propertyDetailManager.handleImageError(this)" alt="Property image"/>
            </div>
            <div class="similar-content">
              <h4 class="similar-title-text">${this.escapeHtml(property.title || 'Không có tiêu đề')}</h4>
              <div class="similar-price">${priceText}</div>
              <div class="similar-details">${property.area} m² · ${property.bedrooms} phòng</div>
              <div class="similar-address">${this.escapeHtml(address)}</div>
            </div>
          </div>`;
    }

    escapeHtml(s) {
        return (s || '').replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]));
    }

    // Placeholder for functions that might be defined elsewhere
    renderStarsBody(rating) {
        if (typeof renderStarsBody === 'function') {
            renderStarsBody(rating);
        }
    }

    // Comments are now handled by ReviewManager
    // loadComments() method removed - use window.reviewManager.loadComments() instead

    // Fallback method to load comments if ReviewManager is not available
    async loadCommentsFallback() {
        try {
            if (typeof getComment === 'function') {
                const commentsData = await getComment(this.currentId);

                // Update comment count in UI
                let commentCount = 0;
                if (commentsData && commentsData.data) {
                    commentCount = Array.isArray(commentsData.data) ? commentsData.data.length : 0;
                } else if (commentsData && Array.isArray(commentsData)) {
                    commentCount = commentsData.length;
                } else if (commentsData && commentsData.items) {
                    commentCount = Array.isArray(commentsData.items) ? commentsData.items.length : 0;
                }

                // Update both comment count displays
                this.updateElement('#totalCommentCount', commentCount);
                this.updateElement('.comment-count span span', commentCount);

                // Try to display comments if ReviewManager becomes available
                if (window.reviewManager && commentsData && commentsData.items) {
                    const transformedComments = commentsData.items.map(comment => {
                        // Transform replies if they exist
                        let transformedReplies = [];

                        // Check for single reply object (API format: reply: {...})
                        if (comment.reply && typeof comment.reply === 'object') {
                            transformedReplies = [{
                                id: comment.reply.id || 'unknown',
                                userName: 'Chủ nhà', // Since it's landlordId, we'll use a default name
                                content: comment.reply.replyContent || 'Không có nội dung',
                                createdAt: comment.reply.createdAt
                            }];
                        }
                        // Check for replies array (fallback format)
                        else if (comment.replies && Array.isArray(comment.replies)) {
                            transformedReplies = comment.replies.map(reply => ({
                                id: reply.replyId || reply.id,
                                userName: reply.userName || reply.renterName || reply.ownerName || 'Người dùng',
                                content: reply.content || reply.replyText || reply.replyContent || 'Không có nội dung',
                                createdAt: reply.createdAt
                            }));
                        }

                        return {
                            id: comment.reviewId || comment.id,
                            rating: comment.rating || 0,
                            content: comment.reviewText || comment.content || 'Không có nội dung',
                            userName: comment.renterName || comment.userName || 'Người dùng',
                            createdAt: comment.createdAt,
                            replies: transformedReplies
                        };
                    });

                    window.reviewManager.comments = transformedComments;
                    window.reviewManager.totalComments = transformedComments.length;
                    window.reviewManager.displayComments();
                }
            }
        } catch (error) {
            console.error('Error in fallback comment loading:', error);
        }
    }

    // Method to load comments with retry logic
    loadCommentsWithRetry() {
        if (window.reviewManager) {
            window.reviewManager.loadComments();
        } else {
            // Retry after a short delay
            setTimeout(() => {
                if (window.reviewManager) {
                    window.reviewManager.loadComments();
                } else {
                    // Try to load comments directly as fallback
                    this.loadCommentsFallback();
                }
            }, 500);
        }
    }



    // Report functions
    openReportModal() {
        document.getElementById('reportModal').classList.remove('hidden');
    }

    closeReportModal() {
        document.getElementById('reportModal').classList.add('hidden');
        document.getElementById('reportReason').value = '';
        document.getElementById('reportDescription').value = '';
    }

    closeReportStatusModal() {
        document.getElementById('reportStatusModal').classList.add('hidden');
    }

    ensureReportService() {
        if (!this.reportService) {
            if (window.ReportService) {
                this.reportService = new window.ReportService();
                console.log('ReportService initialized');
            } else {
                console.error('ReportService class not available');
                return false;
            }
        }
        return true;
    }

    async checkReportStatus() {
        if (!this.checkUserLogin()) {
            return;
        }

        if (!this.ensureReportService()) {
            console.error('Cannot initialize ReportService');
            return;
        }

        try {
            const data = await this.reportService.checkReportStatus(this.currentId, 'PropertyPost', this.currentUserId);
            this.updateReportButton(data);
        } catch (error) {
            console.error('Error checking report status:', error);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Không thể kiểm tra trạng thái báo cáo',
                confirmButtonText: 'Đồng ý'
            });
        }
    }

    updateReportButton(data) {
        const reportBtn = document.getElementById('reportBtn');
        const reportBtnText = document.getElementById('reportBtnText');

        if (!reportBtn || !reportBtnText) {
            console.error('Report button elements not found in updateReportButton');
            return;
        }

        console.log('Updating report button with data:', data);

        // Remove all color classes and let CSS handle styling
        reportBtn.className = 'action-btn report';

        // Always show the report button unless user is owner
        if (this.currentUserId > 0 && this.currentUserId === this.landlordId) {
            console.log('User is owner, hiding report button');
            reportBtn.style.display = 'none';
            return;
        }

        // Show the report button
        reportBtn.style.display = 'flex';

        if (this.currentUserId <= 0) {
            console.log('User not logged in, updating for guest');
            this.updateReportButtonForGuest();
            return;
        }

        if (data.hasReported && !data.canReport) {
            console.log('User has reported, cannot report again');
            reportBtnText.textContent = 'Hủy báo cáo';
            reportBtn.onclick = () => this.cancelReport();
            reportBtn.classList.add('cancel-report');
        } else if (data.hasReported && data.canReport) {
            console.log('User can report again');
            reportBtnText.textContent = 'Báo cáo lại';
            reportBtn.onclick = () => this.openReportModal();
            reportBtn.classList.add('re-report');
        } else {
            console.log('User can report for the first time');
            reportBtnText.textContent = 'Báo xấu';
            reportBtn.onclick = () => this.openReportModal();
            reportBtn.classList.add('new-report');
        }
    }

    async submitReport() {
        if (!this.checkUserLogin()) {
            return;
        }

        if (!this.ensureReportService()) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Dịch vụ báo cáo không khả dụng. Vui lòng thử lại sau.',
                confirmButtonText: 'Đồng ý'
            });
            return;
        }

        const reason = document.getElementById('reportReason').value;
        const description = document.getElementById('reportDescription').value;

        if (!reason) {
            Swal.fire({
                icon: 'warning',
                title: 'Thiếu thông tin',
                text: 'Vui lòng chọn lý do báo cáo',
                confirmButtonText: 'Đồng ý'
            });
            return;
        }

        Swal.fire({
            title: 'Đang gửi báo cáo...',
            text: 'Vui lòng chờ trong giây lát',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        try {
            const requestBody = {
                targetId: this.currentId,
                reason: reason,
                description: description
            };

            console.log('Submitting report with data:', requestBody);
            const result = await this.reportService.reportPropertyPost(this.currentUserId, requestBody);

            Swal.close();
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: result.message || 'Báo cáo đã được gửi thành công',
                confirmButtonText: 'Đồng ý'
            });

            this.closeReportModal();
            this.checkReportStatus();
        } catch (error) {
            console.error('Error submitting report:', error);
            Swal.close();

            let errorMessage = 'Có lỗi xảy ra khi gửi báo cáo';
            if (error.message) {
                errorMessage = error.message;
            } else if (error.response) {
                errorMessage = error.response.data?.message || errorMessage;
            }

            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: errorMessage,
                confirmButtonText: 'Đồng ý'
            });
        }
    }

    async cancelReport() {
        if (!this.checkUserLogin()) {
            return;
        }

        if (!this.ensureReportService()) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Dịch vụ báo cáo không khả dụng. Vui lòng thử lại sau.',
                confirmButtonText: 'Đồng ý'
            });
            return;
        }

        const result = await Swal.fire({
            title: 'Xác nhận hủy báo cáo',
            text: 'Bạn có chắc chắn muốn hủy báo cáo này?\n\nHủy báo cáo sẽ xóa hoàn toàn báo cáo của bạn và bạn có thể báo cáo lại sau này.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Hủy báo cáo',
            cancelButtonText: 'Không'
        });

        if (result.isConfirmed) {
            Swal.fire({
                title: 'Đang hủy báo cáo...',
                text: 'Vui lòng chờ trong giây lát',
                allowOutsideClick: false,
                allowEscapeKey: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            try {
                console.log('Canceling report for:', { targetId: this.currentId, userId: this.currentUserId });
                const response = await this.reportService.cancelPropertyPostReport(this.currentId, this.currentUserId);

                Swal.close();
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: response.message || 'Đã hủy báo cáo thành công',
                    confirmButtonText: 'Đồng ý'
                });

                this.checkReportStatus();
            } catch (error) {
                console.error('Error canceling report:', error);
                Swal.close();

                let errorMessage = 'Có lỗi xảy ra khi hủy báo cáo';
                if (error.message) {
                    errorMessage = error.message;
                } else if (error.response) {
                    errorMessage = error.response.data?.message || errorMessage;
                }

                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: errorMessage,
                    confirmButtonText: 'Đồng ý'
                });
            }
        }
    }

    openLandlord() {
        window.location.href = `/Home/Landlord/${this.landlordId}`;
    }
}

// Initialize when DOM is ready
$(document).ready(function () {
    // Initialize the manager
    window.propertyDetailManager = new PropertyDetailManager();
}); 