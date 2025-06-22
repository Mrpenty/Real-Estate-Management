using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RentalDbContext _context;
        private readonly IElasticClient _elasticClient;

        public PropertyRepository(RentalDbContext context, IElasticClient elasticClient)
        {
            _context = context;
            _elasticClient = elasticClient;
        }
        //Lấy tất cả property
        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .Select(p => new
                {
                    Property = p,
                    // Nếu có bất kỳ gói khuyến mãi nào, lấy mức cao nhất; nếu không thì bằng 0.
                    PromotionLevel = p.PropertyPromotions.Any()
                                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }
        //Lấy property theo Id
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p=>p.Posts)
                    .ThenInclude(Pa=>Pa.RentalContract)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        //Sắp xếp theo giá
        public async Task<IEnumerable<Property>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }
        //Sắp xếp theo diện tích
        public async Task<IEnumerable<Property>> FilterByAreaAsync(decimal? minArea, decimal? maxArea)
        {
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (minArea.HasValue)
                query = query.Where(p => p.Area >= minArea.Value);
            if (maxArea.HasValue)
                query = query.Where(p => p.Area <= maxArea.Value);

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }

        //Sắp xếp nâng cao
        public async Task<IEnumerable<Property>> FilterAdvancedAsync(PropertyFilterDTO filter)
        {
            var query = _context.Properties
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (filter.MinPrice.HasValue)
            {
                filter.MinPrice = filter.MinPrice * 1000000;
                if (filter.ScopePrice == "higher") query = query.Where(p => p.Price > filter.MinPrice.Value);
                else query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
                
            if (filter.MaxPrice.HasValue)
            {
                filter.MaxPrice = filter.MaxPrice * 1000000;
                if (filter.ScopePrice == "lower") query = query.Where(p => p.Price < filter.MaxPrice.Value);
                else query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
                
            if (filter.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            if (filter.MaxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);

            if (filter.MinArea.HasValue)
            {
                if(filter.ScopeArea == "higher") query = query.Where(p => p.Area > filter.MinArea.Value);
                else query = query.Where(p => p.Area >= filter.MinArea.Value);
            }
                
            if (filter.MaxArea.HasValue)
            {
                if (filter.ScopeArea == "lower") query = query.Where(p => p.Area < filter.MaxArea.Value);
                else query = query.Where(p => p.Area <= filter.MaxArea.Value);
            }
                

            if (filter.AmenityName != null
               && filter.AmenityName.Any(name => !string.IsNullOrWhiteSpace(name) && name.ToLower() != "string"))
            {
                var validNames = filter.AmenityName
                    .Where(name => !string.IsNullOrWhiteSpace(name) && name.ToLower() != "string")
                    .Select(name => name.ToLower())
                    .ToList();

                query = query.Where(p =>
                    p.PropertyAmenities.Any(pa => validNames.Contains(pa.Amenity.Name.ToLower())));
            }

            if (!string.IsNullOrEmpty(filter.Location))
            {
                var arrLocation = filter.Location.Split(",");
                var provinceId = int.Parse(arrLocation[0]);
                var wardId = int.Parse(arrLocation[1]);
                var streetId = int.Parse(arrLocation[2]);
                if (provinceId != 0) query = query.Where(p => p.Address.PropertyId == provinceId);
                if (wardId != 0) query = query.Where(p => p.Address.WardId == wardId);
                if (streetId != 0) query = query.Where(p => p.Address.StreetId == streetId);
            }

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }


        //So sánh property
        public async Task<IEnumerable<Property>> ComparePropertiesAsync(List<int> ids)
        {
            return await _context.Properties
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .ToListAsync();
        }

        //Lấy nhiều property để so sánh
        public async Task<List<Property>> GetPropertiesByIdsAsync(List<int> ids)
        {
            return await _context.Properties
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Reviews)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .ToListAsync();
        }
        public async Task<bool> AddFavoritePropertyAsync(int userId, int propertyId)
        {
            // Kiểm tra User có tồn tại không
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Kiểm tra Property có tồn tại không
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return false;

            // Kiểm tra đã tồn tại chưa
            var exists = await _context.UserFavoriteProperties
                .AnyAsync(fp => fp.UserId == userId && fp.PropertyId == propertyId);
            if (exists) return false;

            // Thêm mới
            var favorite = new UserFavoriteProperty
            {
                UserId = userId,
                PropertyId = propertyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserFavoriteProperties.Add(favorite);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> IndexPropertyAsync(PropertySearchDTO dto)
        {
            var response = await _elasticClient.IndexDocumentAsync(dto);
            return response.IsValid;
        }
        public async Task BulkIndexPropertiesAsync(IEnumerable<PropertySearchDTO> properties)
        {
            await _elasticClient.BulkAsync(b => b.IndexMany(properties));
        }
        public async Task<List<int>> SearchPropertyIdsAsync(string keyword)
        {
            ISearchResponse<PropertySearchDTO> response;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                response = await _elasticClient.SearchAsync<PropertySearchDTO>(s => s
                    .Query(q => q.MatchAll())
                );
            }
            else
            {
                response = await _elasticClient.SearchAsync<PropertySearchDTO>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                s => s.MultiMatch(m => m
                                    .Fields(f => f
                                        .Field(p => p.Title)
                                        .Field(p => p.Type)
                                        .Field(p => p.AddressId)
                                        .Field(p => p.Amenities)
                                    )
                                    .Query(keyword)
                                    .Fuzziness(Fuzziness.Auto)
                                ),
                                s => s.Prefix(p => p.Title, keyword.ToLower())  // hỗ trợ từ khóa ngắn
                            )
                        )
                    )

                );
            }

            if (!response.IsValid)
                throw new Exception($"Search failed: {response.OriginalException.Message}");

            return response.Documents.Select(d => d.Id).ToList();
        }

        public async Task<List<ProvinceDTO>> GetListLocationAsync()
        {
            var streets = await _context.Streets.ToListAsync();
            var wards = await _context.Wards.ToListAsync();
            var provinces = await _context.Provinces.ToListAsync();

            provinces.Add(new Province
            {
                Id = 0,
                Name = "All"
            });

            wards.Add(new Ward
            {
                Id = 0,
                Name = "All"
            });

            streets.Add(new Street
            {
                Id = 0,
                Name = "All"
            });

            provinces = provinces.OrderBy(p => p.Id).ToList();
            wards = wards.OrderBy(p => p.Id).ToList();
            streets = streets.OrderBy(p => p.Id).ToList();

            var result = provinces.Select(c => new ProvinceDTO
            {
                Id = c.Id,
                Name = c.Name,
                Wards = wards.Where(w => w.ProvinceId == c.Id || w.Id == 0).Select(w => new WardDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    Streets = streets.Where(s => s.WardId == w.Id || s.Id == 0).Select(s => new StreetDTO
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                }).ToList(),
            }).ToList();
            return result;
        }

    }
}
