using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
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
        public async Task<RentalContract> GetContractByRenterAndPostAsync(int renterId, int propertyPostId)
        {
            return await _context.RentalContracts
                .FirstOrDefaultAsync(c => c.RenterId == renterId && c.PropertyPostId == propertyPostId);
        }
        public async Task<RentalContract> GetCompletedContractAsync(int propertyPostId, int renterId)
        {
            // Lấy tất cả contract thỏa mãn điều kiện trong DB
            var contracts = await _context.RentalContracts
                .Include(c => c.PropertyPost)
                .Where(c => c.PropertyPostId == propertyPostId
                        && c.RenterId == renterId
                        && c.Status == RentalContract.ContractStatus.Confirmed
                        && c.StartDate.HasValue)
                .ToListAsync();

            // Lọc ở ngoài C#
            return contracts.FirstOrDefault(c =>
                c.StartDate.Value.AddMonths(c.ContractDurationMonths) <= DateTime.Now
            );
        }
        public IQueryable<Review> QueryReviewsByPropertyPostId(int propertyPostId)
        {
            // Chỉ lấy review hiển thị và không bị flag.
            // Join qua Contract để lọc theo PropertyPostId.
            return _context.Reviews
                .AsNoTracking()
                .Include(r => r.Reply)
                .Include(r => r.Renter)
                .Include(r => r.Contract) // cần Contract.PropertyPostId
                .Where(r => r.IsVisible && !r.IsFlagged && r.Contract.PropertyPostId == propertyPostId);
        }
    }
}
