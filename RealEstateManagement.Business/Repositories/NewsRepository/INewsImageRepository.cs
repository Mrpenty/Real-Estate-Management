using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.NewsRepository
{
    public interface INewsImageRepository 
    {
        Task<NewsImage> AddImageAsync(NewsImage image);
        Task<NewsImage> UpdateImageAsync(NewsImage updatedImage);
        Task<bool> NewsExistsAsync(int newsId);
        Task<bool> HasAnyImageAsync(int newsId);
        //Task<List<NewsImage>> GetImagesByNewsIdAsync(int newsId);
    }
}
