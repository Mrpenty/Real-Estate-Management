using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Business.Repositories.PropertyTypeRepository
{
    public class  PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly IRepositoryAsync<PropertyType> _propertyTypeRepository;
        public PropertyTypeRepository(IRepositoryAsync<PropertyType> propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }
        public async Task AddAsync(PropertyType entity)
        {
            await _propertyTypeRepository.AddAsync(entity);
        }
        public async Task DeleteAsync(int id)
        {
            await _propertyTypeRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<PropertyType>> GetAsync()
        {
            return await _propertyTypeRepository.GetAsync();
        }
        public async Task<PropertyType> GetByIdAsync(int id)
        {
            return await _propertyTypeRepository.GetByIdAsync(id);
        }
        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        public async Task UpdateAsync(PropertyType entity)
        {
            await _propertyTypeRepository.UpdateAsync(entity);
        }

    }
}
