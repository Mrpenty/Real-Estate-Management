using RealEstateManagement.Business.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.NewsService
{
    public interface INewsService
    {
        Task<int> CreateAsync(NewsCreateDto dto);
        Task<NewsViewDto?> GetByIdAsync(int id);
        Task<NewsViewDto?> GetBySlugAsync(string slug);
        Task<IEnumerable<NewsViewDto>> GetAllAsync();
        Task<IEnumerable<NewsViewDto>> GetPublishedAsync();
        Task<bool> UpdateAsync(NewsUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> PublishAsync(int id);
    }
}
