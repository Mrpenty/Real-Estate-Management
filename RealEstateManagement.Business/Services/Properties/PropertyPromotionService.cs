using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public class PropertyPromotionService : IPropertyPromotionService
    {
        private readonly IPropertyPromotionRepository _repo;
        public PropertyPromotionService(IPropertyPromotionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ViewPropertyPromotionDTO>> GetAllAsync()
        {
            var list = await _repo.GetAsync();
            return list.Select(entity => new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true // hoặc lấy từ entity nếu có
            });
        }

        public async Task<ViewPropertyPromotionDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            return new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true // hoặc lấy từ entity nếu có
            };
        }

        public async Task<ViewPropertyPromotionDTO> CreateAsync(CreatePropertyPromotionDTO dto)
        {
            var entity = new PropertyPromotion
            {
                PropertyId = dto.PropertyId,
                PackageId = dto.PackageId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            await _repo.AddAsync(entity);
            return new ViewPropertyPromotionDTO
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PackageId = entity.PackageId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = true // hoặc lấy từ entity nếu có
            };
        }

        public async Task<ViewPropertyPromotionDTO?> UpdateAsync(int id, UpdatePropertyPromotionDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            // Nếu có trường IsActive thì cập nhật luôn
            await _repo.UpdateAsync(existing);
            return new ViewPropertyPromotionDTO
            {
                Id = existing.Id,
                PropertyId = existing.PropertyId,
                PackageId = existing.PackageId,
                StartDate = existing.StartDate,
                EndDate = existing.EndDate,
                IsActive = true // hoặc lấy từ entity nếu có
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
} 