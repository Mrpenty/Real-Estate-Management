using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Reviews
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly RentalDbContext _context;
        public ReviewRepository(RentalDbContext context) => _context = context;

        public Task<Review> GetReviewByIdAsync(int reviewId) =>
            _context.Reviews.Include(r => r.Reply)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        public Task<Review> GetReviewByContractAsync(int contractId, int renterId) =>
            _context.Reviews
                .FirstOrDefaultAsync(r => r.ContractId == contractId && r.RenterId == renterId);

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            return Task.CompletedTask;
        }

        public Task<ReviewReply> GetReplyByReviewIdAsync(int reviewId) =>
            _context.ReviewReplies.FirstOrDefaultAsync(r => r.ReviewId == reviewId);

        public async Task AddReplyAsync(ReviewReply reply)
        {
            await _context.ReviewReplies.AddAsync(reply);
        }

        public Task UpdateReplyAsync(ReviewReply reply)
        {
            _context.ReviewReplies.Update(reply);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        public async Task<Review> GetReviewWithReplyAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Reply)
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<bool> HardDeleteReviewAsync(int reviewId)
        {
            var review = await GetReviewWithReplyAsync(reviewId);
            if (review == null)
                return false;

            if (review.Reply != null)
                _context.ReviewReplies.Remove(review.Reply);
            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
