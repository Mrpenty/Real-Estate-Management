using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.NewsRepository
{
    public class NewsImageRepository : INewsImageRepository
    {
        private readonly RentalDbContext _context;
        private readonly ILogger<NewsImageRepository> _logger;

        public NewsImageRepository(RentalDbContext context, ILogger<NewsImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<NewsImage> AddImageAsync(NewsImage image)
        {
            try
            {
                _logger.LogInformation("Adding NewsImage to context: NewsId={NewsId}, URL={Url}, IsPrimary={IsPrimary}, Order={Order}",
                    image.NewsId, image.ImageUrl, image.IsPrimary, image.Order);

                _context.NewsImages.Add(image);

                _logger.LogInformation("Saving changes to database");
                await _context.SaveChangesAsync();

                _logger.LogInformation("NewsImage saved successfully with ID: {ImageId}", image.Id);
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving NewsImage to database: NewsId={NewsId}, URL={Url}",
                    image.NewsId, image.ImageUrl);
                throw;
            }
        }


        public async Task<bool> NewsExistsAsync(int newsId)
        {
            try
            {
                _logger.LogInformation("Checking if new {newsId} exists", newsId);
                var exists = await _context.News.AnyAsync(p => p.Id == newsId);
                _logger.LogInformation("new {newsId} exists: {Exists}", newsId, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if new {newsId} exists", newsId);
                throw;
            }
        }

        public async Task<NewsImage> UpdateImageAsync(NewsImage updatedImage)
        {
            try
            {

                var existingImage = await _context.NewsImages.FirstOrDefaultAsync(x => x.Id == updatedImage.Id);

                if (existingImage == null)
                {
                    _logger.LogWarning("NewsImage with ID {ImageId} not found", updatedImage.Id);
                    throw new Exception("NewsImage not found.");
                }

                existingImage.ImageUrl = updatedImage.ImageUrl;
                existingImage.IsPrimary = updatedImage.IsPrimary;
                existingImage.Order = updatedImage.Order;

                await _context.SaveChangesAsync();

                _logger.LogInformation("NewsImage with ID {ImageId} updated successfully", updatedImage.Id);
                return existingImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating NewsImage with ID: {ImageId}", updatedImage.Id);
                throw;
            }
        }
        //public async Task<List<NewsImage>> GetImagesByNewsIdAsync(int newsId)
        //{
        //    return await _context.NewsImages
        //        .Where(x => x.NewsId == newsId)
        //        .OrderBy(x => x.Order)
        //        .ToListAsync();
        //}
        public async Task<bool> HasAnyImageAsync(int newsId)
        {
            return await _context.NewsImages.AnyAsync(x => x.Id == newsId);
        }
    }
}
