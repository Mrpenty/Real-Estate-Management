using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.NewsService
{
    public interface INewImageService
    {
        Task<NewsImage> AddImageAsync(int newId, NewImageCreateDto dto);
        Task<NewsImage> UpdateImageAsync(NewsImage updatedImage);
        Task<IEnumerable<NewsImageDto>> GetImagesByNewsIdAsync(int newId);
    }
}
