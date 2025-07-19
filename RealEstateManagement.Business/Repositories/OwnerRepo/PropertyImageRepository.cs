using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly RentalDbContext _context;
        private readonly ILogger<PropertyImageRepository> _logger;

        public PropertyImageRepository(RentalDbContext context, ILogger<PropertyImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PropertyImage> AddImageAsync(PropertyImage image)
        {
            try
            {
                _logger.LogInformation("Adding PropertyImage to context: PropertyId={PropertyId}, URL={Url}, IsPrimary={IsPrimary}, Order={Order}", 
                    image.PropertyId, image.Url, image.IsPrimary, image.Order);
                
                _context.PropertyImages.Add(image);
                
                _logger.LogInformation("Saving changes to database");
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("PropertyImage saved successfully with ID: {ImageId}", image.Id);
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving PropertyImage to database: PropertyId={PropertyId}, URL={Url}", 
                    image.PropertyId, image.Url);
                throw;
            }
        }

        public async Task<bool> PropertyExistsAsync(int propertyId)
        {
            try
            {
                _logger.LogInformation("Checking if property {PropertyId} exists", propertyId);
                var exists = await _context.Properties.AnyAsync(p => p.Id == propertyId);
                _logger.LogInformation("Property {PropertyId} exists: {Exists}", propertyId, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if property {PropertyId} exists", propertyId);
                throw;
            }
        }

        public async Task<PropertyImage> UpdateImageAsync(PropertyImage updatedImage)
        {
            try
            {

                var existingImage = await _context.PropertyImages.FirstOrDefaultAsync(x => x.PropertyId == updatedImage.PropertyId && x.Id == updatedImage.Id);

                if (existingImage == null)
                {
                    _logger.LogWarning("PropertyImage with ID {ImageId} not found", updatedImage.PropertyId);
                    throw new Exception("PropertyImage not found.");
                }

                existingImage.Url = updatedImage.Url;
                existingImage.IsPrimary = updatedImage.IsPrimary;
                existingImage.Order = updatedImage.Order;

                await _context.SaveChangesAsync();

                _logger.LogInformation("PropertyImage with ID {ImageId} updated successfully", updatedImage.PropertyId);
                return existingImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PropertyImage with ID: {ImageId}", updatedImage.PropertyId);
                throw;
            }
        }

        public async Task<bool> HasAnyImage(int propertyId)
        {
            return await _context.PropertyImages.AnyAsync(x => x.PropertyId == propertyId);
        }

    }

}
