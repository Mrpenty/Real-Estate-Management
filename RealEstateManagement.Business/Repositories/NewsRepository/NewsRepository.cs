using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.NewsRepository
{
    public class NewsRepository :INewsRepository
    {
        private readonly RentalDbContext _context;

        public NewsRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<News> AddAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News.Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<News?> GetBySlugAsync(string slug)
        {
            return await _context.News.Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Slug == slug);
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News.Include(n => n.Images)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<News>> GetPublishedAsync()
        {
            return await _context.News.Include(n => n.Images)
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<News> UpdateAsync(News news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;
            news.IsPublished = true;
            news.PublishedAt = DateTime.UtcNow;
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
