let locationData = null;
const API_PROPERTY_LOCATION_BASE_URL = 'https://localhost:7031/api/Property';

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

async function getAllAmenities() {
    try {

        const response = await fetch(`${API_PROPERTY_LOCATION_BASE_URL}/amenities`, {
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
}

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
        locationData = data;

        if (!response.ok) {
            throw new Error(data.message || data.errorMessage || 'Get location failed');
        }

        return data;
    } catch (error) {
        console.error('Get location error:', error);
        throw error;
    }
}

function filterAdvanced() {

    //category
    const activeBtnCategory = document.querySelector('#rental-type .rental-btn.text-orange-600');
    const cateValue = activeBtnCategory.value;

    //price
    const activeBtnPrice = document.querySelector('#rental-type-price .rental-btn.text-orange-600');
    const priceMin = activeBtnPrice.dataset.priceMin;
    const priceMax = activeBtnPrice.dataset.priceMax;
    const scopePrice = activeBtnPrice.dataset.scope;
    const priceId = activeBtnPrice.id;

    //area
    const activeBtnArea = document.querySelector('#rental-type-acreage .rental-btn.text-orange-600');
    const areaMin = activeBtnArea.dataset.areaMin;
    const areaMax = activeBtnArea.dataset.areaMax;
    const scopeArea = activeBtnArea.dataset.scope;
    const areaId = activeBtnArea.id;

    //amenity
    const activeBtnAmenities = document.querySelectorAll('#rental-type-special .rental-btn.text-orange-600');
    const valueAmenities = Array.from(activeBtnAmenities).map(btn => btn.value);

    //location
    const province = $('#filterProvinceId').val();
    const ward = $('#filterWarnId').val();
    const street = $('#filterStreetId').val();

    let data = {
        province, ward, street,
        filterCategory: `${cateValue}`,
        filterPrice: `${scopePrice},${priceMin ?? 0},${priceMax ?? 0},${priceId ?? ""}`,
        filterArea: `${scopeArea},${areaMin ?? 0},${areaMax ?? 0},${areaId ?? ""}`,
        filterAmenity: valueAmenities.join(","),
        isFilter: true
    };
    window.location.href = addParamsToHref(window.location.origin, params = data);
}

async function fillDataAmenity(amenityFilter) {
    let listData = await getAllAmenities();
    let html = '';
    let amenityFilterArr = amenityFilter != "" ? amenityFilter.split(',') : [];
    listData.forEach(item => {
        if (amenityFilterArr.includes(`${item.id}`))
            html += `<button value='${item.id}' class="rental-btn border-orange-500 bg-orange-50 text-orange-600 border-2 rounded-full px-4 py-1 text-sm">${item.name}</button>`;
        else html += `<button value='${item.id}' class="rental-btn border-2 rounded-full px-4 py-1 text-sm">${item.name}</button>`;
    })
    $('#rental-type-special').html(html);
    const buttons = document.querySelectorAll(`#rental-type-special .rental-btn`);
    buttons.forEach(btn => {
        btn.addEventListener('click', () => {
            if (btn.classList.contains('border-orange-500')) {
                btn.classList.remove('border-orange-500', 'bg-orange-50', 'text-orange-600');
                btn.classList.add('border-gray-300', 'bg-white');
            }
            else {
                btn.classList.remove('border-gray-300', 'bg-white');
                btn.classList.add('border-orange-500', 'bg-orange-50', 'text-orange-600');
            }

        });
    });
}

function addParamsToHref(href, params = {}) {
    const url = new URL(href, window.location.origin);
    for (const key in params) {
        url.searchParams.set(key, params[key]);
    }
    return url.toString();
}


function getValueFromParam(locations) {
    const urlParams = new URLSearchParams(window.location.search);
    const provinceId = parseInt(urlParams.get('province'));
    const wardId = parseInt(urlParams.get('ward'));
    const streetId = parseInt(urlParams.get('street'));
    if (provinceId == 0 || provinceId == undefined || isNaN(provinceId)) return {
        valueSearch: "Tìm theo khu vực",
        valueTitle: "Kênh thông tin Bất Động Sản Việt Nam",
        selectedProvince: { id: "" }, selectedWard: { id: "" }, selectedStreet: { id: "" }
    };

    const province = locations.find(i => i.id == provinceId);
    if (wardId == 0) return {
        valueSearch: province.name ?? "",
        valueTitle: `Cho Thuê Bất Động Sản ${province.name ?? ""}`,
        selectedProvince: province, selectedWard: { id: "" }, selectedStreet: { id: "" }
    };

    let ward = province.wards.find(w => w.id == wardId);
    if (streetId == 0) return {
        valueSearch: `${ward.name ?? ""}, ${province.name ?? ""}`,
        valueTitle: `Cho Thuê Bất Động Sản ${ward.name ?? ""}`,
        selectedProvince: province, selectedWard: ward, selectedStreet: { id: "" }
    };
    let street = ward.streets.find(s => s.id == streetId);

    return {
        valueSearch: `${ward.name ?? ""}, ${province.name ?? ""}`,
        valueTitle: `Cho Thuê Bất Động Sản ${street.name ?? ""}`,
        selectedProvince: province, selectedWard: ward, selectedStreet: street
    };
}

function filterWard() {
    let selectedProvinceId = $('#filterProvinceId').val();
    let provinceItem = locationData.find(item => item.id == selectedProvinceId);
    let htmlWard = '';
    if (provinceItem.id == 0) {
        $('#filterWarnId').html(`<option value="0">All</option>`);
        $('#filterStreetId').html(`<option value="0">All</option>`);
        return;
    }
    provinceItem.wards.forEach(item => {
        htmlWard += `<option value="${item.id}">${item.name}</option>`;
    })
    $('#filterWarnId').html(htmlWard);
    $('#filterStreetId').html(`<option value="0">All</option>`);
}

function filterStreet() {
    let selectedProvinceId = $('#filterProvinceId').val();
    let selectedWardId = $('#filterWarnId').val();
    let provinceItem = locationData.find(item => item.id == selectedProvinceId);
    let wardItem = provinceItem.wards.find(i => i.id == selectedWardId);
    let html = '';
    if (wardItem.id == 0) {
        $('#filterStreetId').html(`<option value="0">All</option>`);
        return;
    }
    wardItem.streets.forEach(item => {
        html += `<option value="${item.id}">${item.name}</option>`;
    })
    $('#filterStreetId').html(html);
}


function addToFavourite(id) {

}