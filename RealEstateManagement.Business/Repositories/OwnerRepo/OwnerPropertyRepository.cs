using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.impl;
using RealEstateManagement.Business.Repositories.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class OwnerPropertyRepository : RepositoryAsync<Property>, IOwnerPropertyRepository
    {
        private readonly RentalDbContext _context;

        public OwnerPropertyRepository(RentalDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Property> GetByLandlordQueryable(int landlordId)
        {
            return _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Posts)
                .ThenInclude(p => p.RentalContract)
                .Include(p => p.Address)
                .ThenInclude(p => p.Province)
                .Include(p => p.Address)
                .ThenInclude(p => p.Street)
                .Include(p => p.Address)
                .ThenInclude(p => p.Ward)
                .Where(p => p.LandlordId == landlordId);
        }

        public async Task<Property> GetByIdAsync(int id, int landlordId)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Posts)
                    .ThenInclude(p => p.RentalContract)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId);
        }
        public async Task DeleteAsync(Property property)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(int propertyId, int provinceId, int wardId, int streetId, string detailedAddress)
        {
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
                throw new Exception("Property not found");

            // Giả sử bảng Property có cột AddressId liên kết bảng Address
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == property.AddressId);

            if (address == null)
                throw new Exception("Address not found");

            address.ProvinceId = provinceId;
            address.WardId = wardId;
            address.StreetId = streetId;
            address.DetailedAddress = detailedAddress;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAmenitiesAsync(int propertyId, List<int> amenityIds)
        {
            // Xoá tiện nghi cũ
            var existingAmenities = _context.PropertyAmenities
                .Where(pa => pa.PropertyId == propertyId);
            _context.PropertyAmenities.RemoveRange(existingAmenities);

            // Thêm tiện nghi mới
            var newAmenities = amenityIds.Select(aid => new PropertyAmenity
            {
                PropertyId = propertyId,
                AmenityId = aid
            });

            await _context.PropertyAmenities.AddRangeAsync(newAmenities);
            await _context.SaveChangesAsync();
        }

        //Lấy tất cả các bất động sản đã được cho thuê và có RentalContract.Status = Confirmed
        public async Task<List<Property>> GetRentedPropertiesByLandlordIdAsync(int landlordId)
        {
            return await _context.Properties
                .Include(p => p.Posts)
                    .ThenInclude(post => post.RentalContract)
                .Where(p => p.LandlordId == landlordId
                    && p.Posts.Any(post =>
                        post.Status == PropertyPost.PropertyPostStatus.Rented
                        && post.RentalContract != null
                        && post.RentalContract.Status == RentalContract.ContractStatus.Confirmed))
                .ToListAsync();
        }
    }

}
