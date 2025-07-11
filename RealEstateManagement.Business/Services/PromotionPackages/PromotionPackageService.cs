using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Data.Entity.Payment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RealEstateManagement.Business.Services.PromotionPackages
{
    public class PromotionPackageService : IPromotionPackageService
    {
        private readonly IPromotionPackageRepository _repo;
        public PromotionPackageService(IPromotionPackageRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ViewPromotionPackageDTO>> GetAllAsync()
        {
            var list = await _repo.GetAsync();
            return list.Select(entity => new ViewPromotionPackageDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                DurationInDays = entity.DurationInDays,
                Level = entity.Level,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            });
        }

        public async Task<ViewPromotionPackageDTO?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            return new ViewPromotionPackageDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                DurationInDays = entity.DurationInDays,
                Level = entity.Level,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }

        public async Task<ViewPromotionPackageDTO> CreateAsync(CreatePromotionPackageDTO dto)
        {
            var entity = new PromotionPackage
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DurationInDays = dto.DurationInDays,
                Level = dto.Level,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsActive = dto.IsActive
            };
            await _repo.AddAsync(entity);
            return new ViewPromotionPackageDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                DurationInDays = entity.DurationInDays,
                Level = entity.Level,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }

        public async Task<ViewPromotionPackageDTO?> UpdateAsync(int id, UpdatePromotionPackageDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.DurationInDays = dto.DurationInDays;
            existing.Level = dto.Level;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.IsActive = dto.IsActive;
            await _repo.UpdateAsync(existing);
            return new ViewPromotionPackageDTO
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                Price = existing.Price,
                DurationInDays = existing.DurationInDays,
                Level = existing.Level,
                CreatedAt = existing.CreatedAt,
                UpdatedAt = existing.UpdatedAt,
                IsActive = existing.IsActive
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