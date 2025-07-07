using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.PromotionPackages
{
    public interface IPromotionPackageService
    {
        Task<IEnumerable<ViewPromotionPackageDTO>> GetAllAsync();
        Task<ViewPromotionPackageDTO?> GetByIdAsync(int id);
        Task<ViewPromotionPackageDTO> CreateAsync(CreatePromotionPackageDTO dto);
        Task<ViewPromotionPackageDTO?> UpdateAsync(int id, UpdatePromotionPackageDTO dto);
        Task<bool> DeleteAsync(int id);
    }
} 