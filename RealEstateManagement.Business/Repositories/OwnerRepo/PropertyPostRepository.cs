using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
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

                return post.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Ném lại ngoại lệ để xử lý ở tầng trên
            }
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
    }

}
