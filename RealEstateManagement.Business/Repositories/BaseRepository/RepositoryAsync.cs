using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.impl
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly RentalDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryAsync(RentalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            IQueryable<T> query = _dbSet;
            var data = query.ToListAsync();
            return await data;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync([id])
                   ?? throw new InvalidOperationException($"Entity with ID {id} not found.");
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}