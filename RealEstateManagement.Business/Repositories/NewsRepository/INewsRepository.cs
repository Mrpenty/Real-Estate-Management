using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        Task<News> AddAsync(News news);
        Task<News?> GetByIdAsync(int id);
        Task<News?> GetBySlugAsync(string slug);
        Task<IEnumerable<News>> GetAllAsync();
        Task<IEnumerable<News>> GetPublishedAsync();
        Task<News> UpdateAsync(News news);
        Task<bool> DeleteAsync(int id);
        Task<bool> PublishAsync(int id);
    }
}
