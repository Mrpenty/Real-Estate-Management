using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public interface IPropertyPromotionService
    {
        Task<IEnumerable<ViewPropertyPromotionDTO>> GetAllAsync();
        Task<ViewPropertyPromotionDTO?> GetByIdAsync(int id);
        Task<ViewPropertyPromotionDTO> CreateAsync(CreatePropertyPromotionDTO dto, int userId);
        Task<ViewPropertyPromotionDTO?> UpdateAsync(int id, UpdatePropertyPromotionDTO dto);
        Task<bool> DeleteAsync(int id);
    }
} 