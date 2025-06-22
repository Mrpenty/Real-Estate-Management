using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Favorite
{
    public interface IFavoriteService
    {
        Task<bool> AddToFavoriteAsync(int userId, int propertyId);
        Task<bool> RemoveFavoritePropertyAsync(int userId, int propertyId);
        Task<IEnumerable<HomePropertyDTO>> AllFavoritePropertyAsync(int userId);
    }
}
