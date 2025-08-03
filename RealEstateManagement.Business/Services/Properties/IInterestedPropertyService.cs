using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public interface IInterestedPropertyService
    {
        Task<bool> ConfirmInterestAsync(int interestedPropertyId, bool isRenter, bool confirmed);
        Task<InterestedPropertyDTO> AddInterestAsync(int renterId, int propertyId);
        Task<bool> RemoveInterestAsync(int renterId, int propertyId);
        Task<IEnumerable<InterestedPropertyDTO>> GetByRenterAsync(int renterId);
        Task<InterestedPropertyDTO> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, InterestedStatus status);
        IQueryable<InterestedPropertyDTO> QueryDTO(); // For OData
        Task<PaginatedResponseDTO<InterestedPropertyDTO>> GetPaginatedAsync(int page = 1, int pageSize = 10, int? renterId = null);

    }
}
