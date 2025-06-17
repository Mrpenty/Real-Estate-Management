using RealEstateManagement.Business.Repositories.FavoriteProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.FavortiteProperties
{
    public class UserPreferenceService :IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _repository;

        public UserPreferenceService(IUserPreferenceRepository repository)
        {
            _repository = repository;
        }

        //public async Task<bool> AddFavoritePropertyAsync(int userId, int propertyId)
        //{
        //    return await _repository.AddFavoritePropertyAsync(userId, propertyId);
        //}
    }
}
