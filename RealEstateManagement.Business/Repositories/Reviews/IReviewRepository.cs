using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Reviews
{
    public interface IReviewRepository
    {
        Task<bool> HasCompletedContractAsync(int propertyId, int renterId);
        Task<bool> HasReviewedAsync(int propertyId, int renterId);
        Task<Review> AddAsync(Review review);
        Task<List<Review>> GetByPropertyIdAsync(int propertyId);
    }
}
