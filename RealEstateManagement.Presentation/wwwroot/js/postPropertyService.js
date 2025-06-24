const postPropertyService = {
    getToken() {
        // Ưu tiên lấy từ localStorage, fallback sang cookie
        const localToken = localStorage.getItem('authToken');
        if (localToken) return localToken;
        const cookieToken = document.cookie.split('; ').find(row => row.startsWith('accessToken='));
        return cookieToken ? cookieToken.split('=')[1] : null;
    },

    createProperty(formData) {
        return new Promise((resolve, reject) => {
            const token = this.getToken();
            if (!token) {
                reject('Không tìm thấy token xác thực. Vui lòng đăng nhập lại.');
                return;
            }
            $.ajax({
                url: '/PostProperty/CreateProperty',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                headers: {
                    Authorization: `Bearer ${token}`
                },
                xhrFields: {
                    withCredentials: true
                },
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, status, error) {
                    reject(xhr.responseText || error);
                }
            });
        });
    }
};

// Đảm bảo có thể import ở nơi khác nếu cần
window.postPropertyService = postPropertyService; 