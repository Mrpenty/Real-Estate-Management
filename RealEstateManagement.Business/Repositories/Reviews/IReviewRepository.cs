using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Reviews
{
    public interface IReviewRepository
    {
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<Review> GetReviewByContractAsync(int contractId, int renterId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task<ReviewReply> GetReplyByReviewIdAsync(int reviewId);
        Task AddReplyAsync(ReviewReply reply);
        Task UpdateReplyAsync(ReviewReply reply);
        Task SaveChangesAsync();
        Task<Review> GetReviewWithReplyAsync(int reviewId);
        Task<bool> HardDeleteReviewAsync(int reviewId);

    }
}
