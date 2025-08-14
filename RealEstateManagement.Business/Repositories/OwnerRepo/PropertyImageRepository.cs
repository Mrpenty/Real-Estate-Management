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

                var existingImage = await _context.PropertyImages.FirstOrDefaultAsync(x => x.PropertyId == updatedImage.PropertyId);

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

        public async Task<bool> ClearAllImagesAsync(int propertyId)
        {
            try
            {
                _logger.LogInformation("Clearing all images for property {PropertyId}", propertyId);
                
                var imagesToDelete = await _context.PropertyImages
                    .Where(img => img.PropertyId == propertyId)
                    .ToListAsync();
                
                if (!imagesToDelete.Any())
                {
                    _logger.LogInformation("No images found for property {PropertyId}", propertyId);
                    return true;
                }
                
                _logger.LogInformation("Found {Count} images to delete for property {PropertyId}", imagesToDelete.Count, propertyId);
                
                _context.PropertyImages.RemoveRange(imagesToDelete);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully cleared {Count} images for property {PropertyId}", imagesToDelete.Count, propertyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing images for property {PropertyId}", propertyId);
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(int propertyId, int imageId)
        {
            try
            {
                _logger.LogInformation("Deleting image {ImageId} for property {PropertyId}", imageId, propertyId);
                
                var imageToDelete = await _context.PropertyImages
                    .FirstOrDefaultAsync(img => img.Id == imageId && img.PropertyId == propertyId);
                
                if (imageToDelete == null)
                {
                    _logger.LogWarning("Image {ImageId} not found for property {PropertyId}", imageId, propertyId);
                    return false;
                }
                
                _context.PropertyImages.Remove(imageToDelete);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted image {ImageId} for property {PropertyId}", imageId, propertyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {ImageId} for property {PropertyId}", imageId, propertyId);
                return false;
            }
        }

    }

}
