using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RealEstateManagement.Business.Repositories.Repository;

namespace RealEstateManagement.Business.Repositories.Package
{
    public class PropertyPromotionRepository : IPropertyPromotionRepository
    {
        private readonly IRepositoryAsync<PropertyPromotion> _context;
        public PropertyPromotionRepository(IRepositoryAsync<PropertyPromotion> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyPromotion>> GetAsync()
        {
            return await _context.GetAsync();
        }

        public async Task<PropertyPromotion?> GetByIdAsync(int id)
        {
            return await _context.GetByIdAsync(id);
        }

        public async Task AddAsync(PropertyPromotion entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task UpdateAsync(PropertyPromotion entity)
        {
            await _context.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await _context.DeleteAsync(id);
            }
        }
    }
}