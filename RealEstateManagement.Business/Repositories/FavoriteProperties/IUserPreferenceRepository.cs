using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.FavoriteProperties
{
    public interface IUserPreferenceRepository
    {
        Task<bool> AddFavoritePropertyAsync(int userId, int propertyId);
    }
}
