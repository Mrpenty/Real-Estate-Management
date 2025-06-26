using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.FavoriteRepository
{
    public interface IFavoriteRepository
    {
        Task<bool> AddFavoritePropertyAsync(int userId, int propertyId);

        Task<bool> RemoveFavoritePropertyAsync(int userId, int propertyId);
        Task<IEnumerable<Property>> AllFavoritePropertyAsync(int userId);
    }
}
