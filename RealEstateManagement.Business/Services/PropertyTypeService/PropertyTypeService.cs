using RealEstateManagement.Business.DTO.PropertyTypeDTO;
using RealEstateManagement.Business.Repositories.PropertyTypeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.PropertyTypeService
{
    public class PropertyTypeService : IPropertyTypeService
    {
      private readonly IPropertyTypeRepository _repo;
        public PropertyTypeService(IPropertyTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PropertyTypeGet>> GetAllAsync()
        {
            var list = await _repo.GetAsync();
            return list.Select(entity => new PropertyTypeGet
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            });
        }
        public async Task<PropertyTypeGet?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            return new PropertyTypeGet
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<PropertyTypeGet> CreateAsync(PropertyTypeGet dto)
        {
            var entity = new Data.Entity.PropertyEntity.PropertyType
            {
                Name = dto.Name,
                Description = dto.Description
            };
            await _repo.AddAsync(entity);
            return new PropertyTypeGet
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<PropertyTypeGet> UpdateAsync(int id, PropertyTypeGet dto)
        {
                
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            await _repo.UpdateAsync(entity);
            return new PropertyTypeGet
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

      
    }
}
