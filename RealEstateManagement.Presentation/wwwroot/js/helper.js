function timeAgo(dateInput) {
    const date = new Date(dateInput);
    const now = new Date();
    const diff = now - date; 

    const msPerMinute = 60 * 1000;
    const msPerHour = msPerMinute * 60;
    const msPerDay = msPerHour * 24;
    const msPerMonth = msPerDay * 30;
    const msPerYear = msPerDay * 365;

    if (diff < msPerMinute) {
        const seconds = Math.floor(diff / 1000);
        return seconds <= 1 ? 'Vừa xong' : `${seconds} giây trước`;
    } else if (diff < msPerHour) {
        const minutes = Math.floor(diff / msPerMinute);
        return `${minutes} phút trước`;
    } else if (diff < msPerDay) {
        const hours = Math.floor(diff / msPerHour);
        return `${hours} giờ trước`;
    } else if (diff < msPerDay * 2) {
        return 'Hôm qua';
    } else if (diff < msPerMonth) {
        const days = Math.floor(diff / msPerDay);
        return `${days} ngày trước`;
    } else if (diff < msPerYear) {
        const months = Math.floor(diff / msPerMonth);
        return `${months} tháng trước`;
    } else {
        const years = Math.floor(diff / msPerYear);
        return `${years} năm trước`;
    }
}


function handleImageError(img) {
    img.onerror = null;
    img.src = './image/no-image.png'; 
}

function formatVietnameseNumber(num) {
    if (num < 1000) return num.toString();

    const units = [
        { value: 1_000_000_000, symbol: 'tỷ' },
        { value: 1_000_000, symbol: 'triệu' },
        { value: 1_000, symbol: 'nghìn' },
        { value: 100, symbol: 'trăm' },
    ];

    for (let i = 0; i < units.length; i++) {
        if (num >= units[i].value) {
            const formatted = (num / units[i].value).toFixed(1);
            return `${parseFloat(formatted)} ${units[i].symbol}`;
        }
    }

    return num.toString();
}

function formatVietnameseDateTime(dateInput) {
    const date = new Date(dateInput);

    const days = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
    const dayName = days[date.getDay()];

    const pad = n => n.toString().padStart(2, '0');

    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());
    const day = pad(date.getDate());
    const month = pad(date.getMonth() + 1);
    const year = date.getFullYear();

    return `${dayName}, ${hours}:${minutes} ${day}/${month}/${year}`;
}

const API_PROPERTY_LOCATION_BASE_URL = 'https://localhost:7031/api/Property';
async function getAllLocation() {
    try {

        const response = await fetch(`${API_PROPERTY_LOCATION_BASE_URL}/list-location`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.message || data.errorMessage || 'Get location failed');
        }

        return data;
    } catch (error) {
        console.error('Get location error:', error);
        throw error;
    }
}