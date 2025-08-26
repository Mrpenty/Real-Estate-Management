using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.ReportEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class PropertyPostRepository : IPropertyPostRepository
    {
        private readonly RentalDbContext _context;

        public PropertyPostRepository(RentalDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountByStatusAsync(PropertyPost.PropertyPostStatus? status)
        {
            var query = _context.PropertyPosts.AsQueryable();
            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query.CountAsync();
        }
        //Landlord tạo 1 bài đăng mới với status Draft
        public async Task<int> CreatePropertyPostAsync(Property property, PropertyPost post, List<int> amenityIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                foreach (var amenityId in amenityIds)
                {
                    _context.PropertyAmenities.Add(new PropertyAmenity
                    {
                        PropertyId = property.Id,
                        AmenityId = amenityId
                    });
                }

                post.PropertyId = property.Id;
                _context.PropertyPosts.Add(post);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return property.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Ném lại ngoại lệ để xử lý ở tầng trên
            }
        }

        //Kiểm tra Bài viết tương ứng với chủ nhà
        public async Task<PropertyPost> GetByPropertyIdAndOwnerIdAsync(int propertyId, int ownerId)
        {
            return await _context.PropertyPosts
                .FirstOrDefaultAsync(p => p.PropertyId == propertyId && p.LandlordId == ownerId);
        }
        public async Task<PropertyPost> GetByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyPosts
                .FirstOrDefaultAsync(p => p.PropertyId == propertyId);
        }

        public async Task UpdateAsync(PropertyPost post)
        {
            _context.PropertyPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PropertyPost> GetByIdAsync(int postId)
        {
            return await _context.PropertyPosts.FirstOrDefaultAsync(p => p.Id == postId);
        }

        public IQueryable<PropertyPost> GetAll()
        {
            return _context.PropertyPosts.AsQueryable();
        }
        public async Task<PropertyPost> GetPropertyPostByIdAsync(int id, int landlordId)
        {
            return await _context.PropertyPosts
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId);
        }

        public async Task UpdatePropertyAmenities(int propertyId, List<int> amenityIds)
        {
            var existing = await _context.PropertyAmenities
                .Where(a => a.PropertyId == propertyId)
                .ToListAsync();

            _context.PropertyAmenities.RemoveRange(existing);

            foreach (var id in amenityIds)
            {
                _context.PropertyAmenities.Add(new PropertyAmenity
                {
                    PropertyId = propertyId,
                    AmenityId = id
                });
            }

            await _context.SaveChangesAsync();
        }
        public async Task<PropertyPost?> GetPostWithPropertyAsync(int propertyPostId)
        {
            return await _context.PropertyPosts
                .Include(p => p.Property)
                .FirstOrDefaultAsync(p => p.Id == propertyPostId);
        }

        public async Task<List<Report>> GetReportsForPostAsync(int propertyPostId)
        {
            return await _context.Reports
                .Where(r => r.TargetType == "PropertyPost" && r.TargetId == propertyPostId)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<PropertyPost?> GetPostDetailForAdminAsync(int postId)
        {
            return await _context.PropertyPosts
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Province)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Ward)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Street)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Images)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.PropertyAmenities)
                        .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Landlord)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }
        public async Task<List<PropertyPost>> GetPostsByStatusAsync(PropertyPost.PropertyPostStatus? status, int page, int pageSize)
        {
            IQueryable<PropertyPost> query = _context.PropertyPosts
                .Include(p => p.Property)
                .Include(p => p.Landlord);  

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }

}
