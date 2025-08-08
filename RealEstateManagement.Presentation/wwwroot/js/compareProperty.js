
let compareList = [];

function getListCompare() {
    const table = document.getElementById('compare-table');
    const warning = document.getElementById('compare-warning');
    table.innerHTML = '';

    if (compareList.length < 2 || compareList.length > 3) {
        warning.classList.remove('hidden');
        return;
    } else {
        warning.classList.add('hidden');
    }

    let ids = compareList;
    $.ajax({
        url: '/PostProperty/Compare',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ids),
        success: function (data) {
            let items = JSON.parse(data.responseString);
            //console.log(items);
            const rows = [
                { label: 'Tiêu đề', key: 'title' },
                { label: 'Giá', key: 'price', highlight: 'isBestPrice', format: v => formatVietnameseNumber(v) + ' / tháng' },
                { label: 'Diện tích', key: 'area', highlight: 'isBestArea', format: v => v + ' m²' },
                { label: 'Phòng ngủ', key: 'bedrooms', highlight: 'isMostBedrooms' },
                //{ label: 'Đánh giá', key: 'totalReviews', highlight: 'IsBestRating' },
                { label: 'Lượt xem', key: 'viewsCount', highlight: 'isMostViewed' },
                { label: 'Lượt đánh giá', key: 'totalReviews', highlight: 'isBestRating' },
            ];

            rows.forEach(row => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <th class="text-left font-semibold px-2 py-2 border border-gray-200 bg-gray-50 sticky left-0">${row.label}</th>
                    ${items.map(item => {
                    const val = row.format ? row.format(item[row.key]) : item[row.key] ?? 'N/A';
                    const isBest = row.highlight && item[row.highlight];
                    return `
                            <td class="px-2 py-2 border text-center ${isBest ? 'bg-emerald-100 font-bold text-emerald-700' : ''}">
                                ${val}
                            </td>
                        `;
                }).join('')}
                `;
                table.appendChild(tr);
            });

            //const titleRow = document.createElement('tr');
            //titleRow.innerHTML = `
            //    <th class="px-2 py-2 border bg-gray-100 sticky left-0">Xoá</th>
            //    ${items.map(item => `
            //        <td class="px-2 py-2 border text-center">
            //            <button onclick="removeFromCompare(${item.id})" class="text-red-500 hover:text-red-700 text-sm underline">Xoá</button>
            //        </td>
            //    `).join('')}
            //`;
            //table.appendChild(titleRow);
        },
        error: function (xhr, status, error) {
            return null;
        }
    });
    //try {
    //    let body = { ides: compareList.join(",") };
    //    const response = await fetch(`https://localhost:7031/api/Property/compare`, {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json',
    //            'Accept': 'application/json'
    //        },
    //        body: JSON.stringify(body)
    //    });

    //    const data = await response.json();

    //    if (!response.ok) {
    //        throw new Error(data.message || data.errorMessage || 'Get failed');
    //    }

    //    console.log(data);
    //} catch (error) {
    //    console.error('Get error:', error);
    //    throw error;
    //}
}

function addToCompare(id) {
    if (compareList.indexOf(id) != -1) {
        //$('#btn-compare-'+id).html('So sánh');
        compareList = compareList.filter(ID => ID !== id);
    }
    else {
        if (compareList.length == 3) {
            alert('Quá số lượng so sánh');
            return;
        }
        compareList.push(id);
        //$('#btn-compare-' + id).html('Bỏ so sánh');
    }
    $('#card-property-' + id).toggleClass("border-orange-400", "border-gray-200");
    $('#btn-compare-' + id).toggleClass("bg-orange-600", "bg-white");
    $('#btn-compare-' + id).toggleClass("text-white", "text-gray-700");
    //alert("Đã thêm ID " + id + " vào danh sách so sánh!");
}

function getItemById(id) {
    return allItems.find(item => item.id === id);
}

function formatVietnameseNumber(number) {
    return number.toLocaleString('vi-VN');
}

function renderComparePopup() {
    getListCompare();
   //const items = compareList.map(id => getItemById(id));
}

function removeFromCompare(id) {
    const index = compareList.indexOf(id);
    if (index !== -1) {
        compareList.splice(index, 1);
        renderComparePopup();
    }
}
