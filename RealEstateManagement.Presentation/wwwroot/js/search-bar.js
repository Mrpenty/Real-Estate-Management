// Search bar functions
function switchTab(tab) {
    // Remove active class from all tabs
    document.querySelectorAll('.search-tab').forEach(t => t.classList.remove('active'));
    // Add active class to clicked tab
    event.target.classList.add('active');
}

function toggleDropdown(dropdownId) {
    const dropdown = document.getElementById(dropdownId);
    const isOpen = dropdown.classList.contains('open');
    
    // Close all dropdowns
    document.querySelectorAll('.filter-dropdown').forEach(d => d.classList.remove('open'));
    
    // Toggle current dropdown
    if (!isOpen) {
        dropdown.classList.add('open');
    }
}

function resetPropertyType() {
    document.querySelector('input[name="propertyType"][value=""]').checked = true;
}

function applyPropertyType() {
    const selectedType = document.querySelector('input[name="propertyType"]:checked').value;
    // Apply filter logic here
    console.log('Property type selected:', selectedType);
    toggleDropdown('propertyTypeDropdown');
}

function resetPrice() {
    document.querySelector('input[name="priceRange"][value=""]').checked = true;
    document.getElementById('minPriceInput').value = '';
    document.getElementById('maxPriceInput').value = '';
}

function applyPrice() {
    const selectedRange = document.querySelector('input[name="priceRange"]:checked').value;
    const minPrice = document.getElementById('minPriceInput').value;
    const maxPrice = document.getElementById('maxPriceInput').value;
    // Apply filter logic here
    console.log('Price filter:', { selectedRange, minPrice, maxPrice });
    toggleDropdown('priceDropdown');
}

function resetArea() {
    document.querySelector('input[name="areaRange"][value=""]').checked = true;
    document.getElementById('minAreaInput').value = '';
    document.getElementById('maxAreaInput').value = '';
}

function applyArea() {
    const selectedRange = document.querySelector('input[name="areaRange"]:checked').value;
    const minArea = document.getElementById('minAreaInput').value;
    const maxArea = document.getElementById('maxAreaInput').value;
    // Apply filter logic here
    console.log('Area filter:', { selectedRange, minArea, maxArea });
    toggleDropdown('areaDropdown');
}

function performSearch() {
    const searchTerm = document.getElementById('searchInput').value;
    const location = document.getElementById('locationInput').value;
    const activeTab = document.querySelector('.search-tab.active').textContent;
    
    // Perform search logic here
    console.log('Searching for:', {
        searchTerm,
        location,
        activeTab
    });
    
    // You can redirect to search results page or apply filters
    // window.location.href = `/Search?q=${encodeURIComponent(searchTerm)}&location=${encodeURIComponent(location)}&type=${activeTab}`;
}

// Close dropdowns when clicking outside
document.addEventListener('click', function(event) {
    if (!event.target.closest('.filter-dropdown')) {
        document.querySelectorAll('.filter-dropdown').forEach(d => d.classList.remove('open'));
    }
});

// Initialize search bar
document.addEventListener('DOMContentLoaded', function() {
    // Set default location if available
    const savedLocation = sessionStorage.getItem('selectedLocation');
    if (savedLocation) {
        document.getElementById('locationInput').value = savedLocation;
    }
    
    // Add keyboard shortcuts
    document.getElementById('searchInput').addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            performSearch();
        }
    });
}); 