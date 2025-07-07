using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Package
{
    public class PromotionPackageRepository : IPromotionPackageRepository
    {
        private readonly IRepositoryAsync<PromotionPackage> _promotionPackageRepository;
        public PromotionPackageRepository(IRepositoryAsync<PromotionPackage> promotionPackageRepository)
        {
            _promotionPackageRepository = promotionPackageRepository;
        }

        public async Task AddAsync(PromotionPackage entity)
        {
            await _promotionPackageRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _promotionPackageRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<PromotionPackage>> GetAsync()
        {
            return await _promotionPackageRepository.GetAsync();
        }
        public async Task<PromotionPackage> GetByIdAsync(int id)
        {
            return await _promotionPackageRepository.GetByIdAsync(id);
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PromotionPackage entity)
        {
            await _promotionPackageRepository.UpdateAsync(entity);
        }
        

       
    }
}
