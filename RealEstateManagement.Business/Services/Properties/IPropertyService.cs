using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.DTO.Review;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public interface IPropertyService
    {
        Task<IEnumerable<HomePropertyDTO>> GetAllPropertiesAsync(int? userId = 0);
        Task<PaginatedResponseDTO<HomePropertyDTO>> GetPaginatedPropertiesAsync(int page = 1, int pageSize = 10, 
                int? userId = 0, string type = "", string provinces = "",string wards = "", string streets = "", 
                string keyword = "", int minPrice = 0, int maxPrice = 100,
                int minArea = 0, int maxArea = 100, int minRoom = 0,  int maxRoom = 15, string sortBy = "newest");
        Task<IEnumerable<HomePropertyDTO>> GetPropertiesByUserAsync(int? userId);
        //Lấy 1 id
        Task<PropertyDetailDTO> GetPropertyByIdAsync(int id,int userId);
        Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice);
        Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync([FromQuery] decimal? minArea, [FromQuery] decimal? maxArea);

        Task<IEnumerable<HomePropertyDTO>> FilterAdvancedAsync(PropertyFilterDTO filter);
        //So sánh property (tối đa 3)
        Task<IEnumerable<ComparePropertyDTO>> ComparePropertiesAsync(List<int> ids);

        Task<List<PropertyDetailDTO>> GetPropertiesByIdsAsync(List<int> ids);

        Task<List<ProvinceDTO>> GetListLocationAsync();

        // Address APIs
        Task<IEnumerable<Province>> GetProvincesAsync();
        Task<IEnumerable<Street>> GetStreetAsync(int wardId);
        Task<IEnumerable<Ward>> GetWardsAsync(int provinces);

        Task<IEnumerable<Amenity>> GetAmenitiesAsync();

        Task<IEnumerable<HomePropertyDTO>> FilterByTypeAsync(string type);
        // Lấy tất cả property theo landlordId
        Task<UserProfileWithPropertiesDTO?> GetUserProfileWithPropertiesAsync(int userId, int? currentId = null);
        //Gợi ý bđs tương tự
        //Task<IEnumerable<HomePropertyDTO>> SuggestSimilarPropertiesAsync(int propertyId, int take = 12, int? currentUserId = 0);

        // thêm method phân trang
        Task<PagedResultDTO<HomePropertyDTO>> SuggestSimilarPropertiesPagedAsync(
            int propertyId, int page = 1, int pageSize = 12, int? currentUserId = 0);
        /// <summary>
        /// Lấy danh sách BĐS có điểm trung bình cao nhất trong khoảng tuần (rolling 7 ngày).
        /// </summary>
        /// <param name="topN">Số lượng cần lấy (sau khi lọc minReviews)</param>
        /// <param name="minReviewsInWeek">Số lượng review tối thiểu trong tuần</param>
        /// <param name="fromUtc">Thời điểm bắt đầu (UTC). Null = UtcNow - 7 ngày</param>
        /// <param name="toUtc">Thời điểm kết thúc (UTC). Null = UtcNow</param>
        /// <param name="currentUserId">User hiện tại (để set IsFavorite). Null/0 = anonymous</param>
        Task<PagedResultDTO<WeeklyBestRatedPropertyDTO>> GetWeeklyBestRatedPropertiesPagedAsync(
            int page = 1,
            int pageSize = 12,
            int minReviewsInWeek = 1,
            DateTime? fromUtc = null,
            DateTime? toUtc = null,
            int? currentUserId = 0);

        Task<List<PropertyDetailDTO>> GetRentedPropertiesByUserAsync(int userId);
    }
}